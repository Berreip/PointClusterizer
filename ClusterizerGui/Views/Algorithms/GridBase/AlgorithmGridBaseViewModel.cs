using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Media.Imaging;
using ClusterizerGui.Views.MainDisplay;
using ClusterizerLib;
using ClusterizerLib.Results;

namespace ClusterizerGui.Views.Algorithms.GridBase;

internal sealed class AlgorithmGridBaseViewModel : AlgorithmViewModelBase<GridBasedHistory>
{
    private int _runGridBased;
    private int _selectedNbRows;
    private int _selectedNbColumn;
    
    public IReadOnlyList<int> AvailableNbColumns { get; } = Enumerable.Range(1, 100).ToArray();
    public IReadOnlyList<int> AvailableNbRows { get; } = Enumerable.Range(1, 100).ToArray();
    
    public AlgorithmGridBaseViewModel(IAlgorithmExecutor algorithmExecutor, IDisplayImageAndClusterController displayImageAndClusterController)
        : base(algorithmExecutor, displayImageAndClusterController)
    {
        _selectedNbRows = AvailableNbRows[0];
        _selectedNbColumn = AvailableNbColumns[0];
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

    protected override GridBasedHistory ExecuteAlgorithmImplementation(BitmapImage? img, IPoint[] points)
    { 
        var watch = Stopwatch.StartNew();
        // execute TODO
        var results = new ClusterGlobalResult<IPoint>(Array.Empty<ClusterResult<IPoint>>(), Array.Empty<IPoint>());
            
        watch.Stop();
        return new GridBasedHistory(
            _displayImageAndClusterController,
            runNumber: Interlocked.Increment(ref _runGridBased), 
            duration: watch.Elapsed, 
            nbInitialPoints: points.Length, 
            clusterResults: results,
            sourceImage: img);
    }
}