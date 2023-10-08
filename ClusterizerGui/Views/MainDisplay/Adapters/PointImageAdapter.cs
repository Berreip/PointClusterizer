using System.Windows.Media.Imaging;
using ClusterizerGui.Views.MainDisplay.Display;
using ClusterizerGui.Views.MainDisplay.Helpers;

namespace ClusterizerGui.Views.MainDisplay.Adapters;

internal interface IPointImageAdapter : ICanvasItemAdapter
{
    BitmapImage BitmapImage { get; }
}

internal class PointImageAdapter : IPointImageAdapter
{
    public BitmapImage BitmapImage { get; }
    public int DisplayIndex { get; }
    public double WidthPercentage { get; } = 0;
    public double HeightPercentage { get; }= 0;

    public PointImageAdapter(BitmapImage bitmapImage, bool isMainImage)
    {
        BitmapImage = bitmapImage;
        DisplayIndex = isMainImage ? DisplayIndexConstant.IMAGE_MAIN_DATA_POINTS : DisplayIndexConstant.IMAGE_POINTS_FROM_PREVIOUS_RUN;
    }
}