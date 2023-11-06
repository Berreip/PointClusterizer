using ClusteringModels;

namespace ClusteringGridBase;

internal readonly struct Cube
{
    public double OriginX { get; }
    public double OriginY { get; }
    public double OriginZ { get; }
    private readonly double _endingX;
    private readonly double _endingY;
    private readonly double _endingZ;
    public double XSize { get; }
    public double YSize { get; }
    public double ZSize { get; }

    public Cube(IPoint origin, double xLenght, double yLenght, double zLenght)
    {
        XSize = xLenght;
        YSize = yLenght;
        ZSize = zLenght;
        OriginX = origin.X;
        OriginY = origin.Y;
        OriginZ = origin.Z;
        _endingX = origin.X + xLenght;
        _endingY = origin.Y + yLenght;
        _endingZ = origin.Z + zLenght;
    }

    public bool Contains(double x, double y, double z)
    {
        // origin including, ending excluding (for cluster shift on camera moved: should be consistent)
        return x >= OriginX && x < _endingX &&
               y >= OriginY && y < _endingY &&
               z >= OriginZ && z < _endingZ;
    }
}