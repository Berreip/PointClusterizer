using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ClusteringModels.Results;
using ClusterizerGui.Views.ImportDatasets.Extraction;
using ClusterizerGui.Views.MainDisplay.Adapters;
using Color = System.Drawing.Color;

namespace ClusterizerGui.Views.MainDisplay;

/// <summary>
/// Object that allow the control of displayed images and cluster on earth view
/// </summary>
internal interface IDisplayImageAndClusterController
{
    void ShowOrHideClusters(bool value, ClusterGlobalResult<PointWrapper> clusterResults, SolidColorBrush clusterColor, Color unclusteredPointsColor, IconCategory category, AvailableRadiusCalculationModeAdapter mode);
    void ShowOrHideSourceImage(bool value, IPointImageAdapter sourceImage);
    void ClearCurrentImage();
    void SetNewCurrentImage(BitmapImage newImage);
    bool ShowPointsOnMap { get; set; }
    public ICollectionView AllCanvasItems { get; }
    BitmapImage? GetCurrentImage();
}