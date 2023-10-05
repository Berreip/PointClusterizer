using System.Collections.Generic;
using System.Linq;
using ClusterizerLib.Results;

namespace ClusterizerLib.DbScanAlgorithm;

/// <summary>
/// Run a Db Scan cluster algorithm
/// </summary>
public static class ClusterizerDbScan
{
    /// <summary>
    /// Run the cluster on given points
    /// </summary>
    public static ClusterGlobalResult<T> Run<T>(IEnumerable<T> points, double epsilon, int minimumPointsPerCluster) where T : IPoint
    {
        var clustersResult = Dbscan.Dbscan.CalculateClusters(
            points,
            epsilon: epsilon,
            minimumPointsPerCluster: minimumPointsPerCluster);

        var cluster = clustersResult.Clusters.Select(o => new ClusterResult<T>(o.Objects)).ToArray();
        return new ClusterGlobalResult<T>(cluster, clustersResult.UnclusteredObjects);
    }
}