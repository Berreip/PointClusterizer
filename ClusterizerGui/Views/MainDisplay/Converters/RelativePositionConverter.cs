using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace ClusterizerGui.Views.MainDisplay.Converters;

public class RelativePositionConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length != 2)
        {
            return 0;
        }
        try
        {
            if (TryGetDouble(values[0], out var canvasActualWidthOrHeight) && TryGetDouble(values[1], out var percentage))
            {
                return canvasActualWidthOrHeight * percentage;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }

        return 0;
    }

    private static bool TryGetDouble(object parameter, out double parsed)
    {
        if (parameter is string str && double.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out parsed))
        {
            return true;
        }

        if (parameter is double percentage)
        {
            parsed = percentage;
            return true;
        }

        parsed = 0;
        return false;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}