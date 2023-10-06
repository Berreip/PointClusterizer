using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using ClusterizerGui.Utils.Aggregators;
using ClusterizerLib;

namespace ClusterizerGui.Utils.BitmapGeneration;

internal static class BitmapGeneratorFromPoints
{
    public static Bitmap GenerateBitmapFromPoint(IEnumerable<IPoint> points)
    {
        // aggregate similar point rounded to int
        var pointsAggregated = PointsRoundedAggregator.AggregateSimilarPoints(
            points,
            ClusterizerGuiConstants.IMAGE_MAX_X,
            ClusterizerGuiConstants.IMAGE_MAX_Y,
            ClusterizerGuiConstants.IMAGE_MAX_Z);

        var arrayPoints = PointsRoundedAggregator.CreatePointArrayFromAggregatedData(
            pointsAggregated,
            ClusterizerGuiConstants.IMAGE_MAX_X,
            ClusterizerGuiConstants.IMAGE_MAX_Y);

        var bmp = new Bitmap(ClusterizerGuiConstants.IMAGE_MAX_X, ClusterizerGuiConstants.IMAGE_MAX_Y, PixelFormat.Format32bppArgb);
        unsafe
        {
            var bitmapData = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadWrite,
                bmp.PixelFormat);
            try
            {
                var bytesPerPixel = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
                var heightInPixels = bitmapData.Height;
                var widthInBytes = bitmapData.Width * bytesPerPixel;

                var currentLine = (byte*)bitmapData.Scan0;
                // set every pixel value
                var arrayHeightRow = 0;
                for (var y = 0; y < heightInPixels; y++, currentLine += bitmapData.Stride)
                {
                    var arrayWidthColum = 0;
                    for (var x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {
                        if (arrayPoints.TryGetPointAltitude(x, y, out var altitude))
                        {
                            var color = ColorByAltitudeProvider.GetColor(altitude);
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

                        arrayWidthColum++;
                    }

                    arrayHeightRow++;
                }
            }
            finally
            {
                bmp.UnlockBits(bitmapData);
            }
        }

        return bmp;
    }
}