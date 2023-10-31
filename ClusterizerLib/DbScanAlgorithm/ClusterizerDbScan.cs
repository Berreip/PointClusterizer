using System;
using System.Collections.Generic;
using System.Linq;
using ClusteringModels;
using ClusteringModels.Results;
using Dbscan;

namespace ClusterizerLib.DbScanAlgorithm;

/// <summary>
/// Run a Db Scan cluster algorithm
/// </summary>
public static class ClusterizerDbScan
{
    /// <summary>
    /// Run the cluster on given points
    /// </summary>
    public static ClusterGlobalResult<T> Run<T>(IEnumerable<T> points, double epsilon, int minimumPointsPerCluster, Func<IPoint, T> conversionFunc) where T : IPoint
    {
        var clustersResult = Dbscan.Dbscan.CalculateClusters(
            points.Select(o => new PointDataDbScan(o.X, o.Y, o.Z)),
            epsilon: epsilon,
            minimumPointsPerCluster: minimumPointsPerCluster);

        var cluster = clustersResult.Clusters.Select(o => new ClusterResult<T>(o.Objects.Select(pt => conversionFunc(pt)).ToArray())).ToArray();
        return new ClusterGlobalResult<T>(cluster, clustersResult.UnclusteredObjects.Select(pt => conversionFunc(pt)).ToArray());
    }

    private sealed record PointDataDbScan(double X, double Y, double Z) : IPointData, IPoint
    {
        public double X { get; } = X;
        public double Y { get; } = Y;
        public double Z { get; } = Z;
        public Point Point { get; } = new Point(X, Y);
    }
}