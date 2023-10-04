using PRF.WPFCore;

namespace ClusterizerGui.Views.MainDisplay.Adapters;

internal sealed class PointAdapter : ViewModelBaseUnchecked
{
    public int X { get; }
    public int Y { get; }
    public int Z { get; }

    public PointAdapter(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}