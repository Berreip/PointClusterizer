using System.Collections.Generic;
using ClusteringModels;
using ClusteringModels.Results;

namespace ClusterizerLib.Results;

/// <summary>
/// Regroup all cluster and un-clustered object
/// </summary>
public sealed class ClusterGlobalResult<T> where T : IPoint
{
    public IReadOnlyList<ClusterResult<T>> ClusterResults { get; }
    public IReadOnlyList<T> UnClusteredPoint { get; }

    public ClusterGlobalResult(IReadOnlyList<ClusterResult<T>> clusterResults, IReadOnlyList<T> unClusteredPoint)
    {
        ClusterResults = clusterResults;
        UnClusteredPoint = unClusteredPoint;
    }
}