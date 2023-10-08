using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing;

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

    public static BitmapImage ComputeGridBitmapFromRowsAndColumns(int selectedNbRows, int selectedNbColumn)
    {
        using (var bmp = new Bitmap(ClusterizerGuiConstants.IMAGE_WIDTH, ClusterizerGuiConstants.IMAGE_HEIGHT, PixelFormat.Format32bppArgb))
        {
            unsafe
            {
                var bitmapData = bmp.LockBits(
                    new Rectangle(0, 0, bmp.Width, bmp.Height),
                    ImageLockMode.ReadWrite,
                    bmp.PixelFormat);

                var verticalLineTag = ClusterizerGuiConstants.IMAGE_WIDTH / selectedNbColumn;
                var horizontalLineTag = ClusterizerGuiConstants.IMAGE_HEIGHT / selectedNbRows;

                try
                {
                    var bytesPerPixel = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
                    var heightInPixels = bitmapData.Height;
                    var widthInBytes = bitmapData.Width * bytesPerPixel;

                    var currentLine = (byte*)bitmapData.Scan0;
                    // set every pixel value
                    var heightCursor = 0;
                    for (var y = 0; y < heightInPixels; y++, currentLine += bitmapData.Stride)
                    {
                        var widthCursor = 0;
                        for (var x = 0; x < widthInBytes; x = x + bytesPerPixel)
                        {
                            if (heightCursor % horizontalLineTag == 0 || widthCursor % verticalLineTag == 0)
                            {
                                var color = Color.Blue;
                                currentLine[x] = color.B; //blue
                                currentLine[x + 1] = color.G; //green
                                currentLine[x + 2] = color.R; //red
                                currentLine[x + 3] = color.A; //transparency
                            }
                            else
                            {
                                currentLine[x] = 0; //blue
                                currentLine[x + 1] = 0; //green
                                currentLine[x + 2] = 0; //red
                                currentLine[x + 3] = 0; //transparency
                            }

                            widthCursor++;
                        }

                        heightCursor++;
                    }
                }
                finally
                {
                    bmp.UnlockBits(bitmapData);
                }
            }

            return bmp.GetBitmapImage();
        }
    }
}