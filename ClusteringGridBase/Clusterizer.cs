using System.Runtime.CompilerServices;
using ClusteringModels;
using ClusteringModels.Results;

// ReSharper disable UnusedMember.Global

namespace ClusteringGridBase;

/// <summary>
/// Run a Grid based cluster algorithm
/// </summary>
public static class Clusterizer
{
    public const int NUMBER_PASSES_LIMIT = 6;

    /// <summary>
    /// Run the cluster on given points using a grid based method BUT SUPPOSE THAT THE AOI IS A CUBE THAT WILL BE SPLIT EVENLY: the given AOI will be split into smaller cube,
    /// then the density of each will be computed and then, they will be aggregated between neighbours with less than a given N distance
    /// </summary>
    /// <param name="points">all points to cluster</param>
    /// <param name="origin">the origin of the AOI (It is the corner with minimum x, y and z of the 3D cube)</param>
    /// <param name="cubeLenght">the lenght the Cube</param>
    /// <param name="nbCubePart">the number of part in which the cube will be split in each direction</param>
    /// <param name="clusteringDensityThreshold">the density above which elements are clustered </param>
    /// <param name="neighbouringMergingDistance">the number of execution passes that will be done to check if another neighbour cell should be merged</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ClusterGlobalResult<T> RunWithCubeAsAoi<T>(
        IEnumerable<T> points,
        IPoint origin,
        double cubeLenght,
        int nbCubePart,
        int clusteringDensityThreshold,
        int neighbouringMergingDistance) where T : IPoint
    {
        return Run(points, origin, cubeLenght, cubeLenght, cubeLenght, nbCubePart, nbCubePart, nbCubePart, clusteringDensityThreshold, neighbouringMergingDistance);
    }

    /// <summary>
    /// Run the cluster on given points using a grid based method: the given AOI will be split into smaller polygon 3D,
    /// then the density of each will be computed and then, they will be aggregated between neighbours with less than a given N distance
    /// </summary>
    /// <param name="points">all points to cluster</param>
    /// <param name="origin">the origin of the AOI (It is the corner with minimum x, y and z of the 3D polygon)</param>
    /// <param name="xLenght">the lenght in X direction of the polygon3D</param>
    /// <param name="yLenght">the lenght in Y direction of the polygon3D</param>
    /// <param name="zLenght">the lenght in Z direction of the polygon3D</param>
    /// <param name="nbPartX">the number of part in which the X direction will be split</param>
    /// <param name="nbPartY">the number of part in which the Y direction will be split</param>
    /// <param name="nbPartZ">the number of part in which the Z direction will be split</param>
    /// <param name="clusteringDensityThreshold">the density above which elements are clustered </param>
    /// <param name="neighbouringMergingDistance">the number of execution passes that will be done to check if another neighbour cell should be merged</param>
    public static ClusterGlobalResult<T> Run<T>(
        IEnumerable<T> points,
        IPoint origin,
        double xLenght,
        double yLenght,
        double zLenght,
        int nbPartX,
        int nbPartY,
        int nbPartZ,
        int clusteringDensityThreshold,
        int neighbouringMergingDistance) where T : IPoint
    {
        if (neighbouringMergingDistance < 1 || nbPartX < 1 || nbPartY < 1 || nbPartZ < 1 || clusteringDensityThreshold < 1)
        {
            throw new ArgumentException("neighbouringMergingDistance, nbPartX, nbPartY, nbPartZ, clusteringDensityThreshold and nbColumnTargeted should all be greater than zero");
        }

        if (neighbouringMergingDistance > NUMBER_PASSES_LIMIT)
        {
            throw new ArgumentException($"A numberOfPasses greater than {NUMBER_PASSES_LIMIT} will be slow, may be less accurate and will not lead to better result (asked numberOfPasses = {neighbouringMergingDistance})");
        }

        var grid = new ResultGrid<T>(new Cube(origin, xLenght, yLenght, zLenght), nbPartX, nbPartY, nbPartZ);
        grid.AddPoints(points);

        return grid.ComputeClusters(clusteringDensityThreshold, neighbouringMergingDistance);
    }


    private sealed class ResultGrid<T> where T : IPoint
    {
        private readonly int _nbPartX;
        private readonly int _nbPartY;
        private readonly int _nbPartZ;
        private readonly Cube _aoi;
        private readonly DensityCell<T>[,,] _grid;
        private readonly double _cellXSize;
        private readonly double _cellYSize;
        private readonly double _cellZSize;

        public ResultGrid(Cube aoi, int nbPartX, int nbPartY, int nbPartZ)
        {
            _nbPartX = nbPartX;
            _nbPartY = nbPartY;
            _nbPartZ = nbPartZ;
            _aoi = aoi;
            _grid = GetInitializedGrid(nbPartX, nbPartY, nbPartZ);
            // TODO PBO => UNIT TESTS if Aoi not zero based
            _cellXSize = aoi.XSize / nbPartX;
            _cellYSize = aoi.YSize / nbPartY;
            _cellZSize = aoi.ZSize / nbPartZ;
        }

        private static DensityCell<T>[,,] GetInitializedGrid(int nbPartX, int nbPartY, int nbPartZ)
        {
            var grid = new DensityCell<T>[nbPartX, nbPartY, nbPartZ];
            for (var i = 0; i < nbPartX; i++)
            {
                for (var j = 0; j < nbPartY; j++)
                {
                    for (var k = 0; k < nbPartZ; k++)
                    {
                        grid[i, j, k] = new DensityCell<T>();
                    }
                }
            }

            return grid;
        }

        public void AddPoints(IEnumerable<T> points)
        {
            foreach (var point in points)
            {
                if (_aoi.Contains(point.X, point.Y, point.Z))
                {
                    // target column if the truncated part of x divided by cell X Size
                    var targetCellX = (int)(point.X / _cellXSize);
                    var targetCellY = (int)(point.Y / _cellYSize);
                    var targetCellZ = (int)(point.Z / _cellZSize);
                    _grid[targetCellX, targetCellY, targetCellZ].AddPoint(point);
                }
            }
        }

        public ClusterGlobalResult<T> ComputeClusters(int clusteringDensityThreshold, int numberOfPasses)
        {
            var superClusterGrid = new DensityCell<T>?[_nbPartX, _nbPartY, _nbPartZ];

            // first compute cluster with clusteringDensityThreshold
            var unClusteredPoint = new List<T>();

            for (var i = 0; i < _nbPartX; i++)
            {
                for (var j = 0; j < _nbPartY; j++)
                {
                    for (var k = 0; k < _nbPartZ; k++)
                    {
                        var cell = _grid[i, j, k];
                        if (cell.Density >= clusteringDensityThreshold)
                        {
                            // add it to the supperCluster grid
                            superClusterGrid[i, j, k] = cell;
                        }
                        else
                        {
                            unClusteredPoint.AddRange(cell.GetPoints());
                        }
                    }
                }
            }

            var neighbouringDistance = numberOfPasses - 1;
            if (neighbouringDistance != 0)
            {
                // then merge closest cells the expected number of times:
                // go through the cluster grid:
                for (var i = 0; i < _nbPartX; i++)
                {
                    for (var j = 0; j < _nbPartY; j++)
                    {
                        for (var k = 0; k < _nbPartZ; k++)
                        {
                            // retrieve the cluster (or null if no clusters)
                            var potentialCluster = superClusterGrid[i, j, k];
                            // do not touch null (not a cluster) nor already merged cluster (to avoid merging multiple time the same cluster)
                            if (potentialCluster is { HasBeenMerged: false })
                            {
                                // try to merge with cluster around
                                for (var ii = -neighbouringDistance; ii <= neighbouringDistance; ii++)
                                {
                                    for (var jj = -neighbouringDistance; jj <= neighbouringDistance; jj++)
                                    {
                                        for (var kk = -neighbouringDistance; kk <= neighbouringDistance; kk++)
                                        {
                                            var xIndex = i + ii;
                                            var yIndex = j + jj;
                                            var zIndex = k + kk;
                                            // search neighbours for cluster that are within bounds
                                            if (xIndex >= 0 && yIndex >= 0 && zIndex >= 0 && xIndex < _nbPartX && yIndex < _nbPartY && zIndex < _nbPartZ)
                                            {
                                                var neighbour = superClusterGrid[xIndex, yIndex, zIndex];
                                                // if there is a neighbour
                                                if (neighbour != null
                                                    // AND it is not already itself (after a merge)
                                                    && !ReferenceEquals(neighbour, potentialCluster)
                                                    // AND it has no been merged already (IMPORTANT: this is done to avoid
                                                    // that chains of clusters are merged together with a reach of more than the neighbour distance
                                                    // => once a cluster has been merged, we do not allow a neighbour to merge with it again)
                                                    && !neighbour.HasBeenMerged)
                                                {
                                                    // merge them:
                                                    potentialCluster.Merge(neighbour);
                                                    // and use the potential cluster as the neighbour itself
                                                    superClusterGrid[xIndex, yIndex, zIndex] = potentialCluster;
                                                }
                                            }
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

        private IReadOnlyList<ClusterResult<T>> ExtractRemainingCluster(DensityCell<T>?[,,] superClusterGrid)
        {
            // use a hashset for filtering clusters (there is duplicates after fusing
            var remainingClusters = new HashSet<DensityCell<T>>();
            for (var i = 0; i < _nbPartX; i++)
            {
                for (var j = 0; j < _nbPartY; j++)
                {
                    for (var k = 0; k < _nbPartZ; k++)
                    {
                        var cluster = superClusterGrid[i, j, k];
                        if (cluster != null)
                        {
                            remainingClusters.Add(cluster);
                        }
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