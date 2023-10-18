using ClusterizerGui.Views.Algorithms.Adapters;

namespace ClusterizerGui.Views.Algorithms.HierarchicalGreedyClustering;

internal sealed partial class HierarchicalGreedyClusteringView : IAlgorithmView
{
    public HierarchicalGreedyClusteringView(HierarchicalGreedyClusteringViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}