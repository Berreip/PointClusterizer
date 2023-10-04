using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ClusterizerGui.Views.Algorithms.Adapters;
using ClusterizerGui.Views.Algorithms.DbScan;
using PRF.Utils.Injection.Containers;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using PRF.WPFCore.CustomCollections;
using PRF.WPFCore.UiWorkerThread;

namespace ClusterizerGui.Views.MainDisplay;

internal interface IMainDisplayViewModel
{
}

internal sealed class MainDisplayViewModel : ViewModelBase, IMainDisplayViewModel
{
    private readonly ObservableCollectionRanged<AlgorithmAvailableAdapter> _algorithmsAvailable;
    private AlgorithmAvailableAdapter? _selectedAlgorithm;
    private IAlgorithmView? _selectedAlgorithmView;
    private int _selectedNbRows;
    private int _selectedNbColumn;
    public IDelegateCommandLight AddPointsCommand { get; }
    public ICollectionView AlgorithmsAvailable { get; }

    public IReadOnlyList<int> AvailableNbColumns { get; } = Enumerable.Range(1, 100).ToArray();
    public IReadOnlyList<int> AvailableNbRows { get; } = Enumerable.Range(1, 100).ToArray();

    public MainDisplayViewModel(IInjectionContainer container)
    {
        AddPointsCommand = new DelegateCommandLight(ExecuteAddPointsCommand);
        AlgorithmsAvailable = ObservableCollectionSource.GetDefaultView(new[]
        {
            new AlgorithmAvailableAdapter("DBSCAN", container.Resolve<AlgorithmDbScanView>)
        }, out _algorithmsAvailable);

        SelectedAlgorithm = _algorithmsAvailable.FirstOrDefault();
        _selectedNbRows = 10;
        _selectedNbColumn = 20;
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

    private void ExecuteAddPointsCommand()
    {
        // TODO
    }
}