using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Media.Imaging;
using ClusterizerGui.Utils;
using ClusterizerGui.Views.MainDisplay;
using ClusterizerLib;
using ClusterizerLib.GridBased;

namespace ClusterizerGui.Views.Algorithms.GridBase;

internal sealed class AlgorithmGridBaseViewModel : AlgorithmViewModelBase<GridBasedHistory>
{
    private int _runGridBased;
    private int _selectedNbRows;
    private int _selectedNbColumn;
    private int _minimumDensity;
    private int _selectedPassesNumber;

    public IReadOnlyList<int> AvailableNbColumns { get; } = Enumerable.Range(1, 100).ToArray();
    public IReadOnlyList<int> AvailableNbRows { get; } = Enumerable.Range(1, 100).ToArray();
    public IReadOnlyList<int> AvailablePassesNumber { get; } = Enumerable.Range(1, 6).ToArray();

    public AlgorithmGridBaseViewModel(IAlgorithmExecutor algorithmExecutor, IDisplayImageAndClusterController displayImageAndClusterController)
        : base(algorithmExecutor, displayImageAndClusterController)
    {
        _selectedNbRows = AvailableNbRows[9];
        _selectedNbColumn = AvailableNbColumns[19];
        _selectedPassesNumber = AvailablePassesNumber[1];
        _minimumDensity = 20;
    }

    public int MinimumDensity
    {
        get => _minimumDensity;
        set => SetProperty(ref _minimumDensity, value);
    }

    public int SelectedPassesNumber
    {
        get => _selectedPassesNumber;
        set => SetProperty(ref _selectedPassesNumber, value);
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
        var density = _minimumDensity;
        var rows = _selectedNbRows;
        var columns = _selectedNbColumn;
        var numberOfPasses = _selectedPassesNumber;
        
        var watch = Stopwatch.StartNew();
        var results = ClusterizerGridBase.Run(
            points: points,
            aoi: new Rectangle(0, 0, ClusterizerGuiConstants.IMAGE_WIDTH, ClusterizerGuiConstants.IMAGE_HEIGHT),
            nbRowTargeted: rows,
            nbColumnTargeted: columns,
            clusteringDensityThreshold: density,
            numberOfPasses: numberOfPasses);

        watch.Stop();
        return new GridBasedHistory(
            _displayImageAndClusterController,
            runNumber: Interlocked.Increment(ref _runGridBased),
            duration: watch.Elapsed,
            nbInitialPoints: points.Length,
            clusterResults: results,
            sourceImage: img,
            minimumDensity:density,
            selectedPassesNumber:numberOfPasses,
            columns:columns,
            rows:rows
        );
    }
}