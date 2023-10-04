using System;
using ClusterizerGui.Views.MainDisplay.Adapters;

namespace ClusterizerGui.Utils;

internal static class RandomPointCreator
{
    private static readonly Random _random;

    static RandomPointCreator()
    {
        _random = new Random();
    }

    public static PointAdapter CreateNew(int xMax, int yMax, int zMax)
    {
        return new PointAdapter(_random.Next(xMax), _random.Next(yMax), _random.Next(zMax));
    }
}