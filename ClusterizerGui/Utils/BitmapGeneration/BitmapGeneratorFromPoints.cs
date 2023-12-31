﻿using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using ClusterizerGui.Utils.Aggregators;
using ClusterizerGui.Views.MainDisplay.Adapters;

namespace ClusterizerGui.Utils.BitmapGeneration;

internal static class BitmapGeneratorFromPoints
{
    public static BitmapImage GenerateBitmapImageFromPoint(this IEnumerable<PointWrapper> points)
    {
        using (var bmp = GenerateBitmapFromPoint(points))
        {
            return bmp.GetBitmapImage();
        }
    }
    
    public static Bitmap GenerateBitmapFromPoint(IEnumerable<PointWrapper> points)
    {
        // aggregate similar point rounded to int
        var pointsAggregated = PointsRoundedAggregator.AggregateSimilarPoints(
            points,
            ClusterizerGuiConstants.DATA_MAX_X, 
            ClusterizerGuiConstants.DATA_MAX_Y,
            ClusterizerGuiConstants.DATA_MAX_Z);

        var arrayPoints = PointsRoundedAggregator.CreatePointArrayFromAggregatedData(
            pointsAggregated,
            ClusterizerGuiConstants.IMAGE_WIDTH,
            ClusterizerGuiConstants.IMAGE_HEIGHT);

        var bmp = new Bitmap(ClusterizerGuiConstants.IMAGE_WIDTH, ClusterizerGuiConstants.IMAGE_HEIGHT, PixelFormat.Format32bppArgb);
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
                var heightCursor = 0;
                for (var y = 0; y < heightInPixels; y++, currentLine += bitmapData.Stride)
                {
                    var widthCursor = 0;
                    for (var x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {
                        var altitudeAndColor = arrayPoints.GetPointAltitude(widthCursor, heightCursor);
                        if (altitudeAndColor.Altitude != PointsRoundedAggregator.ALTITUDE_NO_VALUE)
                        {
                            var color = ColorByAltitudeProvider.GetColor(altitudeAndColor);
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

        return bmp;
    }
}