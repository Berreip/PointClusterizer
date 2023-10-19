using System.Collections.Generic;
using ClusterizerGui.Services;
using PRF.WPFCore;

namespace ClusterizerGui.Views.MainDisplay.Adapters;

internal sealed class DatasetAvailableAdapters : ViewModelBase
{
    private readonly IDataset _dataset;

    public DatasetAvailableAdapters(IDataset dataset)
    {
        _dataset = dataset;
    }

    public string DatasetName => _dataset.File.Name;
    public string DatasetFileFulName => _dataset.File.FullName;
    public int FeaturesCount => _dataset.NbFeatures;

    public IReadOnlyCollection<PointWrapper> GetDatasetContent()
    {
        return _dataset.GetDatasetContent();
    }
}