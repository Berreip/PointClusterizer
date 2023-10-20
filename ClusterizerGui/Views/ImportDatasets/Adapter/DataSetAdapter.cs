using ClusterizerGui.Services;
using PRF.WPFCore;

namespace ClusterizerGui.Views.ImportDatasets.Adapter;

/// <summary>
/// Adapter of a loaded datasets with some points and a file source 
/// </summary>
internal sealed class DataSetAdapter : ViewModelBase
{
    private readonly IDataset _dataset;

    public DataSetAdapter(IDataset dataset)
    {
        _dataset = dataset;
    }

    public string FileFullName => _dataset.File.FullName;
    public string DataSetName => _dataset.DatasetName;
    public int NbFeatures => _dataset.NbFeatures;

    public IDataset GetUnderlyingDataset() => _dataset;
}