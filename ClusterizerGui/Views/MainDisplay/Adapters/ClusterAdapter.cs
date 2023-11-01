using System;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ClusteringModels;
using ClusterizerGui.Utils;
using ClusterizerGui.Views.ImportDatasets.Extraction;
using ClusterizerGui.Views.MainDisplay.Display;
using ClusterizerGui.Views.MainDisplay.Helpers;
using PRF.WPFCore;

namespace ClusterizerGui.Views.MainDisplay.Adapters;

internal sealed class ClusterAdapter : ViewModelBase, ICanvasItemAdapter
{
    public int Radius { get; }
    public IPoint Centroid { get; }
    public int DisplayIndex { get; }
    public double WidthPercentage { get; }
    public double HeightPercentage { get; }
    public string TooltipText { get; }
    public SolidColorBrush ClusterKindColor { get; }
    public SolidColorBrush CategoryColor { get; }
    public IconCategory Category { get; }
    public double IconCategoryHeight { get; }
    
    /// <summary>
    /// Shift for centering icons
    /// </summary>
    public double IconCategoryHeightShift { get; }
    public bool HasCategory { get; }
    public BitmapImage? CategoryIcon { get; }

    private const double MIN_IMAGE_SIZE = 10d;

    public ClusterAdapter(
        SolidColorBrush clusterKindColor,
        int pointsCount,
        (IPoint centroid, Rectangle aoi) clusterInfo,
        IconCategory category,
        AvailableRadiusCalculationModeAdapter mode)
    {
        ClusterKindColor = clusterKindColor;
        Category = category;
        CategoryColor = category.ToCategoryColorBrush();
        HasCategory = category != IconCategory.None;
        CategoryIcon = category.ToCategoryIcon();
        // Test with different radius
        Radius = mode.ComputeRadius(pointsCount, clusterInfo.aoi);
        // Radius = RadiusCalculation.ComputeRadiusFromPointCountsSurfaceBased(pointsCount, clusterInfo.aoi);
        Centroid = clusterInfo.centroid;
        WidthPercentage = clusterInfo.centroid.X / ClusterizerGuiConstants.IMAGE_WIDTH;
        HeightPercentage = clusterInfo.centroid.Y / ClusterizerGuiConstants.IMAGE_HEIGHT;
        DisplayIndex = DisplayIndexConstant.IMAGE_CLUSTERS;
        TooltipText = $"{pointsCount} items";
        
        // have fun with icon size: make it relative to radius
        IconCategoryHeight = Math.Max(MIN_IMAGE_SIZE, Radius * 0.8d);
        IconCategoryHeightShift = -IconCategoryHeight / 2;
    }
}