using System;
using ClusterizerGui.Views.MainDisplay.Adapters;

namespace ClusterizerGui.Utils;

internal static class RandomPointCreator
{
    public static PointWrapper CreateNew(PointRange range)
    {
        return new PointWrapper(
            range.GetRandomX(),
            range.GetRandomY(),
            range.GetRandomZ());
    }
}

internal sealed class PointRange
{
    private static readonly Random _random = new Random();
    private readonly int _xMin;
    private readonly int _xMax;
    private readonly int _yMin;
    private readonly int _yMax;
    private readonly int _zMin;
    private readonly int _zMax;

    public PointRange(int xMin, int xMax, int yMin, int yMax, int zMin, int zMax)
    {
        _xMin = xMin;
        _xMax = xMax;
        _yMin = yMin;
        _yMax = yMax;
        _zMin = zMin;
        _zMax = zMax;
    }

    public double GetRandomX()
    {
        return _xMin + _random.NextDouble() * (_xMax - _xMin);
    }

    public double GetRandomY()
    {
        return _yMin + _random.NextDouble() * (_yMax - _yMin);
    }

    public double GetRandomZ()
    {
        return _zMin + _random.NextDouble() * (_zMax - _zMin);
    }
}