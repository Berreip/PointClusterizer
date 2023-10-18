using ClusterizerLib;
using Dbscan;

namespace ClusterizerGui.Views.MainDisplay.Adapters;

internal sealed class PointWrapper : IPoint
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }

    public PointWrapper(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
        Point = new Point(x, y);
    }

    public Point Point { get; }

    public override string ToString()
    {
        return $"X={X:0.##} Y={Y:0.##} Z={Z:0.##}";
    }
}