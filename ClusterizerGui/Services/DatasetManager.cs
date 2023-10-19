using System;
using System.Collections.Generic;
using System.Linq;
using ClusterizerGui.Views.ImportDatasets.Extraction;
using ClusterizerGui.Views.MainDisplay.Adapters;
using PRF.Utils.CoreComponents.IO;

namespace ClusterizerGui.Services;

/// <summary>
/// Host the list of loaded datasets
/// </summary>
internal interface IDatasetManager
{
    event Action OnDatasetUpdated;
    IReadOnlyCollection<IDataset> GetAllDatasets();
    void RemoveDataSet(IDataset dataset);
    void AddNewDataset(IDataset dataset);
}

internal interface IDataset
{
    IFileInfo File { get; }
    int NbFeatures { get; }
    IReadOnlyCollection<PointWrapper> GetDatasetContent();
}

internal sealed class DatasetManager : IDatasetManager
{
    public event Action? OnDatasetUpdated;
    private readonly HashSet<IDataset> _datasets = new HashSet<IDataset>();
    private readonly object _key = new object();

    public IReadOnlyCollection<IDataset> GetAllDatasets()
    {
        lock (_key)
        {
            return _datasets.ToArray();
        }
    }

    public void RemoveDataSet(IDataset dataset)
    {
        var shouldNotify = false;
        lock (_key)
        {
            if (_datasets.Remove(dataset))
            {
                shouldNotify = true;
            }
        }

        if (shouldNotify)
        {
            RaiseOnDatasetUpdated();
        }
    }

    public void AddNewDataset(IDataset dataset)
    {
        lock (_key)
        {
            _datasets.Add(dataset);
        }

        RaiseOnDatasetUpdated();
    }

    private void RaiseOnDatasetUpdated()
    {
        OnDatasetUpdated?.Invoke();
    }
}

internal sealed class Dataset : IDataset
{
    private readonly CategoryMapper _categoryMapper;
    private readonly PointWrapper[] _datasetPoints;

    public Dataset(IFileInfo file, IEnumerable<PointWrapper> datasetPoints, CategoryMapper categoryMapper)
    {
        _categoryMapper = categoryMapper;
        _datasetPoints = datasetPoints.ToArray();
        File = file;
        NbFeatures = _datasetPoints.Length;
    }

    public IFileInfo File { get; }
    public int NbFeatures { get; }
    public IReadOnlyCollection<PointWrapper> GetDatasetContent()
    {
        return _datasetPoints;
    }
}