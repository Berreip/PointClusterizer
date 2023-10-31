using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Media.Imaging;
using ClusteringGridBase;
using ClusterizerGui.Utils;
using ClusterizerGui.Utils.BitmapGeneration;
using ClusterizerGui.Views.Algorithms.Adapters;
using ClusterizerGui.Views.ImportDatasets.Extraction;
using ClusterizerGui.Views.MainDisplay;
using ClusterizerGui.Views.MainDisplay.Adapters;

namespace ClusterizerGui.Views.Algorithms.GridBase;

internal sealed class AlgorithmGridBaseViewModel : AlgorithmViewModelBase<GridBasedHistory>
{
    private int _runGridBased;
    private int _selectedNbRows;
    private int _selectedNbColumn;
    private int _minimumDensity;
    private int _selectedPassesNumber;
    private bool _displayGridOnEarth;
    private readonly GridImageAdapter _gridImageAdapter;

    public IReadOnlyList<int> AvailableNbColumns { get; } = Enumerable.Range(1, 100).ToArray();
    public IReadOnlyList<int> AvailableNbRows { get; } = Enumerable.Range(1, 100).ToArray();
    public IReadOnlyList<int> AvailablePassesNumber { get; } = Enumerable.Range(1, Clusterizer.NUMBER_PASSES_LIMIT).ToArray();

    public AlgorithmGridBaseViewModel(IAlgorithmExecutor algorithmExecutor, IDisplayImageAndClusterController displayImageAndClusterController, IRadiusModeProvider radiusModeProvider)
        : base(algorithmExecutor, displayImageAndClusterController, radiusModeProvider)
    {
        _selectedNbRows = 15;
        _selectedNbColumn = 30;
        _selectedPassesNumber = 2;
        _minimumDensity = 20;
        _gridImageAdapter = new GridImageAdapter(BitmapImageHelpers.ComputeGridBitmapFromRowsAndColumns(_selectedNbRows, _selectedNbColumn));
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
        set
        {
            if (SetProperty(ref _selectedNbRows, value))
            {
                _gridImageAdapter.UpdateImage(BitmapImageHelpers.ComputeGridBitmapFromRowsAndColumns(value, _selectedNbColumn));
            }
        }
    }

    public int SelectedNbColumn
    {
        get => _selectedNbColumn;
        set
        {
            if (SetProperty(ref _selectedNbColumn, value))
            {
                _gridImageAdapter.UpdateImage(BitmapImageHelpers.ComputeGridBitmapFromRowsAndColumns(_selectedNbRows, value));
            }
        }
    }

    public bool DisplayGridOnEarth
    {
        get => _displayGridOnEarth;
        set
        {
            if (SetProperty(ref _displayGridOnEarth, value))
            {
                _displayImageAndClusterController.ShowOrHideSourceImage(value, _gridImageAdapter);
            }
        }
    }

    protected override GridBasedHistory ExecuteAlgorithmImplementation(BitmapImage? img, PointWrapper[] points, IconCategory category, AvailableRadiusCalculationModeAdapter radiusMode)
    {
        var density = _minimumDensity;
        var rows = _selectedNbRows;
        var columns = _selectedNbColumn;
        var numberOfPasses = _selectedPassesNumber;
        
        var watch = Stopwatch.StartNew();
        var results = Clusterizer.Run(
            points: points,
            origin:new PointWrapper(0, 0, 0, default),
            xLenght : ClusterizerGuiConstants.IMAGE_WIDTH,
            yLenght : ClusterizerGuiConstants.IMAGE_HEIGHT,
            zLenght : ClusterizerGuiConstants.IMAGE_ALTITUDE,
            nbPartX : columns,
            nbPartY : rows,
            nbPartZ : 1, // one cell for altitude
            clusteringDensityThreshold: density,
            neighbouringMergingDistance: numberOfPasses);

        watch.Stop();
        return new GridBasedHistory(
            _displayImageAndClusterController,
            runNumber: Interlocked.Increment(ref _runGridBased),
            duration: watch.Elapsed,
            nbInitialPoints: points.Length,
            clusterResults: results,
            sourceImage: img,
            minimumDensity: density,
            selectedPassesNumber: numberOfPasses,
            columns: columns,
            rows: rows,
            category: category,
            radiusMode: radiusMode);
    }
}