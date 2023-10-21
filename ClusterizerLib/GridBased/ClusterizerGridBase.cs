using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ClusterizerLib.Results;

namespace ClusterizerLib.GridBased;

/// <summary>
/// Run a Grid based cluster algorithm
/// </summary>
public static class ClusterizerGridBase
{
    public const int NUMBER_PASSSES_LIMIT = 6;

    /// <summary>
    /// Run the cluster on given points
    /// </summary>
    /// <param name="points">all points to cluster</param>
    /// <param name="aoi">the targeted Aoi</param>
    /// <param name="nbRowTargeted">the grid rows number</param>
    /// <param name="nbColumnTargeted">the grid columns number</param>
    /// <param name="clusteringDensityThreshold">the density above which elements are clusterized </param>
    /// <param name="neighbouringMergingDistance">the number of execution passes that will be done to check if another neighbour cell should be merged</param>
    public static ClusterGlobalResult<T> Run<T>(IEnumerable<T> points,
        Rectangle aoi,
        int nbRowTargeted,
        int nbColumnTargeted,
        int clusteringDensityThreshold,
        int neighbouringMergingDistance) where T : IPoint
    {
        if (neighbouringMergingDistance < 1 || nbRowTargeted < 1 || nbColumnTargeted < 1 || clusteringDensityThreshold < 1)
        {
            throw new ArgumentException("numberOfPasses, nbRowTargeted, clusteringDensityThreshold and nbColumnTargeted should all be greater than zero");
        }

        if (neighbouringMergingDistance > NUMBER_PASSSES_LIMIT)
        {
            throw new ArgumentException($"A numberOfPasses greater than {NUMBER_PASSSES_LIMIT} will be slow, may be less accurate and will not lead to better result (asked numberOfPasses = {neighbouringMergingDistance})");
        }

        var grid = new ResultGrid<T>(nbRowTargeted, nbColumnTargeted, aoi);
        grid.AddPoints(points);

        return grid.ComputeClusters(clusteringDensityThreshold, neighbouringMergingDistance);
    }


    private sealed class ResultGrid<T> where T : IPoint
    {
        private readonly int _nbRowTargeted;
        private readonly int _nbColumnTargeted;
        private readonly Rectangle _aoi;
        private readonly DensityCell<T>[,] _grid;
        private readonly double _cellWidth;
        private readonly double _cellHeight;

        public ResultGrid(int nbRowTargeted, int nbColumnTargeted, Rectangle aoi)
        {
            _nbRowTargeted = nbRowTargeted;
            _nbColumnTargeted = nbColumnTargeted;
            _aoi = aoi;
            _grid = GetInitializedGrid(nbRowTargeted, nbColumnTargeted);
            // TODO PBO => not working if Aoi not zero based => improve later
            _cellWidth = aoi.Width / (double)nbColumnTargeted;
            _cellHeight = aoi.Height / (double)nbRowTargeted;
        }

        private static DensityCell<T>[,] GetInitializedGrid(int nbRowTargeted, int nbColumnTargeted)
        {
            var grid = new DensityCell<T>[nbColumnTargeted, nbRowTargeted];
            for (var i = 0; i < nbColumnTargeted; i++)
            {
                for (var j = 0; j < nbRowTargeted; j++)
                {
                    grid[i, j] = new DensityCell<T>();
                }
            }

            return grid;
        }

        public void AddPoints(IEnumerable<T> points)
        {
            foreach (var point in points)
            {
                var xRounded = (int)Math.Round(point.X);
                var yRounded = (int)Math.Round(point.Y);
                if (_aoi.Contains(xRounded, yRounded))
                {
                    // target column if the truncated part of x divided by cell width
                    var targetColumn = (int)(xRounded / _cellWidth);
                    var targetRow = (int)(yRounded / _cellHeight);
                    _grid[targetColumn, targetRow].AddPoint(point);
                }
            }
        }

