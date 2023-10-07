using System;
using System.Drawing;

namespace ClusterizerGui.Utils.BitmapGeneration;

internal static class ColorByAltitudeProvider
{
    private const int ATTENUATION_FACTOR = 4;
    private static readonly int _attenuator = ClusterizerGuiConstants.DATA_MAX_Z * ATTENUATION_FACTOR;

    public static Color GetColor(int altitude, Color pointColor)
    {
        if (altitude == 0)
        {
            return pointColor;
        }

        // else, alter color to illustrate altitude
        var percentageModifier = 1 - (altitude /(double)_attenuator );
        return Color.FromArgb(pointColor.A,
            (int)Math.Min(255, pointColor.R * percentageModifier),
            (int)Math.Min(255, pointColor.G * percentageModifier),
            (int)Math.Min(255, pointColor.B * percentageModifier));
    }
}