using ClusteringModels;

namespace ClusteringGridBase;

internal readonly struct Cube
{
    private readonly double _originX;
    private readonly double _originY;
    private readonly double _originZ;
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
        _originX = origin.X;
        _originY = origin.Y;
        _originZ = origin.Z;
        _endingX = origin.X + xLenght;
        _endingY = origin.Y + yLenght;
        _endingZ = origin.Z + zLenght;
    }

    public bool Contains(double x, double y, double z)
    {
        // origin including, ending excluding (for cluster shift on camera moved: should be consistent)
        return x >= _originX && x < _endingX &&
               y >= _originY && y < _endingY &&
               z >= _originZ && z < _endingZ;
    }
}