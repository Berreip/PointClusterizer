using System.Drawing;
using System.Windows.Media;
using ClusterizerGui.Utils;
using ClusterizerGui.Views.ImportDatasets.Extraction;
using ClusterizerGui.Views.MainDisplay.Display;
using ClusterizerGui.Views.MainDisplay.Helpers;
using ClusterizerLib;
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
    public SolidColorBrush ClusterColor { get; }
    public IconCategory Category { get; }

    public ClusterAdapter(SolidColorBrush clusterColor, int pointsCount, (IPoint centroid, Rectangle aoi) clusterInfo, IconCategory category)
    {
        ClusterColor = clusterColor;
        Category = category;
        // Test with different radius
        Radius = RadiusCalculation.ComputeRadiusFromPointCountsLogBased(pointsCount, clusterInfo.aoi);
        // Radius = RadiusCalculation.ComputeRadiusFromPointCountsSurfaceBased(pointsCount, clusterInfo.aoi);
        Centroid = clusterInfo.centroid;
        WidthPercentage = clusterInfo.centroid.X / ClusterizerGuiConstants.IMAGE_WIDTH;
        HeightPercentage = clusterInfo.centroid.Y / ClusterizerGuiConstants.IMAGE_HEIGHT;
        DisplayIndex = DisplayIndexConstant.IMAGE_CLUSTERS;
        TooltipText = $"{pointsCount} items";
    }
}