using System.ComponentModel;
using System.Linq;
using ClusterizerGui.Services;
using ClusterizerGui.Utils;
using ClusterizerGui.Views.ImportDatasets.Adapter;
using ClusterizerGui.Views.ImportDatasets.Extraction;
using PRF.Utils.CoreComponents.IO;
using PRF.WPFCore;
using PRF.WPFCore.Browsers;
using PRF.WPFCore.Commands;
using PRF.WPFCore.CustomCollections;

namespace ClusterizerGui.Views.ImportDatasets;

internal interface IImportDatasetsViewModel
{
}

internal sealed class ImportDatasetsViewModel : ViewModelBase, IImportDatasetsViewModel
{
    private const string SEPARATOR_TAB_ALLIAS = "TAB";
    private string? _selectedFilePath;
    public IDelegateCommandLight SelectFileCommand { get; }
    public IDelegateCommandLight<DataSetAdapter> RemoveDatasetCommand { get; }
    public IDelegateCommandLight ValidateImportCommand { get; }
    public IDelegateCommandLight RefreshCurrentDataCommand { get; }
    public int[] AvailableStartingPosition { get; } = Enumerable.Range(0, 100).ToArray();
    public int[] AvailableColumnPositions { get; } = Enumerable.Range(DatasetLoader.GUESS_COLUMN_POSITION, 20).ToArray();
    public string[] AvailableSeparators { get; } = { ";", ",", "|", SEPARATOR_TAB_ALLIAS };

    private readonly IDatasetManager _datasetManager;

    public ICollectionView Datasets { get; }
    private readonly ObservableCollectionRanged<DataSetAdapter> _datasets;
    private readonly ObservableCollectionRanged<CsvLineAdapter> _currentFileContent;
    private int _dataStartingPosition;
    private string _selectedSeparator;
    private int _nameHeaderPosition; // guess auto
    private int _latitudeHeaderPosition; // guess auto
    private int _categoryHeaderPosition; // guess auto
    private int _longitudeHeaderPosition; // guess auto

    private int _distinctCategoriesCount;
    private int _dataCount;
    private IFileInfo? _validFile;
    private string _miscContent;

    public ICollectionView CurrentFileContent { get; }

    public ImportDatasetsViewModel(IDatasetManager datasetManager)
    {
        _datasetManager = datasetManager;
        datasetManager.OnDatasetUpdated += ReloadDatasets;
        Datasets = ObservableCollectionSource.GetDefaultView(datasetManager.GetAllDatasets().Select(o => new DataSetAdapter(o)), out _datasets);
        CurrentFileContent = ObservableCollectionSource.GetDefaultView(out _currentFileContent);

        CurrentFileContent.Filter = FilterSkippedData;

        SelectFileCommand = new DelegateCommandLight(ExecuteSelectFileCommand);
        ValidateImportCommand = new DelegateCommandLight(ExecuteValidateImportCommand);
        RefreshCurrentDataCommand = new DelegateCommandLight(ExecuteRefreshCurrentDataCommand);
        RemoveDatasetCommand = new DelegateCommandLight<DataSetAdapter>(ExecuteRemoveDatasetCommand);
        _dataStartingPosition = 1; // by default, skip the header
        _selectedSeparator = ";";
        ResetPosition();
    }

    private void ResetPosition()
    {
        NameHeaderPosition = DatasetLoader.GUESS_COLUMN_POSITION;
        LatitudeHeaderPosition = DatasetLoader.GUESS_COLUMN_POSITION;
        CategoryHeaderPosition = DatasetLoader.GUESS_COLUMN_POSITION;
        LongitudeHeaderPosition = DatasetLoader.GUESS_COLUMN_POSITION;
    }

    private void ExecuteValidateImportCommand()
    {
        // TODO PBO
    }

    private void ReloadDatasets()
    {
        _datasets.Reset(_datasetManager.GetAllDatasets().Select(o => new DataSetAdapter(o)));
    }

