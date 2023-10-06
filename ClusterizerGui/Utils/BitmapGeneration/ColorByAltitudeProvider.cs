using System;
using System.Drawing;

namespace ClusterizerGui.Utils.BitmapGeneration;

internal static class ColorByAltitudeProvider
{
    private const int ATTENUATION_FACTOR = 4;
    private static readonly Color _pointColor = Color.GreenYellow;
    private static readonly int _attenuator = ClusterizerGuiConstants.DATA_MAX_Z * ATTENUATION_FACTOR;

    public static Color GetColor(int altitude)
    {
        return _pointColor;
        if (altitude == 0)
        {
            return _pointColor;
        }

        // else, alter color to illustrate altitude
        var percentageModifier = altitude / _attenuator;
        return Color.FromArgb(_pointColor.A,
            Math.Min(255, _pointColor.R * percentageModifier), 
            Math.Min(255, _pointColor.G * percentageModifier), 
            Math.Min(255, _pointColor.B * percentageModifier));
    }
}