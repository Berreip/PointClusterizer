using ClusteringModels;
using Dbscan;

namespace ClusterizerLib.UnitTests;

public class PointUnitTest: IPoint
{
    public PointUnitTest(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
        Point = new Point(x, y);
    }

    public Point Point { get; }
    public double X { get; }
    public double Y { get; }
    public double Z { get; }
    
    public override string ToString()
    {
        return $"X={X:0.##} Y={Y:0.##} Z={Z:0.##}";
    }
}