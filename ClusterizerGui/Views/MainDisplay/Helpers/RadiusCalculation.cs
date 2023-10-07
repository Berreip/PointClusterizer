using System;
using ClusterizerGui.Utils;

namespace ClusterizerGui.Views.MainDisplay.Helpers;

public static class RadiusCalculation
{
    private const int MAX_RADIUS = ClusterizerGuiConstants.IMAGE_HEIGHT/4;
    private const int AMPLIFIER = 4;
    private const int SUB_AMPLIFIER = 4;
    
    /// <summary>
    /// Compute a radius depending on point count
    /// </summary>
    public static int ComputeRadiusFromPointCounts(int pointCounts)
    {
        var radius = (int)Math.Log2(pointCounts) * AMPLIFIER;
        return Math.Min(MAX_RADIUS, radius);
    }

}