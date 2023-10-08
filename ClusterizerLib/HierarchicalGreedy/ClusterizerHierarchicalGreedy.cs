using System;
using ClusterizerLib.Results;

namespace ClusterizerLib.HierarchicalGreedy;

public static class ClusterizerHierarchicalGreedy
{
    public static ClusterGlobalResult<IPoint> Run(IPoint[] points, int searchRadius, int numberOfLevels)
    {
        return new ClusterGlobalResult<IPoint>(Array.Empty<ClusterResult<IPoint>>(), Array.Empty<IPoint>());
    }
}