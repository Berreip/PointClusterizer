namespace ClusteringModels.Results;

/// <summary>
/// A cluster result
/// </summary>
public sealed record ClusterResult<T>(IReadOnlyList<T> Points) where T : IPoint
{
    /// <summary>
    /// All point within the cluster
    /// </summary>
    public IReadOnlyList<T> Points { get; } = Points;
}