using System;
using System.Drawing;
using ClusterizerGui.Utils;

namespace ClusterizerGui.Views.MainDisplay.Helpers;

public static class RadiusCalculation
{
    private const int MAX_RADIUS = ClusterizerGuiConstants.IMAGE_HEIGHT / 4;

    /// <summary>
    /// Compute a radius depending on point count
    /// </summary>
    public static int Radius4Log2(int pointCounts, Rectangle aoi)
    {
        var radius = 4 * (int)Math.Log2(pointCounts + 1);
        return Math.Min(MAX_RADIUS, radius);
    }

    /// <summary>
    /// Compute a radius depending on point count
    /// </summary>
    public static int Radius4Log10(int pointCounts, Rectangle aoi)
    {
        var radius = 4 * (int)Math.Log10(pointCounts + 1);
        return Math.Min(MAX_RADIUS, radius);
    }


    /// <summary>
    /// Compute a radius depending on point count
    /// </summary>
    public static int Radius10Log10(int pointCounts, Rectangle aoi)
    {
        var radius = 10 * (int)Math.Log10(pointCounts + 1);
        return Math.Min(MAX_RADIUS, radius);
    }

    /// <summary>
    /// Compute a radius depending on point count
    /// </summary>
    public static int RadiusLog(int pointCounts, Rectangle aoi)
    {
        var radius = (int)Math.Log(pointCounts + 1);
        return Math.Min(MAX_RADIUS, radius);
    }

    /// <summary>
    /// Compute a radius depending on point count
    /// </summary>
    public static int RadiusLinear10(int pointCounts, Rectangle aoi)
    {
        return Math.Min(MAX_RADIUS, pointCounts/10);
    }

    /// <summary>
    /// Compute a radius depending on aoi
    /// </summary>
    public static int RadiusSurfaceBased(int pointCounts, Rectangle aoi)
    {
        // add %
        return (int)(Math.Max(aoi.Width, aoi.Height) * 1.3);
    }
}