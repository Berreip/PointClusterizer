using System.ComponentModel;
using System.Linq;
using System.Windows.Media.Imaging;
using ClusterizerGui.Views.MainDisplay;
using ClusterizerLib;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using PRF.WPFCore.CustomCollections;

namespace ClusterizerGui.Views.Algorithms;

internal abstract class AlgorithmViewModelBase<T> : ViewModelBase where T : HistoryBase
{
    private readonly IAlgorithmExecutor _algorithmExecutor;
    protected readonly IDisplayImageAndClusterController _displayImageAndClusterController;
    private readonly ObservableCollectionRanged<T> _history;
    public ICollectionView History { get; }

    public IDelegateCommandLight RunAlgorithmCommand { get; }
    public IDelegateCommandLight ClearHistoryCommand { get; }
    public IDelegateCommandLight<T> DeleteSingleHistoryCommand { get; }

    protected AlgorithmViewModelBase(IAlgorithmExecutor algorithmExecutor, IDisplayImageAndClusterController displayImageAndClusterController)
    {
        History = ObservableCollectionSource.GetDefaultView(out _history);
        _algorithmExecutor = algorithmExecutor;
        _displayImageAndClusterController = displayImageAndClusterController;
        RunAlgorithmCommand = new DelegateCommandLight(ExecuteRunAlgorithmCommand);
        ClearHistoryCommand = new DelegateCommandLight(ExecuteClearHistoryCommand);
        DeleteSingleHistoryCommand = new DelegateCommandLight<T>(ExecuteDeleteSingleHistoryCommand);
    }

    private async void ExecuteRunAlgorithmCommand()
    {
        var img = _displayImageAndClusterController.GetCurrentImage();
        await _algorithmExecutor.ExecuteAsync(points =>
        {
            _history.Add(ExecuteAlgorithmImplementation(img, points));
            // hide initial data points
            _displayImageAndClusterController.ShowPointsOnMap = false;
        }).ConfigureAwait(false);
    }

    protected abstract T ExecuteAlgorithmImplementation(BitmapImage? img, IPoint[] points);

    private void ExecuteDeleteSingleHistoryCommand(T elementToRemove)
    {
        if (_history.Remove(elementToRemove))
        {
            elementToRemove.Dispose();
        }
        
        // show initial data points if no more history
        if (_history.Count == 0)
        {
            _displayImageAndClusterController.ShowPointsOnMap = true;
        }
    }

    private void ExecuteClearHistoryCommand()
    {
        var all = _history.ToArray();
        _history.Clear();
        foreach (var data in all)
        {
            data.Dispose();
        }

        // show initial data points
        _displayImageAndClusterController.ShowPointsOnMap = true;
    }
}