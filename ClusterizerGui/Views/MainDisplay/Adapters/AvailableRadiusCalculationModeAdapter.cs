using System;
using System.Drawing;
using PRF.WPFCore;

namespace ClusterizerGui.Views.MainDisplay.Adapters;

public sealed class AvailableRadiusCalculationModeAdapter : ViewModelBase
{
    private readonly string _modeName;
    private readonly Func<int, Rectangle, int> _radiusComputationFunc;

    public AvailableRadiusCalculationModeAdapter(string modeName, Func<int, Rectangle, int> radiusComputationFunc)
    {
        _modeName = modeName;
        _radiusComputationFunc = radiusComputationFunc;
    }

    /// <summary>
    /// Compute a radius depending on point count
    /// </summary>
    public int ComputeRadius(int pointCounts, Rectangle aoi)
    {
        return _radiusComputationFunc.Invoke(pointCounts, aoi);
    }

    public override string ToString() => _modeName;
}