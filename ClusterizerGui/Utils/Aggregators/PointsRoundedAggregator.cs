using System;
using System.Collections.Generic;
using ClusterizerLib;

namespace ClusterizerGui.Utils.Aggregators;

internal static class PointsRoundedAggregator
{
    /// <summary>
    /// From a list of points, Round X, Y, Z to the nearest int and cap them. Then, filtered all points of the same value
    /// </summary>
    public static IReadOnlyCollection<PointRounded> AggregateSimilarPoints(IEnumerable<IPoint> points,
        int maxX,
        int maxY,
        int maxZ)
    {
        var pointGrouped = new HashSet<PointRounded>(1000);
        foreach (var point in points)
        {
            pointGrouped.Add(new PointRounded(
                Math.Clamp((int)Math.Round(point.X), 0, maxX),
                Math.Clamp((int)Math.Round(point.Y), 0, maxY),
                Math.Clamp((int)Math.Round(point.Z), 0, maxZ)
            ));
        }

        return pointGrouped;
    }

    /// <summary>
    /// Create a 2 dimension array that contains the given aggregated points
    /// </summary>
    public static PointAggregatedArray CreatePointArrayFromAggregatedData(IReadOnlyCollection<PointRounded> pointsAggregated, int arrayX, int arrayY)
    {
        var noValue = -1;
        // init 2 dimensional array with noValue 
        var array = new int[arrayX,arrayY];
        for (int i = 0; i < arrayX; i++)
        {
            for (int j = 0; j < arrayY; j++)
            {
                array[i, j] = noValue;
            }
        }
        foreach (var point in pointsAggregated)
        {
            array[point.X, point.Y] = point.Z;
        }

        return new PointAggregatedArray(array, noValue);
    }
}

internal sealed class PointAggregatedArray
{
    private readonly int[,] _array;
    private readonly int _noValue;

    public PointAggregatedArray(int[,] array, int noValue)
    {
        _array = array;
        _noValue = noValue;
    }

    public bool TryGetPointAltitude(int x, int y, out int altitude)
    {
        altitude = _array[x, y];
        return altitude != _noValue;
    }
}

/// <summary>
/// represent a point rounded to closest int and with values between 0 and the provided maximum. It implement GetHascode and equals based on int values to allow hashset filtering
/// </summary>
internal sealed class PointRounded : IEquatable<PointRounded>
{
    public int X { get; }
    public int Y { get; }
    public int Z { get; }

    public PointRounded(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public bool Equals(PointRounded? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return X == other.X && Y == other.Y && Z == other.Z;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is PointRounded other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }

    public static bool operator ==(PointRounded? left, PointRounded? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(PointRounded? left, PointRounded? right)
    {
        return !Equals(left, right);
    }
}