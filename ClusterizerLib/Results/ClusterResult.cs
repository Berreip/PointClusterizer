using System.Collections.Generic;

namespace ClusterizerLib.Results;

/// <summary>
/// A cluster result
/// </summary>
public sealed class ClusterResult<T> where T : IPoint
{
    public ClusterResult(IReadOnlyList<T> points)
    {
        Points = points;
    }

    /// <summary>
    /// All point within the cluster
    /// </summary>
    public IReadOnlyList<T> Points { get; }
}