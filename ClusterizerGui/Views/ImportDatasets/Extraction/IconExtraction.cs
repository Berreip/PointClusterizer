using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace ClusterizerGui.Views.ImportDatasets.Extraction;

internal static class IconExtraction
{
    private static Dictionary<IconCategory, BitmapImage?>? _categoryBitmapImageMapping;

    public static BitmapImage? ToCategoryIcon(this IconCategory category)
    {
        if (_categoryBitmapImageMapping == null)
        {
            throw new InvalidOperationException("the method IconExtraction LoadIconUri should be called in the UIThread before accessing ToCategoryIcon method");
        }

        return _categoryBitmapImageMapping[category];
    }

    public static void LoadIconUri()
    {
        if (_categoryBitmapImageMapping != null)
        {
            return;
        }
        _categoryBitmapImageMapping = new Dictionary<IconCategory, BitmapImage?>
        {
            // nothing for none
            { IconCategory.None, null },
            { IconCategory.Blue, new BitmapImage(new Uri("pack://application:,,,/Ressources/blue.png")) },
            { IconCategory.Yellow, new BitmapImage(new Uri("pack://application:,,,/Ressources/yellow.png")) },
            { IconCategory.Red, new BitmapImage(new Uri("pack://application:,,,/Ressources/red.png")) },
            { IconCategory.Green, new BitmapImage(new Uri("pack://application:,,,/Ressources/green.png")) },
        };
    }
}