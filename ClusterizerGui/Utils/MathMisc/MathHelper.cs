using System.Collections.Generic;
using ClusterizerGui.Views.MainDisplay.Adapters;
using ClusterizerLib;

namespace ClusterizerGui.Utils.MathMisc;

internal static class MathHelper
{
    /// <summary>
    /// Compute the 2 dimensional centroid of a set of points
    /// </summary>
    public static PointD Compute2dCentroid(IReadOnlyList<IPoint> points)
    {
        double x = 0;
        double y = 0;
        foreach (var point in points)
        {
            x += point.X;
            y += point.Y;
        }

        return new PointD(x / points.Count, y / points.Count);
    }
    
    /// <summary>
    /// Compute the 2 dimensional centroid of a set of points
    /// </summary>
    public static IPoint Compute3dCentroid(IReadOnlyList<IPoint> points)
    {
        double x = 0;
        double y = 0;
        double z = 0;
        foreach (var point in points)
        {
            x += point.X;
            y += point.Y;
            z += point.Z;
        }

        return new PointAdapter(x / points.Count, y / points.Count, z /points.Count);
    }
}

internal readonly record struct PointD(double X, double Y);