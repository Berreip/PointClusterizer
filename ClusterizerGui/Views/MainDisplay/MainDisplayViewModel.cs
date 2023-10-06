using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ClusterizerGui.Utils;
using ClusterizerGui.Utils.BitmapGeneration;
using ClusterizerGui.Views.Algorithms;
using ClusterizerGui.Views.Algorithms.Adapters;
using ClusterizerGui.Views.Algorithms.DbScan;
using ClusterizerGui.Views.Algorithms.GridBase;
using ClusterizerGui.Views.MainDisplay.Adapters;
using ClusterizerLib;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using PRF.WPFCore.CustomCollections;

namespace ClusterizerGui.Views.MainDisplay;

internal interface IMainDisplayViewModel
{
}

internal sealed class MainDisplayViewModel : ViewModelBase, IMainDisplayViewModel
{
    private AlgorithmAvailableAdapter? _selectedAlgorithm;
    private IAlgorithmView? _selectedAlgorithmView;
    private int _selectedNbRows;
    private int _selectedNbColumn;
    private int _selectedNbPoints;
    private bool _isIdle = true;
    private BitmapImage? _currentImage;

    public IObservableCollectionRanged<PointAdapter> Points { get; }
    public IDelegateCommandLight AddPointsCommand { get; }
    public IDelegateCommandLight ClearPointsCommand { get; }
    public ICollectionView AlgorithmsAvailable { get; }

    public IReadOnlyList<int> AvailableNbColumns { get; } = Enumerable.Range(1, 100).ToArray();
    public IReadOnlyList<int> AvailableNbRows { get; } = Enumerable.Range(1, 100).ToArray();
    public IReadOnlyList<int> AvailableNbPoints { get; } = new[] { 1, 5, 10, 100, 1_000, 10_000, 50_000, 100_000, 500_000, 1_000_000 };

    public MainDisplayViewModel()
    {
        AddPointsCommand = new DelegateCommandLight(ExecuteAddPointsCommand);
        ClearPointsCommand = new DelegateCommandLight(ExecuteClearPointsCommand);
        Points = new ObservableCollectionRanged<PointAdapter>();

        // create an executor that will be provided to every algorithm
        var executor = new AlgorithmExecutor(() => Points.ToArray<IPoint>(), o => IsIdle = o);

        AlgorithmsAvailable = ObservableCollectionSource.GetDefaultView(new[]
        {
            new AlgorithmAvailableAdapter("DBSCAN - density-based spatial clustering", () =>
            {
                var vm = new AlgorithmDbScanViewModel(executor);
                return new AlgorithmDbScanView(vm);
            }),
            new AlgorithmAvailableAdapter("Grid-Based Subspace Clustering (CLIQUE/STING)", () =>
            {
                var vm = new AlgorithmGridBaseViewModel(executor);
                return new AlgorithmGridBaseView(vm);
            })
        }, out var algorithmsAvailable);


        SelectedAlgorithm = algorithmsAvailable.FirstOrDefault();
        _selectedNbRows = AvailableNbRows[0];
        _selectedNbColumn = AvailableNbColumns[0];
        _selectedNbPoints = AvailableNbPoints[0];
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

    public int SelectedNbPoints
    {
        get => _selectedNbPoints;
        set => SetProperty(ref _selectedNbPoints, value);
    }

    private async void ExecuteAddPointsCommand()
    {
        IsIdle = false;
        var pointsNumber = _selectedNbPoints;
        await AsyncWrapper.DispatchAndWrapAsync(() =>
        {
            var points = new List<PointAdapter>(pointsNumber);
            for (var i = 0; i < pointsNumber; i++)
            {
                points.Add(RandomPointCreator.CreateNew(
                    xMax: ClusterizerGuiConstants.DATA_MAX_X,
                    yMax: ClusterizerGuiConstants.DATA_MAX_Y,
                    zMax: ClusterizerGuiConstants.DATA_MAX_Z));
            }

            Points.AddRange(points);

            // Regenerate Bitmap:
            using (var bmp = BitmapGeneratorFromPoints.GenerateBitmapFromPoint(Points))
            {
                CurrentImage = bmp.GetBitmapImage();
            }
        }, () => IsIdle = true).ConfigureAwait(false);
    }

    public BitmapImage? CurrentImage
    {
        get => _currentImage;
        private set => SetProperty(ref _currentImage, value);
    }

    public bool IsIdle
    {
        get => _isIdle;
        set => SetProperty(ref _isIdle, value);
    }

    private void ExecuteClearPointsCommand()
    {
        Points.Clear();
        CurrentImage = null;
    }
}