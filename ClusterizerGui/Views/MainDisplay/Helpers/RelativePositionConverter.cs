using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace ClusterizerGui.Views.MainDisplay.Helpers;

public class RelativePositionConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            if(value is double canvasActualWidthOrHeight && parameter is double percentage)
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

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}