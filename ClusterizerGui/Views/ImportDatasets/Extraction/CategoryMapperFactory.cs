using System.Collections.Generic;
using ClusterizerGui.Utils;

namespace ClusterizerGui.Views.ImportDatasets.Extraction;

internal static class CategoryMapperFactory
{
    public static CategoryMapper Create(string? blueCategoryMapping, string? yellowCategoryMapping, string? redCategoryMapping, string? greenCategoryMapping)
    {
        var mapper = new Dictionary<string, IconCategory>();
        var allOtherCategoryDefaultToIcon = IconCategory.None;
        
        Map(blueCategoryMapping, ref allOtherCategoryDefaultToIcon, mapper, IconCategory.Blue);
        Map(yellowCategoryMapping, ref allOtherCategoryDefaultToIcon, mapper, IconCategory.Yellow);
        Map(redCategoryMapping, ref allOtherCategoryDefaultToIcon, mapper, IconCategory.Red);
        Map(greenCategoryMapping, ref allOtherCategoryDefaultToIcon, mapper, IconCategory.Green);
        
        return new CategoryMapper(mapper, allOtherCategoryDefaultToIcon);
    }

    private static void Map(string? category, ref IconCategory allOtherCategoryDefaultToIcon, Dictionary<string, IconCategory> mapper, IconCategory icon)
    {
        if (category != null)
        {
            if (category == ClusterizerGuiConstants.ALL_REMAINING_ALIAS_CATEGORY)
            {
                allOtherCategoryDefaultToIcon = icon;
            }
            else
            {
                mapper.Add(category, icon);
            }
        }
    }
}

/// <summary>
/// A mapping between string category and global category
/// </summary>
internal sealed class CategoryMapper
{
    private readonly Dictionary<string, IconCategory> _mapper;
    private readonly IconCategory _allOtherCategoryDefaultToIcon;

    public CategoryMapper(Dictionary<string,IconCategory> mapper, IconCategory allOtherCategoryDefaultToIcon)
    {
        _mapper = mapper;
        _allOtherCategoryDefaultToIcon = allOtherCategoryDefaultToIcon;
    }

    public IconCategory GetCategory(string category)
    {
        return _mapper.TryGetValue(category, out var categoryIcon) ? categoryIcon : _allOtherCategoryDefaultToIcon;
    }
}