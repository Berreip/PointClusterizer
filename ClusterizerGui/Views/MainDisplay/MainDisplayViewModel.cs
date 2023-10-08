using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ClusterizerGui.Utils;
using ClusterizerGui.Utils.BitmapGeneration;
using ClusterizerGui.Views.Algorithms;
using ClusterizerGui.Views.Algorithms.Adapters;
using ClusterizerGui.Views.Algorithms.DbScan;
using ClusterizerGui.Views.Algorithms.GridBase;
using ClusterizerGui.Views.Algorithms.HierarchicalGreedyClustering;
using ClusterizerGui.Views.MainDisplay.Adapters;
using ClusterizerGui.Views.MainDisplay.Display;
using ClusterizerLib;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using PRF.WPFCore.CustomCollections;

namespace ClusterizerGui.Views.MainDisplay;

internal interface IMainDisplayViewModel
{
    /// <summary>
    /// Generate a bunch of points on click position
    /// </summary>
    void GeneratePointsOnClick(Point mousePosition);
}

internal sealed class MainDisplayViewModel : ViewModelBase, IMainDisplayViewModel
{
    private AlgorithmAvailableAdapter? _selectedAlgorithm;
    private IAlgorithmView? _selectedAlgorithmView;
    private int _selectedNbPoints;
    private bool _isIdle = true;
    private int _selectedClickDispersion;
    private bool _addPointOnClick = true;
    private readonly PointRange _defaultRange;

    public IObservableCollectionRanged<PointAdapter> Points { get; }
    public IDelegateCommandLight AddPointsCommand { get; }
    public IDelegateCommandLight ClearPointsCommand { get; }
    public ICollectionView AlgorithmsAvailable { get; }
    public IDisplayImageAndClusterController DisplayController { get; }

    public IReadOnlyList<int> AvailableClickDispersion { get; } = new[] { 5, 10, 20, 40, 50, 100 };
    public IReadOnlyList<int> AvailableNbPoints { get; } = new[] { 1, 5, 10, 100, 1_000, 10_000, 50_000, 100_000, 500_000, 1_000_000 };

    public MainDisplayViewModel()
    {
        AddPointsCommand = new DelegateCommandLight(ExecuteAddPointsCommand);
        ClearPointsCommand = new DelegateCommandLight(ExecuteClearPointsCommand);
        Points = new ObservableCollectionRanged<PointAdapter>();

        _defaultRange = new PointRange(xMin: 0, xMax: ClusterizerGuiConstants.DATA_MAX_X, yMin: 0, yMax: ClusterizerGuiConstants.DATA_MAX_Y, zMin: 0, zMax: ClusterizerGuiConstants.DATA_MAX_Z);

        // create an executor that will be provided to every algorithm
        var executor = new AlgorithmExecutor(() => Points.ToArray<IPoint>(), o => IsIdle = o);
        DisplayController = new DisplayImageAndClusterController();
        
        // hold vm to hold results
        var vmDbScan = new AlgorithmDbScanViewModel(executor, DisplayController);
        var vmGridBase = new AlgorithmGridBaseViewModel(executor, DisplayController);
        var vmHGC = new HierarchicalGreedyClusteringViewModel(executor, DisplayController);
        
        AlgorithmsAvailable = ObservableCollectionSource.GetDefaultView(new[]
        {
            new AlgorithmAvailableAdapter("DBSCAN - density-based spatial clustering", () => new AlgorithmDbScanView(vmDbScan)),
            new AlgorithmAvailableAdapter("Grid-Based Subspace Clustering (CLIQUE/STING)", () => new AlgorithmGridBaseView(vmGridBase)),
            new AlgorithmAvailableAdapter("Hierarchical Greedy Clustering", () => new HierarchicalGreedyClusteringView(vmHGC)),
        }, out var algorithmsAvailable);

        SelectedAlgorithm = algorithmsAvailable.FirstOrDefault();
        _selectedNbPoints = AvailableNbPoints[3];
        _selectedClickDispersion = AvailableClickDispersion[1];
    }


    public AlgorithmAvailableAdapter? SelectedAlgorithm
    {
        get => _selectedAlgorithm;
        set
        {
            if (SetProperty(ref _selectedAlgorithm, value))
            {
                SelectedAlgorithmView = value?.CreateView();
            }
        }
    }

    public IAlgorithmView? SelectedAlgorithmView
    {
        get => _selectedAlgorithmView;
        private set => SetProperty(ref _selectedAlgorithmView, value);
    }


    public int SelectedNbPoints
    {
        get => _selectedNbPoints;
        set => SetProperty(ref _selectedNbPoints, value);
    }

    private void ExecuteAddPointsCommand()
    {
        AddPointInternalFireAndForget(_defaultRange);
    }

    private async void AddPointInternalFireAndForget(PointRange range)
    {
        IsIdle = false;
        var pointsNumber = _selectedNbPoints;
        await AsyncWrapper.DispatchAndWrapAsync(() =>
        {
            var points = new List<PointAdapter>(pointsNumber);
            for (var i = 0; i < pointsNumber; i++)
            {
                points.Add(RandomPointCreator.CreateNew(range));
            }

            Points.AddRange(points);

            // Regenerate Bitmap:
            DisplayController.SetNewCurrentImage(Points.GenerateBitmapImageFromPoint(Color.Purple));
        }, () => IsIdle = true).ConfigureAwait(false);
    }

    public bool IsIdle
    {
        get => _isIdle;
        set => SetProperty(ref _isIdle, value);
    }

    private void ExecuteClearPointsCommand()
    {
        Points.Clear();
        DisplayController.ClearCurrentImage();
    }

    public void GeneratePointsOnClick(Point mousePosition)
    {
        if (_addPointOnClick)
        {
            var dispersion = _selectedClickDispersion;
            var rangeFromMousePositionAndDispersion = new PointRange(
                xMin: Math.Max(0, mousePosition.X - dispersion),
                xMax: Math.Min(ClusterizerGuiConstants.DATA_MAX_X, mousePosition.X + dispersion),
                yMin: Math.Max(0, mousePosition.Y - dispersion),
                yMax: Math.Min(ClusterizerGuiConstants.DATA_MAX_Y, mousePosition.Y + dispersion),
                zMin: 0,
                zMax: ClusterizerGuiConstants.DATA_MAX_Z);
            AddPointInternalFireAndForget(rangeFromMousePositionAndDispersion);
        }
    }

    public int SelectedClickDispersion
    {
        get => _selectedClickDispersion;
        set => SetProperty(ref _selectedClickDispersion, value);
    }

    public bool AddPointOnClick
    {
        get => _addPointOnClick;
        set => SetProperty(ref _addPointOnClick, value);
    }


    private sealed class AlgorithmExecutor : IAlgorithmExecutor
    {
        private readonly Func<IPoint[]> _pointsProviderCallback;
        private readonly Action<bool> _isIdleCallback;

        public AlgorithmExecutor(Func<IPoint[]> pointsProviderCallback, Action<bool> isIdleCallback)
        {
            _pointsProviderCallback = pointsProviderCallback;
            _isIdleCallback = isIdleCallback;
        }

        public async Task ExecuteAsync(Action<IPoint[]> action)
        {
            _isIdleCallback(false);
            await AsyncWrapper.DispatchAndWrapAsync(() =>
                {
                    var points = _pointsProviderCallback.Invoke();
                    action(points);
                }, () => _isIdleCallback(true))
                .ConfigureAwait(false);
        }
    }
    

}