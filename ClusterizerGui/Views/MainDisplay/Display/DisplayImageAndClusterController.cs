using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ClusteringModels.Results;
using ClusterizerGui.Utils.BitmapGeneration;
using ClusterizerGui.Utils.MathMisc;
using ClusterizerGui.Views.ImportDatasets.Extraction;
using ClusterizerGui.Views.MainDisplay.Adapters;
using PRF.WPFCore;
using PRF.WPFCore.CustomCollections;
using Color = System.Drawing.Color;

namespace ClusterizerGui.Views.MainDisplay.Display;

internal sealed class DisplayImageAndClusterController : ViewModelBase, IDisplayImageAndClusterController
{
    private class ClusterBag
    {
        public ClusterAdapter[] Adapter { get; }
        public PointImageAdapter ImageUnclusterized { get; }

        public ClusterBag(ClusterAdapter[] adapter, BitmapImage imageUnclusterized)
        {
            Adapter = adapter;
            ImageUnclusterized = new PointImageAdapter(imageUnclusterized, false);
        }
    }

    public ICollectionView AllCanvasItems { get; }
    private PointImageAdapter? _currentMainImage;
    private readonly ConcurrentDictionary<ClusterGlobalResult<PointWrapper>, ClusterBag> _clusterAdapterByResult = new ConcurrentDictionary<ClusterGlobalResult<PointWrapper>, ClusterBag>();
    private bool _showPointsOnMap = true;
    private readonly ObservableCollectionRanged<ICanvasItemAdapter> _allCanvasItems;

    public DisplayImageAndClusterController()
    {
        AllCanvasItems = ObservableCollectionSource.GetDefaultView(new []{new EarthImageAdapter()}, out _allCanvasItems);
        AllCanvasItems.SortDescriptions.Add(new SortDescription(nameof(ICanvasItemAdapter.DisplayIndex), ListSortDirection.Ascending));
    }


    public BitmapImage? GetCurrentImage()
    {
        return _currentMainImage?.BitmapImage;
    }

    public bool ShowPointsOnMap
    {
        get => _showPointsOnMap;
        set
        {
            if (SetProperty(ref _showPointsOnMap, value) && _currentMainImage != null)
            {
                ShowOrHideSourceImage(value, _currentMainImage);
            }
        }
    }

    public void ShowOrHideClusters(bool value, ClusterGlobalResult<PointWrapper> clusterResults, SolidColorBrush clusterColor, Color unclusteredPointsColor, IconCategory category, AvailableRadiusCalculationModeAdapter mode)
    {
        if (value)
        {
            // generate adapter:
            var adapters = clusterResults.ClusterResults.Select(o => new ClusterAdapter(clusterColor, o.Points.Count, MathHelper.ComputeClusterInfo(o.Points, category), category, mode)).ToArray();

            // create both images:
            var imageUnclusterized = clusterResults.UnClusteredPoints.GenerateBitmapImageFromPoint();

            var clusterBag = new ClusterBag(adapters, imageUnclusterized);
            if (_clusterAdapterByResult.TryAdd(clusterResults, clusterBag))
            {
                _allCanvasItems.Add(clusterBag.ImageUnclusterized);
                // _allCanvasItems.Add(clusterBag.ImageClusters);
                _allCanvasItems.AddRange(adapters);
                // plus generate image for each unclusterized points and cluster them self for now...
            }
        }
        else
        {
            if (_clusterAdapterByResult.TryRemove(clusterResults, out var clusterBag))
            {
                foreach (var clusterAdapter in clusterBag.Adapter)
                {
                    _allCanvasItems.Remove(clusterAdapter);
                }

                _allCanvasItems.Remove(clusterBag.ImageUnclusterized);
            }
        }
    }

    public void ShowOrHideSourceImage(bool value, IPointImageAdapter sourceImage)
    {
        if (value)
        {
            _allCanvasItems.Add(sourceImage);
        }
        else
        {
            _allCanvasItems.Remove(sourceImage);
        }
    }

    public void ClearCurrentImage()
    {
        var img = _currentMainImage;
        _currentMainImage = null;
        if (img != null)
        {
            _allCanvasItems.Remove(img);
        }
    }

    public void SetNewCurrentImage(BitmapImage newImage)
    {
        var previous = _currentMainImage;
        if (previous != null)
        {
            _allCanvasItems.Remove(previous);
        }

        var adapter = new PointImageAdapter(newImage, true);
        _currentMainImage = adapter;
        if (_showPointsOnMap)
        {
            _allCanvasItems.Add(adapter);
        }
        else
        {
            // re-enable point
            ShowPointsOnMap = true;
        }
    }
}