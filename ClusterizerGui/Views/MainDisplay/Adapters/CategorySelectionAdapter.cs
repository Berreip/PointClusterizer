using System.Windows.Media;
using ClusterizerGui.Views.ImportDatasets.Extraction;
using PRF.WPFCore;
using Color = System.Drawing.Color;

namespace ClusterizerGui.Views.MainDisplay.Adapters;

internal sealed class CategorySelectionAdapter : ViewModelBase
{
    public IconCategory Category { get; }
    public string DisplayName { get; }
    public SolidColorBrush CategoryColorBrush { get; }
    public Color CategoryColor { get; }

    public CategorySelectionAdapter(IconCategory category)
    {
        Category = category;
        DisplayName = category.ConvertToDisplayName();
        CategoryColorBrush = category.ToCategoryColorBrush();
        CategoryColor =  category.ToCategoryColor();
        
    }

}