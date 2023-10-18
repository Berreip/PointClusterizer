using System;
using System.Drawing;
using ClusterizerGui.Utils;

namespace ClusterizerGui.Views.MainDisplay.Helpers;

public static class RadiusCalculation
{
    private const int MAX_RADIUS = ClusterizerGuiConstants.IMAGE_HEIGHT / 4;
    private const int AMPLIFIER = 4;

    /// <summary>
    /// Compute a radius depending on point count
    /// </summary>
    public static int ComputeRadiusFromPointCountsLogBased(int pointCounts, Rectangle aoi)
    {
        var radius = (int)Math.Log2(pointCounts) * AMPLIFIER;
        return Math.Min(MAX_RADIUS, radius);
    }

    /// <summary>
    /// Compute a radius depending on aoi
    /// </summary>
    public static int ComputeRadiusFromPointCountsSurfaceBased(int pointCounts, Rectangle aoi)
    {
        // add %
        return (int)(Math.Max(aoi.Width, aoi.Height) * 1.3);
    }
}