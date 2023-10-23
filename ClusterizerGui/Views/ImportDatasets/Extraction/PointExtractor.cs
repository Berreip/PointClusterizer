using System;
using ClusterizerGui.Utils;
using ClusterizerGui.Views.ImportDatasets.Adapter;
using ClusterizerGui.Views.MainDisplay.Adapters;

namespace ClusterizerGui.Views.ImportDatasets.Extraction;

internal static class PointExtractor
{
    private const int MAP_WIDTH = ClusterizerGuiConstants.IMAGE_WIDTH;
    private const int MAP_HEIGHT = ClusterizerGuiConstants.IMAGE_HEIGHT;

    public static PointWrapper ConvertToPoint(CsvLineAdapter csvLineAdapter, CategoryMapper categoryMapper)
    {
        if (!csvLineAdapter.IsValid)
        {
            throw new InvalidOperationException("unable to convert invalid input data");
        }

        var latitude = csvLineAdapter.LatitudeParsed; // (φ)
        var longitude = csvLineAdapter.LongitudeParsed; // (λ)
        return ConvertAsMillerCylindrical(latitude, longitude, category: categoryMapper.GetCategory(csvLineAdapter.Category));
    }

    /// <summary>
    /// Convert DD to Radian
    /// </summary>
    public static double ToRadian(this double degree)
    {
        return degree * Math.PI / 180d;
    }

    // ReSharper disable once UnusedMember.Local
    public static PointWrapper ConvertAsMillerCylindrical(double latitude, double longitude, IconCategory category)
    {
        const double magicCorrectionFactor = 2.3;
        
        // get x value
        var x = (longitude + 180d) * (MAP_WIDTH / 360d);

        // convert from degrees to radians
        var latRad = latitude.ToRadian();

        // get y value
        var millerLat = 1.25d * Math.Log(Math.Tan(Math.PI / 4d + (0.4d * latRad)));
        
        var millerLatCorrected = millerLat / magicCorrectionFactor;

        var y = MAP_HEIGHT * (1d- millerLatCorrected) / 2d ;

        return new PointWrapper(x: x, y: y, z: 0d, category: category);
    }

    public static PointWrapper ConvertAsMercator(double latitude, double longitude, IconCategory category)
    {
        const double magicCorrectionFactor = 3.13;
        
        // get x value
        var x = (longitude + 180d) * (MAP_WIDTH / 360d);

        // convert from degrees to radians
        var latRad = latitude.ToRadian();

        // get y value
        var mercatorN = Math.Log(Math.Tan(Math.PI / 4d + latRad / 2d));

        var mercatorNCorrected = mercatorN / magicCorrectionFactor;
        var y = MAP_HEIGHT * (1d- mercatorNCorrected) / 2d ;

        return new PointWrapper(x: x, y: y, z: 0, category: category);
    }

    public static PointWrapper ConvertAsSimpleProjection(double latitude, double longitude, IconCategory category)
    {
        var x = (longitude + 180d) * (MAP_WIDTH / 360d);
        var y = -(latitude - 90d) * (MAP_HEIGHT / 180d);

        return new PointWrapper(x: x, y: y, z: 0, category: category);
    }
}