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
        return new PointAdapter(_random.NextDouble() * xMax, _random.NextDouble() * yMax, _random.NextDouble() * zMax);
    }
}