using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ClusterizerLib;
using ClusterizerLib.Results;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using PRF.WPFCore.CustomCollections;

namespace ClusterizerGui.Views.Algorithms.GridBase;

internal sealed class AlgorithmGridBaseViewModel : ViewModelBase
{
    private readonly IAlgorithmExecutor _algorithmExecutor;
    private readonly ObservableCollectionRanged<GridBasedHistory> _gridBasedHistory;
    private int _runGridBased;

    private int _selectedNbRows;
    private int _selectedNbColumn;
    public ICollectionView GridBasedHistory { get; }
    public IDelegateCommandLight RunGridBasedCommand { get; }
    public IDelegateCommandLight ClearHistoryCommand { get; }
    
    public IReadOnlyList<int> AvailableNbColumns { get; } = Enumerable.Range(1, 100).ToArray();
    public IReadOnlyList<int> AvailableNbRows { get; } = Enumerable.Range(1, 100).ToArray();
    
    public AlgorithmGridBaseViewModel(IAlgorithmExecutor algorithmExecutor)
    {
        _algorithmExecutor = algorithmExecutor;
        GridBasedHistory = ObservableCollectionSource.GetDefaultView(out _gridBasedHistory);
        RunGridBasedCommand = new DelegateCommandLight(ExecuteRunGridBasedCommand);
        ClearHistoryCommand = new DelegateCommandLight(ExecuteClearHistoryCommand);
        _selectedNbRows = AvailableNbRows[0];
        _selectedNbColumn = AvailableNbColumns[0];
    }
    
    private void ExecuteClearHistoryCommand()
    {
        _gridBasedHistory.Clear();
    }
    
    public int SelectedNbRows
    {
        get => _selectedNbRows;
        set => SetProperty(ref _selectedNbRows, value);
    }

    public int SelectedNbColumn
    {
        get => _selectedNbColumn;
        set => SetProperty(ref _selectedNbColumn, value);
    }

    
    private async void ExecuteRunGridBasedCommand()
    {
        await _algorithmExecutor.ExecuteAsync(points =>
        {
            var watch = Stopwatch.StartNew();
            // execute TODO
            var results = new ClusterGlobalResult<IPoint>(Array.Empty<ClusterResult<IPoint>>(), Array.Empty<IPoint>());
            
            watch.Stop();
            _gridBasedHistory.Add(new GridBasedHistory(
                runNumber: Interlocked.Increment(ref _runGridBased), 
                duration: watch.Elapsed, 
                nbInitialPoints: points.Length, 
                clusterResults: results));
        }).ConfigureAwait(false);
    }
}