namespace ClusteringModels.Results;

/// <summary>
/// Regroup all cluster and un-clustered object
/// </summary>
public sealed record ClusterGlobalResult<T>(IReadOnlyList<ClusterResult<T>> ClusterResults, IReadOnlyList<T> UnClusteredPoints, IReadOnlyList<T> OutsideAoiPoints)
    where T : IPoint
{
    /// <summary>
    /// All clusters and their containing points
    /// </summary>
    public IReadOnlyList<ClusterResult<T>> ClusterResults { get; } = ClusterResults;

    /// <summary>
    /// Points that are within the AOI but not in a cluster
    /// </summary>
    public IReadOnlyList<T> UnClusteredPoints { get; } = UnClusteredPoints;

    /// <summary>
    /// Points that was outside the provided AOI
    /// </summary>
    public IReadOnlyList<T> OutsideAoiPoints { get; } = OutsideAoiPoints;
}