using System;
using ClusterizerGui.Utils;
using ClusterizerGui.Views.ImportDatasets.Adapter;
using ClusterizerGui.Views.MainDisplay.Adapters;

namespace ClusterizerGui.Views.ImportDatasets.Extraction;

internal static class PointExtractor
{
    private const int MAP_WIDTH = ClusterizerGuiConstants.IMAGE_WIDTH;
    private const int MAP_HEIGHT = ClusterizerGuiConstants.IMAGE_HEIGHT;
    
    public static PointWrapper ConvertToPoint(CsvLineAdapter csvLineAdapter)
    {
        if (!csvLineAdapter.IsValid)
        {
            throw new InvalidOperationException("unable to convert invalid input data");
        }

        var latitude = csvLineAdapter.LatitudeParsed; // (φ)
        var longitude = csvLineAdapter.LongitudeParsed; // (λ)

        // get x value
        var x = (longitude + 180) * (MAP_WIDTH / 360d);

        // convert from degrees to radians
        var latRad = latitude * Math.PI / 180;

        // get y value
        var mercatorN = Math.Log(Math.Tan(Math.PI / 4 + latRad / 2));
        var y = (MAP_HEIGHT / 2d) - (MAP_WIDTH * mercatorN / (2 * Math.PI));

        return new PointWrapper(x: x, y: y, z: 0);
    }
}