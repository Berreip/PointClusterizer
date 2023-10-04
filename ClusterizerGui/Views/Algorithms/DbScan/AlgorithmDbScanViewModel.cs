using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using ClusterizerGui.Views.MainDisplay.Adapters;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using PRF.WPFCore.CustomCollections;

namespace ClusterizerGui.Views.Algorithms.DbScan;

internal sealed class AlgorithmDbScanViewModel
{
    private readonly IAlgorithmExecutor _algorithmExecutor;
    private readonly ObservableCollectionRanged<DbScanHistory> _dbScanHistory;
    private int _runDbScan;
    public ICollectionView DbScanHistory { get; }
    public IDelegateCommandLight RunDbScanCommand { get; }
    public IDelegateCommandLight ClearHistoryCommand { get; }

    public AlgorithmDbScanViewModel(IAlgorithmExecutor algorithmExecutor)
    {
        _algorithmExecutor = algorithmExecutor;
        DbScanHistory = ObservableCollectionSource.GetDefaultView(out _dbScanHistory);
        RunDbScanCommand = new DelegateCommandLight(ExecuteRunDbScanCommand);
        ClearHistoryCommand = new DelegateCommandLight(ExecuteClearHistoryCommand);
    }

    private void ExecuteClearHistoryCommand()
    {
        _dbScanHistory.Clear();
    }

    private async void ExecuteRunDbScanCommand()
    {
        await _algorithmExecutor.ExecuteAsync(points =>
        {
            var watch = Stopwatch.StartNew();
            // execute
            Thread.Sleep(2000);
            watch.Stop();
            _dbScanHistory.Add(new DbScanHistory(Interlocked.Increment(ref _runDbScan), watch.Elapsed, points.Length, -1));
        }).ConfigureAwait(false);
    }
}

internal sealed class DbScanHistory : ViewModelBase
{
    public int RunNumber { get; }
    public TimeSpan Duration { get; }
    public int NbInitialPoints { get; }
    public int NbClusters { get; }

    public DbScanHistory(int runNumber, TimeSpan duration, int nbInitialPoints, int nbClusters)
    {
        RunNumber = runNumber;
        Duration = duration;
        NbInitialPoints = nbInitialPoints;
        NbClusters = nbClusters;
    }
}