using ClusterizerGui.Views.Algorithms.Adapters;

namespace ClusterizerGui.Views.Algorithms.DbScan;

internal partial class AlgorithmDbScanView : IAlgorithmView
{
    public AlgorithmDbScanView(AlgorithmDbScanViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}