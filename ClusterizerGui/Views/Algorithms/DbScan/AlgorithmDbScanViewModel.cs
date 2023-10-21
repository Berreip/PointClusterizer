using System.Diagnostics;
using System.Threading;
using System.Windows.Media.Imaging;
using ClusterizerGui.Views.ImportDatasets.Extraction;
using ClusterizerGui.Views.MainDisplay;
using ClusterizerGui.Views.MainDisplay.Adapters;
using ClusterizerLib.DbScanAlgorithm;

namespace ClusterizerGui.Views.Algorithms.DbScan;

internal sealed class AlgorithmDbScanViewModel : AlgorithmViewModelBase<DbScanHistory>
{
    private int _runDbScan;
    private int _epsilonDbScan;
    private int _minimumPointsPerCluster;

    public AlgorithmDbScanViewModel(IAlgorithmExecutor algorithmExecutor, IDisplayImageAndClusterController displayImageAndClusterController)
        : base(algorithmExecutor, displayImageAndClusterController)
    {
        _epsilonDbScan = 5;
        _minimumPointsPerCluster = 10;
    }

    public int EpsilonDbScan
    {
        get => _epsilonDbScan;
        set => SetProperty(ref _epsilonDbScan, value);
    }

    public int MinimumPointsPerCluster
    {
        get => _minimumPointsPerCluster;
        set => SetProperty(ref _minimumPointsPerCluster, value);
    }

    protected override DbScanHistory ExecuteAlgorithmImplementation(BitmapImage? img, PointWrapper[] points, IconCategory category)
    {
        var epsilonDbScan = EpsilonDbScan;
        var minimumPointsPerCluster = MinimumPointsPerCluster;

        var watch = Stopwatch.StartNew();
        // execute
        var results = ClusterizerDbScan.Run(points: points, epsilon: epsilonDbScan, minimumPointsPerCluster: minimumPointsPerCluster);
        watch.Stop();
        return new DbScanHistory(
            displayImageAndClusterController: _displayImageAndClusterController,
            runNumber: Interlocked.Increment(ref _runDbScan),
            duration: watch.Elapsed,
            nbInitialPoints: points.Length,
            clusterResults: results,
            epsilon: epsilonDbScan,
            minPointByCluster: minimumPointsPerCluster,
            sourceImage: img,
            category:category);
    }
}