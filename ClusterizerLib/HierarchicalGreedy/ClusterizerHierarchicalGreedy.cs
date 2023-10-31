using System;
using ClusteringModels;
using ClusteringModels.Results;

namespace ClusterizerLib.HierarchicalGreedy;

public static class ClusterizerHierarchicalGreedy
{
    public static ClusterGlobalResult<T> Run<T>(T[] points, int searchRadius, int numberOfLevels) where T :IPoint
    {
        return new ClusterGlobalResult<T>(Array.Empty<ClusterResult<T>>(), Array.Empty<T>());
    }
}