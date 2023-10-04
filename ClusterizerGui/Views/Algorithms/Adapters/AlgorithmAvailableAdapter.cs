using System;

namespace ClusterizerGui.Views.Algorithms.Adapters;

internal interface IAlgorithmView
{
}
internal sealed class AlgorithmAvailableAdapter
{
    private readonly Func<IAlgorithmView> _createView;
    public string AlgorithmName { get; }

    public AlgorithmAvailableAdapter(string algorithmName, Func<IAlgorithmView> createView)
    {
        _createView = createView;
        AlgorithmName = algorithmName;
    }

    public IAlgorithmView CreateView()
    {
        return _createView();
    }

    public override string ToString() => AlgorithmName;
}
