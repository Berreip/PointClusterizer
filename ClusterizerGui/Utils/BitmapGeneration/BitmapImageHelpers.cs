using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace ClusterizerGui.Utils.BitmapGeneration;

internal static class BitmapImageHelpers
{
    public static BitmapImage GetBitmapImage(this Bitmap img)
    {
        using (var ms = new MemoryStream())
        {
            var image = new BitmapImage();
            img.Save(ms, ImageFormat.Png);
            ms.Position = 0;
            image.BeginInit();
            image.StreamSource = ms;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            image.Freeze(); //freeze it
            return image;
        }
    }
}