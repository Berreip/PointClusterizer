using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ClusterizerGui.Services;
using ClusterizerGui.Utils;
using ClusterizerGui.Utils.BitmapGeneration;
using ClusterizerGui.Views.Algorithms;
using ClusterizerGui.Views.Algorithms.Adapters;
using ClusterizerGui.Views.Algorithms.DbScan;
using ClusterizerGui.Views.Algorithms.GridBase;
using ClusterizerGui.Views.Algorithms.HierarchicalGreedyClustering;
using ClusterizerGui.Views.ImportDatasets.Extraction;
using ClusterizerGui.Views.MainDisplay.Adapters;
using ClusterizerGui.Views.MainDisplay.Display;
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
    private readonly IDatasetManager _datasetManager;
    private AlgorithmAvailableAdapter? _selectedAlgorithm;
    private IAlgorithmView? _selectedAlgorithmView;
    private int _selectedNbPoints;
    private bool _isIdle = true;
    private int _selectedClickDispersion;
    private bool _addPointOnClick = true;
    private readonly PointRange _defaultRange;

    private readonly List<PointWrapper> _points = new List<PointWrapper>();
    private int _pointsCount;
    private readonly ObservableCollectionRanged<DatasetAvailableAdapters> _availableDatasets;
    private CategorySelectionAdapter _selectedCategory;
    public IDelegateCommandLight AddPointsCommand { get; }
    public IDelegateCommandLight ClearPointsCommand { get; }
    public IDelegateCommandLight<DatasetAvailableAdapters> AddDataSetContentCommand { get; }
    public ICollectionView AlgorithmsAvailable { get; }
    public IDisplayImageAndClusterController DisplayController { get; }

    public IReadOnlyList<int> AvailableClickDispersion { get; } = new[] { 5, 10, 20, 40, 50, 100 };
    public IReadOnlyList<int> AvailableNbPoints { get; } = new[] { 1, 5, 10, 100, 1_000, 10_000, 50_000, 100_000, 500_000, 1_000_000 };

    public CategorySelectionAdapter[] AvailableCategories { get; } = Enum.GetValues<IconCategory>().Select(o => new CategorySelectionAdapter(o)).ToArray();

    public ICollectionView AvailableDatasets { get; }

    public MainDisplayViewModel(IDatasetManager datasetManager)
    {
        _datasetManager = datasetManager;
        AddPointsCommand = new DelegateCommandLight(ExecuteAddPointsCommand);
        ClearPointsCommand = new DelegateCommandLight(ExecuteClearPointsCommand);
        AddDataSetContentCommand = new DelegateCommandLight<DatasetAvailableAdapters>(ExecuteAddDataSetContentCommand);
        _defaultRange = new PointRange(xMin: 0, xMax: ClusterizerGuiConstants.DATA_MAX_X, yMin: 0, yMax: ClusterizerGuiConstants.DATA_MAX_Y, zMin: 0, zMax: ClusterizerGuiConstants.DATA_MAX_Z);

        // create an executor that will be provided to every algorithm
        var executor = new AlgorithmExecutor(() =>
        {
            var array = new PointWrapper[_points.Count];
            _points.CopyTo(array);
            return array;
        }, o => IsIdle = o);
        DisplayController = new DisplayImageAndClusterController();

        // hold vm to hold results
        var vmDbScan = new AlgorithmDbScanViewModel(executor, DisplayController);
        var vmGridBase = new AlgorithmGridBaseViewModel(executor, DisplayController);
        var vmHGC = new HierarchicalGreedyClusteringViewModel(executor, DisplayController);
        _selectedCategory = AvailableCategories.First();
        
        AvailableDatasets = ObservableCollectionSource.GetDefaultView(out _availableDatasets);
        datasetManager.OnDatasetUpdated += ReloadDataSet;
        ReloadDataSet();
        
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

    private void ReloadDataSet()
    {
        _availableDatasets.Reset(_datasetManager.GetAllDatasets().Select(o => new DatasetAvailableAdapters(o)));
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

    private async void ExecuteAddDataSetContentCommand(DatasetAvailableAdapters dataset)
    {
        await AsyncWrapper.DispatchAndWrapAsync(() =>
        {
            AddPointAndRegenerateBitmapAccordingly(dataset.GetDatasetContent());
        }).ConfigureAwait(false);

    }

    private async void AddPointInternalFireAndForget(PointRange range)
    {
        IsIdle = false;
        var pointsNumber = _selectedNbPoints;
        await AsyncWrapper.DispatchAndWrapAsync(() =>
        {
            var points = new List<PointWrapper>(pointsNumber);
            for (var i = 0; i < pointsNumber; i++)
            {
                points.Add(RandomPointCreator.CreateNew(range, _selectedCategory.Category));
            }

            AddPointAndRegenerateBitmapAccordingly(points);
        }, () => IsIdle = true).ConfigureAwait(false);
    }

    private void AddPointAndRegenerateBitmapAccordingly(IEnumerable<PointWrapper> points)
    {
        _points.AddRange(points);
        PointsCount = _points.Count;

        // Regenerate Bitmap:
        DisplayController.SetNewCurrentImage(_points.GenerateBitmapImageFromPoint());
    }

    public bool IsIdle
    {
        get => _isIdle;
        set => SetProperty(ref _isIdle, value);
    }

    private void ExecuteClearPointsCommand()
    {
        _points.Clear();
        PointsCount = 0;
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

    public int PointsCount
    {
        get => _pointsCount;
        private set => SetProperty(ref _pointsCount, value);
    }

    public CategorySelectionAdapter SelectedCategory
    {
        get => _selectedCategory;
        set => SetProperty(ref _selectedCategory, value);
    }

    private sealed class AlgorithmExecutor : IAlgorithmExecutor
    {
        private readonly Func<PointWrapper[]> _pointsProviderCallback;
        private readonly Action<bool> _isIdleCallback;

        public AlgorithmExecutor(Func<PointWrapper[]> pointsProviderCallback, Action<bool> isIdleCallback)
        {
            _pointsProviderCallback = pointsProviderCallback;
            _isIdleCallback = isIdleCallback;
        }

        public async Task ExecuteAsync(Action<PointWrapper[]> action)
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