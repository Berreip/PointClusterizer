using System.Windows.Media.Imaging;

namespace ClusterizerGui.Views.MainDisplay.Adapters;

public class PointImageAdapter
{
    /// <summary>
    /// If the current bitmap is the reference image on which points are added
    /// </summary>
    public bool IsMainImage { get;  }
    public BitmapImage BitmapImage { get; }

    public PointImageAdapter(BitmapImage bitmapImage, bool isMainImage)
    {
        IsMainImage = isMainImage;
        BitmapImage = bitmapImage;
    }
}