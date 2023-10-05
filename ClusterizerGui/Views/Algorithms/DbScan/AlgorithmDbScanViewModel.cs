using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using ClusterizerLib.DbScanAlgorithm;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using PRF.WPFCore.CustomCollections;

namespace ClusterizerGui.Views.Algorithms.DbScan;

internal sealed class AlgorithmDbScanViewModel : ViewModelBase
{
    private readonly IAlgorithmExecutor _algorithmExecutor;
    private readonly ObservableCollectionRanged<DbScanHistory> _dbScanHistory;
    private int _runDbScan;
    private int _epsilonDbScan;
    private int _minimumPointsPerCluster;
    public ICollectionView DbScanHistory { get; }
    public IDelegateCommandLight RunDbScanCommand { get; }
    public IDelegateCommandLight ClearHistoryCommand { get; }

    public AlgorithmDbScanViewModel(IAlgorithmExecutor algorithmExecutor)
    {
        _algorithmExecutor = algorithmExecutor;
        DbScanHistory = ObservableCollectionSource.GetDefaultView(out _dbScanHistory);
        RunDbScanCommand = new DelegateCommandLight(ExecuteRunDbScanCommand);
        ClearHistoryCommand = new DelegateCommandLight(ExecuteClearHistoryCommand);
        _epsilonDbScan = 20;
        _minimumPointsPerCluster = 4;
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

    private void ExecuteClearHistoryCommand()
    {
        _dbScanHistory.Clear();
    }

    private async void ExecuteRunDbScanCommand()
    {
        var epsilonDbScan = EpsilonDbScan;
        var minimumPointsPerCluster = MinimumPointsPerCluster;

        await _algorithmExecutor.ExecuteAsync(points =>
        {
            var watch = Stopwatch.StartNew();
            // execute
            var results = ClusterizerDbScan.Run(points: points, epsilon: epsilonDbScan, minimumPointsPerCluster: minimumPointsPerCluster);
            watch.Stop();
            _dbScanHistory.Add(new DbScanHistory(
                runNumber: Interlocked.Increment(ref _runDbScan), 
                duration: watch.Elapsed, 
                nbInitialPoints: points.Length, 
                clusterResults: results, 
                epsilon: epsilonDbScan, 
                minPointByCluster: minimumPointsPerCluster));
        }).ConfigureAwait(false);
    }
}