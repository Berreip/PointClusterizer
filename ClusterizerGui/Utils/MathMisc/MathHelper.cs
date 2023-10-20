using System;
using System.Collections.Generic;
using System.Drawing;
using ClusterizerGui.Views.ImportDatasets.Extraction;
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
    /// Compute the centroid and aoi of a set of points
    /// </summary>
    public static (IPoint centroid, Rectangle aoi) ComputeClusterInfo(IReadOnlyList<IPoint> points, IconCategory category)
    {
        double x = 0;
        double y = 0;
        double z = 0;

        var minX = ClusterizerGuiConstants.DATA_MAX_X;
        var minY = ClusterizerGuiConstants.DATA_MAX_Y;
        var maxX = 0;
        var maxY = 0;

        foreach (var point in points)
        {
            x += point.X;
            y += point.Y;
            z += point.Z;
            minX = Math.Min(minX, (int)point.X);
            minY = Math.Min(minY, (int)point.Y);
            maxX = Math.Max(maxX, (int)point.X);
            maxY = Math.Max(maxY, (int)point.Y);
        }

        return (new PointWrapper(x / points.Count, y / points.Count, z / points.Count, category), new Rectangle(minX, minY, maxX - minX, maxY - minY));
    }
}

internal readonly record struct PointD(double X, double Y);