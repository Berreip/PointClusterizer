using System;
using System.Windows.Media.Imaging;
using ClusterizerGui.Views.MainDisplay;
using ClusterizerGui.Views.MainDisplay.Adapters;
using ClusterizerLib;
using ClusterizerLib.Results;
using PRF.WPFCore;

namespace ClusterizerGui.Views.Algorithms;

internal abstract class HistoryBase : ViewModelBase, IDisposable
{
    private readonly IDisplayImageAndClusterController _displayImageAndClusterController;
    private readonly ClusterGlobalResult<IPoint> _clusterResults;
    private bool _showClusters;
    private bool _showPointsImageRef;
    private readonly PointImageAdapter? _sourceImageAdapter;
    public int RunNumber { get; }
    public TimeSpan Duration { get; }
    public int NbInitialPoints { get; }
    public int NbClusters { get; }
    public int UnClusteredPoint { get; }
    public string DurationInSecond { get; }

    protected HistoryBase(
        IDisplayImageAndClusterController displayImageAndClusterController, 
        int runNumber,
        TimeSpan duration,
        int nbInitialPoints,
        ClusterGlobalResult<IPoint> clusterResults,
        BitmapImage? sourceImage)
    {
        _displayImageAndClusterController = displayImageAndClusterController;
        _clusterResults = clusterResults;
        RunNumber = runNumber;
        Duration = duration;
        DurationInSecond = $"{duration.TotalSeconds:0.##}";
        NbInitialPoints = nbInitialPoints;
        NbClusters = clusterResults.ClusterResults.Count;
        UnClusteredPoint = clusterResults.UnClusteredPoint.Count;
        if (sourceImage != null)
        {
            _sourceImageAdapter = new PointImageAdapter(sourceImage, false);
        }
        ShowClusters = true;
    }

    public bool ShowClusters
    {
        get => _showClusters;
        set
        {
            if (SetProperty(ref _showClusters, value))
            {
                _displayImageAndClusterController.ShowOrHideClusters(value, _clusterResults);
            }
        }
    }

    public bool ShowPointsImageRef
    {
        get => _showPointsImageRef;
        set
        {
            if (SetProperty(ref _showPointsImageRef, value) && _sourceImageAdapter != null)
            {
                _displayImageAndClusterController.ShowOrHideSourceImage(value, _sourceImageAdapter);
            }
        }
    }

    public void Dispose()
    {
        // remove all line if needed
        _displayImageAndClusterController.ShowOrHideClusters(false, _clusterResults);
        if (_sourceImageAdapter != null)
        {
            _displayImageAndClusterController.ShowOrHideSourceImage(false, _sourceImageAdapter);
        }
        _displayImageAndClusterController.ShowOrHideClusters(false, _clusterResults);
    }
}