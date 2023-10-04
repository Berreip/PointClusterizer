using System.Collections.Generic;

namespace ClusterizerLib;

/// <summary>
/// A cluster result
/// </summary>
public sealed class ClusterResult
{
    public IReadOnlyList<IPoint> Points { get; }
}