using ClusteringModels;

namespace ClusteringGridBase.UnitTests;

public class PointUnitTest: IPoint
{
    public PointUnitTest(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public double X { get; }
    public double Y { get; }
    public double Z { get; }
    
    public override string ToString()
    {
        return $"X={X:0.##} Y={Y:0.##} Z={Z:0.##}";
    }
}