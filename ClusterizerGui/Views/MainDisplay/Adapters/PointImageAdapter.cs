using System.Windows.Media.Imaging;
using ClusterizerGui.Views.MainDisplay.Display;
using ClusterizerGui.Views.MainDisplay.Helpers;

namespace ClusterizerGui.Views.MainDisplay.Adapters;

public class PointImageAdapter : ICanvasItemAdapter
{
    /// <summary>
    /// If the current bitmap is the reference image on which points are added
    /// </summary>
    public bool IsMainImage { get;  }
    public BitmapImage BitmapImage { get; }
    public int DisplayIndex { get; }
    public double WidthPercentage { get; } = 0;
    public double HeightPercentage { get; }= 0;

    public PointImageAdapter(BitmapImage bitmapImage, bool isMainImage)
    {
        IsMainImage = isMainImage;
        BitmapImage = bitmapImage;
        DisplayIndex = isMainImage ? DisplayIndexContant.IMAGE_MAIN_DATA_POINTS : DisplayIndexContant.IMAGE_POINTS_FROM_PREVIOUS_RUN;
    }
}