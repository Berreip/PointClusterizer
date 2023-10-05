using Dbscan;

namespace ClusterizerLib;

public interface IPoint : IPointData
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }
}