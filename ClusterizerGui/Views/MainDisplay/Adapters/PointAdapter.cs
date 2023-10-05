using ClusterizerLib;
using Dbscan;
using PRF.WPFCore;

namespace ClusterizerGui.Views.MainDisplay.Adapters;

internal sealed class PointAdapter : ViewModelBaseUnchecked, IPoint
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }

    public PointAdapter(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
        Point = new Point(x, y);
    }

    public Point Point { get; }
}