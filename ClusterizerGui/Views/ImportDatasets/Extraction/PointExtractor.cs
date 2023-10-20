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
        return ConvertAsMercator(latitude, longitude, category: categoryMapper.GetCategory(csvLineAdapter.Category));
    }

    /// <summary>
    /// Convert DD to Radian
    /// </summary>
    private static double ToRadian(this double degree)
    {
        return degree * Math.PI / 180d;
    }

    // ReSharper disable once UnusedMember.Local
    private static PointWrapper ConvertAsMillerCylindrical(double latitude, double longitude, IconCategory category)
    {
        // get x value
        var x = (longitude + 180) * (MAP_WIDTH / 360d);
        
        // convert from degrees to radians
        var latRad = latitude.ToRadian();
        
        // get y value
        var millerLat = 1.25 * Math.Log(Math.Tan(Math.PI / 4 + (0.4 * latRad)));

        var y = MAP_HEIGHT - millerLat * MAP_HEIGHT;
        
        return new PointWrapper(x: x, y: y, z: 0, category: category);
    }

    private static PointWrapper ConvertAsMercator(double latitude, double longitude, IconCategory category)
    {
        // get x value
        var x = (longitude + 180) * (MAP_WIDTH / 360d);

        // convert from degrees to radians
        var latRad = latitude.ToRadian();

        // get y value
        var mercatorN = Math.Log(Math.Tan(Math.PI / 4 + latRad / 2));
        var y = (MAP_HEIGHT / 2d) - (MAP_WIDTH * mercatorN / (2 * Math.PI));

        return new PointWrapper(x: x, y: y, z: 0, category: category);
    }

    public static PointWrapper ConvertAsSimpleProjection(double latitude, double longitude, IconCategory category)
    {
        var x = (longitude + 180) * (MAP_WIDTH / 360d);
        var y = -(latitude - 90) * (MAP_HEIGHT / 180d);

        return new PointWrapper(x: x, y: y, z: 0, category: category);
    }
}