using System;
using System.Collections.Generic;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace ClusterizerGui.Views.ImportDatasets.Extraction;

internal enum IconCategory
{
    None,
    Blue,
    Yellow,
    Red,
    Green,
}

internal static class IconCategoryExtensions
{
    public static string ConvertToDisplayName(this IconCategory category)
    {
        switch (category)
        {
            case IconCategory.None:
                return "None";
            case IconCategory.Blue:
                return "Friendly";
            case IconCategory.Yellow:
                return "Unknown";
            case IconCategory.Red:
                return "Hostile";
            case IconCategory.Green:
                return "Neutral";
            default:
                throw new ArgumentOutOfRangeException(nameof(category), category, null);
        }
    }

    private static readonly Dictionary<IconCategory, SolidColorBrush> _colorBrushMapping = new Dictionary<IconCategory, SolidColorBrush>
    {
        { IconCategory.None, Brushes.Purple },
        { IconCategory.Blue, Brushes.CornflowerBlue },
        { IconCategory.Yellow, Brushes.Yellow },
        { IconCategory.Red, Brushes.Red },
        { IconCategory.Green, Brushes.Green },
    };
    
    private static readonly Dictionary<IconCategory, Color> _colorMapping = new Dictionary<IconCategory, Color>
    {
        { IconCategory.None, Color.Purple },
        { IconCategory.Blue, Color.CornflowerBlue },
        { IconCategory.Yellow, Color.Yellow },
        { IconCategory.Red, Color.Red },
        { IconCategory.Green, Color.Green },
    };

    public static SolidColorBrush ToCategoryColorBrush(this IconCategory category)
    {
        return _colorBrushMapping[category];
    }

    public static Color ToCategoryColor(this IconCategory category)
    {
        return _colorMapping[category];
    }
}