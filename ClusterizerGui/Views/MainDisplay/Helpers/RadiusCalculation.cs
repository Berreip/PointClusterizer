using System;

namespace ClusterizerGui.Views.MainDisplay.Helpers;

public static class RadiusCalculation
{
    private const int MAX_RADIUS = 50;
    
    /// <summary>
    /// Compute a radius depending on point count
    /// </summary>
    public static int ComputeRadiusFromPointCounts(int pointCounts)
    {
        // TODO => Do Better
        var radius = (int)Math.Log(pointCounts);
        return Math.Min(MAX_RADIUS, radius);
    }

}