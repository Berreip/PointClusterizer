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
    /// <summary>
    /// Run the cluster on given points
    /// </summary>
    /// <param name="points">all points to cluster</param>
    /// <param name="aoi">the targeted Aoi</param>
    /// <param name="nbRowTargeted">the final wanted row number</param>
    /// <param name="nbColumnTargeted">the final wanted colum</param>
    /// <param name="clusteringDensityThreshold">the density (for final cell) above which elements are clusterized </param>
    /// <param name="numberOfPasses">the number of execution passes that will be done (each passes use a cell twice smaller than the previous one)</param>
    public static ClusterGlobalResult<T> Run<T>(IEnumerable<T> points,
        Rectangle aoi,
        int nbRowTargeted,
        int nbColumnTargeted,
        int clusteringDensityThreshold,
        int numberOfPasses) where T : IPoint
    {
        if (numberOfPasses < 1 || nbRowTargeted < 1 || nbColumnTargeted < 1 || clusteringDensityThreshold < 1)
        {
            throw new ArgumentException("numberOfPasses, nbRowTargeted, clusteringDensityThreshold and nbColumnTargeted should all be greater than zero");
        }
        
        // Grid bases algorithm calculate the density of grid cells with a resulting target of nbRowTargeted x nbColumnTargeted
        // so depending on the number of passes, the cells will be smaller

        var grid = new ResultGrid<T>(nbRowTargeted, nbColumnTargeted, aoi);
        grid.AddPoints(points);

        return grid.ComputeClusters(clusteringDensityThreshold);
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

        public ClusterGlobalResult<T> ComputeClusters(int clusteringDensityThreshold)
        {
            var clusters = new List<ClusterResult<T>>();
            var unClusteredPoint = new List<T>();
            for (var i = 0; i < _nbColumnTargeted; i++)
            {
                for (var j = 0; j < _nbRowTargeted; j++)
                {
                    var cell = _grid[i, j];
                    if (cell.Density >= clusteringDensityThreshold)
                    {
                        clusters.Add(new ClusterResult<T>(cell.GetPoints()));
                    }
                    else
                    {
                        unClusteredPoint.AddRange(cell.GetPoints());
                    }
                }
            }

            return new ClusterGlobalResult<T>(clusters, unClusteredPoint);
        }
    }
    
    private sealed class DensityCell<T> where T : IPoint
    {
        private readonly List<T> _points = new List<T>();
        public int Density => _points.Count;

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
