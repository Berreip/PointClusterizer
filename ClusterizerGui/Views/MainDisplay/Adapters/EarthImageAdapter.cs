using ClusterizerGui.Views.MainDisplay.Display;
using ClusterizerGui.Views.MainDisplay.Helpers;

namespace ClusterizerGui.Views.MainDisplay.Adapters;

internal sealed class EarthImageAdapter : ICanvasItemAdapter
{
    public int DisplayIndex { get; } = DisplayIndexContant.IMAGE_EARTH;
    public double WidthPercentage { get; } = 0;
    public double HeightPercentage { get; } = 0;
}