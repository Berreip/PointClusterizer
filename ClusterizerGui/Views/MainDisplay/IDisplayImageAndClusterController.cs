using System.ComponentModel;
using System.Windows.Media.Imaging;
using ClusterizerGui.Views.MainDisplay.Adapters;
using ClusterizerLib;
using ClusterizerLib.Results;

namespace ClusterizerGui.Views.MainDisplay;

/// <summary>
/// Object that allow the control of displayed images and cluster on earth view
/// </summary>
internal interface IDisplayImageAndClusterController
{
    void ShowOrHideClusters(bool value, ClusterGlobalResult<IPoint> clusterResults);
    void ShowOrHideSourceImage(bool value, PointImageAdapter sourceImage);
    void ClearCurrentImage();
    void SetNewCurrentImage(BitmapImage newImage);
    ICollectionView AllPointsImages { get; }
    ICollectionView AllClusters { get; }
    bool ShowPointsOnMap { get; set; }
    BitmapImage? GetCurrentImage();
}