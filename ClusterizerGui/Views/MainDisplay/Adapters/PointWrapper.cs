using System.Drawing;
using ClusterizerGui.Views.ImportDatasets.Extraction;
using ClusterizerLib;
using Point = Dbscan.Point;

namespace ClusterizerGui.Views.MainDisplay.Adapters;

internal sealed class PointWrapper : IPoint
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }
    public IconCategory Category { get; }
    public Color Color { get; }
    

    public PointWrapper(double x, double y, double z, IconCategory category)
    {
        X = x;
        Y = y;
        Z = z;
        Category = category;
        Color = category.ToCategoryColor();
        Point = new Point(x, y);
    }

    public Point Point { get; }

    public override string ToString()
    {
        return $"X={X:0.##} Y={Y:0.##} Z={Z:0.##}";
    }
}