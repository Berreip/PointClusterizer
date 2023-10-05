using ClusterizerGui.Views.Algorithms.Adapters;

namespace ClusterizerGui.Views.Algorithms.GridBase;

internal sealed partial class AlgorithmGridBaseView : IAlgorithmView
{
    public AlgorithmGridBaseView(AlgorithmGridBaseViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}