using System;
using System.Drawing;
using ClusterizerGui.Utils.Aggregators;

namespace ClusterizerGui.Utils.BitmapGeneration;

internal static class ColorByAltitudeProvider
{
    private const int ATTENUATION_FACTOR = 4;
    private const int ATTENUATOR = ClusterizerGuiConstants.DATA_MAX_Z * ATTENUATION_FACTOR;

    public static Color GetColor(AltitudeAndColor altitudeAndColor)
    {
        if (altitudeAndColor.Altitude == 0)
        {
            return altitudeAndColor.Color;
        }

        // else, alter color to illustrate altitude
        var percentageModifier = 1 - (altitudeAndColor.Altitude /(double)ATTENUATOR );
        return Color.FromArgb(altitudeAndColor.Color.A,
            (int)Math.Min(255, altitudeAndColor.Color.R * percentageModifier),
            (int)Math.Min(255, altitudeAndColor.Color.G * percentageModifier),
            (int)Math.Min(255, altitudeAndColor.Color.B * percentageModifier));
    }
}