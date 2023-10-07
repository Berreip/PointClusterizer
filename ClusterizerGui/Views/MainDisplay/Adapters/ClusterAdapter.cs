using ClusterizerGui.Utils;
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

    public ClusterAdapter(int pointsCount, IPoint centroid)
    {
        Radius = RadiusCalculation.ComputeRadiusFromPointCounts(pointsCount);
        Centroid = centroid;
        WidthPercentage = centroid.X / ClusterizerGuiConstants.IMAGE_WIDTH;
        HeightPercentage = centroid.Y / ClusterizerGuiConstants.IMAGE_HEIGHT;
        DisplayIndex = DisplayIndexContant.IMAGE_CLUSTERS;
        TooltipText = $"{pointsCount} items";
    }
}