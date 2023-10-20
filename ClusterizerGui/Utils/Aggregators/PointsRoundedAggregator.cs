using System;
using System.Collections.Generic;
using System.Drawing;
using ClusterizerGui.Views.ImportDatasets.Extraction;
using ClusterizerGui.Views.MainDisplay.Adapters;

namespace ClusterizerGui.Utils.Aggregators;

internal static class PointsRoundedAggregator
{
    public const int ALTITUDE_NO_VALUE = -1;
    public static readonly AltitudeAndColor NoValue = new AltitudeAndColor(ALTITUDE_NO_VALUE, IconCategory.None.ToCategoryColor());

    /// <summary>
    /// From a list of points, Round X, Y, Z to the nearest int and cap them. Then, filtered all points of the same value
    /// </summary>
    public static IReadOnlyCollection<PointRounded> AggregateSimilarPoints(IEnumerable<PointWrapper> points,
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
                Math.Clamp((int)Math.Round(point.Z), 0, maxZ),
                point.Color
            ));
        }

        return pointGrouped;
    }
    
    /// <summary>
    /// Create a 2 dimension array that contains the given aggregated points
    /// </summary>
    public static PointAggregatedArray CreatePointArrayFromAggregatedData(IReadOnlyCollection<PointRounded> pointsAggregated, int arrayX, int arrayY)
    {
        // init 2 dimensional array with noValue 
        var array = new AltitudeAndColor[arrayX,arrayY];
        for (var i = 0; i < arrayX; i++)
        {
            for (var j = 0; j < arrayY; j++)
            {
                array[i, j] = NoValue;
            }
        }
        foreach (var point in pointsAggregated)
        {
            array[point.X, point.Y] = new AltitudeAndColor(point.Z, point.Color);
        }

        return new PointAggregatedArray(array);
    }
}

internal record struct AltitudeAndColor
{
    public int Altitude { get; }
    public Color Color { get; }

    public AltitudeAndColor(int altitude, Color color)
    {
        Altitude = altitude;
        Color = color;
    }
}

internal sealed class PointAggregatedArray
{
    private readonly AltitudeAndColor[,] _array;

    public PointAggregatedArray(AltitudeAndColor[,] array)
    {
        _array = array;
    }

    public AltitudeAndColor GetPointAltitude(int x, int y)
    {
        return _array[x, y];
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
    public Color Color { get; }

    public PointRounded(int x, int y, int z, Color color)
    {
        X = x;
        Y = y;
        Z = z;
        Color = color; // ignored for equality
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