    private void ExecuteSelectFileCommand()
    {
        var file = BrowserDialogManager.OpenFileBrowser("csv files (*.csv)|*.csv");
        if (file != null)
        {
            SelectedFilePath = file.FullName;
        }
    }

    private void ExecuteRemoveDatasetCommand(DataSetAdapter datasetToRemove)
    {
        _datasetManager.RemoveDataSet(datasetToRemove.GetUnderlyingDataset());
    }

    public string? SelectedFilePath
    {
        get => _selectedFilePath;
        set
        {
            if (SetProperty(ref _selectedFilePath, value))
            {
                if (FileValidationHelper.TryValidateFileInput(value, out var validFile))
                {
                    ResetPosition(); // guess again
                    _validFile = validFile;
                    AsyncWrapper.DispatchInFireAndForgetAndWrapAsync(() => ReLoadDataset(validFile));
                }
            }
        }
    }

    public int DataStartingPosition
    {
        get => _dataStartingPosition;
        set => SetProperty(ref _dataStartingPosition, value);
    }

    public string SelectedCsvSeparator
    {
        get => _selectedSeparator;
        set => SetProperty(ref _selectedSeparator, value);
    }

    public int NameHeaderPosition
    {
        get => _nameHeaderPosition;
        set => SetProperty(ref _nameHeaderPosition, value);
    }

    public int LatitudeHeaderPosition
    {
        get => _latitudeHeaderPosition;
        set => SetProperty(ref _latitudeHeaderPosition, value);
    }

    public int LongitudeHeaderPosition
    {
        get => _longitudeHeaderPosition;
        set => SetProperty(ref _longitudeHeaderPosition, value);
    }

    public int CategoryHeaderPosition
    {
        get => _categoryHeaderPosition;
        set => SetProperty(ref _categoryHeaderPosition, value);
    }

    public int DataCount
    {
        get => _dataCount;
        private set => SetProperty(ref _dataCount, value);
    }

    public int DistinctCategoriesCount
    {
        get => _distinctCategoriesCount;
        private set => SetProperty(ref _distinctCategoriesCount, value);
    }

    public string MiscContent
    {
        get => _miscContent;
        private set => SetProperty(ref _miscContent, value);
    }

    private async void ExecuteRefreshCurrentDataCommand()
    {
        await AsyncWrapper.DispatchAndWrapAsync(() =>
        {
            var validFile = _validFile;
            if (validFile != null)
            {
                ReLoadDataset(validFile);
            }
        }).ConfigureAwait(false);
    }

    private void ReLoadDataset(IFileInfo validFile)
    {
        // handle tab specifically:
        var separator = _selectedSeparator == SEPARATOR_TAB_ALLIAS ? "\t" : _selectedSeparator;

        var loadDatasetResult = DatasetLoader.LoadDataset(validFile, separator, _dataStartingPosition, _nameHeaderPosition, _longitudeHeaderPosition, _latitudeHeaderPosition, _categoryHeaderPosition);
        // refresh guessed position if needed
        NameHeaderPosition = loadDatasetResult.GuessedPositions.GuessedNamePosition;
        LongitudeHeaderPosition = loadDatasetResult.GuessedPositions.GuessedLongitudePosition;
        LatitudeHeaderPosition = loadDatasetResult.GuessedPositions.GuessedLatitudePosition;
        CategoryHeaderPosition = loadDatasetResult.GuessedPositions.GuessedCategoryPosition;
        _currentFileContent.Reset(loadDatasetResult.CsvLines);
        DataCount = loadDatasetResult.CsvLines.Count;
        DistinctCategoriesCount = loadDatasetResult.CsvLines.Select(o => o.Category).ToHashSet().Count;
        MiscContent = loadDatasetResult.GuessedPositions.MiscHeaders;
    }

    private static bool FilterSkippedData(object obj)
    {
        if (obj is CsvLineAdapter lineAdapter)
        {
            return lineAdapter.IsNotSkipped;
        }

        return false;
    }
}