        public ClusterGlobalResult<T> ComputeClusters(int clusteringDensityThreshold, int numberOfPasses)
        {
            var superClusterGrid = new DensityCell<T>[_nbColumnTargeted, _nbRowTargeted];

            // first compute cluster with clusteringDensityThreshold
            var unClusteredPoint = new List<T>();

            for (var i = 0; i < _nbColumnTargeted; i++)
            {
                for (var j = 0; j < _nbRowTargeted; j++)
                {
                    var cell = _grid[i, j];
                    if (cell.Density >= clusteringDensityThreshold)
                    {
                        // add it to the supperCluster grid
                        superClusterGrid[i, j] = cell;
                    }
                    else
                    {
                        unClusteredPoint.AddRange(cell.GetPoints());
                    }
                }
            }

            var neighbouringDistance = numberOfPasses -1;
            if (neighbouringDistance != 0)
            {
                // then merge closest cells the expected number of times
                for (var i = 0; i < _nbColumnTargeted; i++)
                {
                    for (var j = 0; j < _nbRowTargeted; j++)
                    {
                        var potentialCluster = superClusterGrid[i, j];
                        // do not touch null or already merged cluster
                        if (potentialCluster is { HasBeenMerged: false })
                        {
                            // try to merge with cluster around
                            for (var ii = -neighbouringDistance; ii <= neighbouringDistance; ii++)
                            {
                                for (var jj = -neighbouringDistance; jj <= neighbouringDistance; jj++)
                                {
                                    var xIndex = i + ii;
                                    var yIndex = j + jj;
                                    // search neighbours for cluster that are within bounds
                                    if (xIndex >= 0 && yIndex >= 0 && xIndex < _nbColumnTargeted && yIndex < _nbRowTargeted)
                                    {
                                        var neighbour = superClusterGrid[xIndex, yIndex];
                                        // if there is a neighbour 
                                        if (neighbour != null
                                            // AND it is not already itself (after a merge)
                                            && !ReferenceEquals(neighbour, potentialCluster)
                                            // AND it has no been merged already (IMPORTANT: this is done to avoid that chains of clusters are merged together with a reach of more than numberOfPasses => once a cluster has been merged, we do not allow a neighbour to merge with it again)
                                            && !neighbour.HasBeenMerged)
                                        {
                                            // merge them:
                                            potentialCluster.Merge(neighbour);
                                            // and use the potential cluster as the neighbour itself
                                            superClusterGrid[xIndex, yIndex] = potentialCluster;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // after merging neighbours, retrieve all remaining clusters
            var clusters = ExtractRemainingCluster(superClusterGrid);
            return new ClusterGlobalResult<T>(clusters, unClusteredPoint);
        }

        private IReadOnlyList<ClusterResult<T>> ExtractRemainingCluster(DensityCell<T>[,] superClusterGrid)
        {
            // use a hashset for filtering clusters (there is duplicates after fusing
            var remainingClusters = new HashSet<DensityCell<T>>();
            for (var i = 0; i < _nbColumnTargeted; i++)
            {
                for (var j = 0; j < _nbRowTargeted; j++)
                {
                    var cluster = superClusterGrid[i, j];
                    if (cluster != null)
                    {
                        remainingClusters.Add(cluster);
                    }
                }
            }

            return remainingClusters.Select(c => new ClusterResult<T>(c.GetPoints())).ToArray();
        }
    }

    private sealed class DensityCell<T> where T : IPoint
    {
        private readonly List<T> _points = new List<T>();
        public int Density => _points.Count;
        public bool HasBeenMerged { get; private set; }

        public void Merge(DensityCell<T> neighbour)
        {
            _points.AddRange(neighbour.GetPoints());
            HasBeenMerged = true;
        }

        public void AddPoint(T point)
        {
            _points.Add(point);
        }

        public IReadOnlyList<T> GetPoints()
        {
            return _points.ToArray();
        }

        public override string ToString()
        {
            return $"{_points.Count} points";
        }
    }
}