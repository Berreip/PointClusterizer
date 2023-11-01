using System.Collections.Generic;
using ClusterizerGui.Views.MainDisplay.Adapters;
using ClusterizerGui.Views.MainDisplay.Helpers;
using PRF.WPFCore;

namespace ClusterizerGui.Views.MainDisplay;

internal interface IRadiusModeProvider
{
    IReadOnlyList<AvailableRadiusCalculationModeAdapter> AvailableRadiusCalculationMode { get; }
    AvailableRadiusCalculationModeAdapter SelectedRadiusCalculationMode { get; set; }
}

internal sealed class RadiusModeProvider : ViewModelBase, IRadiusModeProvider
{
    private AvailableRadiusCalculationModeAdapter _selectedRadiusCalculationMode;
    public IReadOnlyList<AvailableRadiusCalculationModeAdapter> AvailableRadiusCalculationMode { get; } = new[]
    {
        new AvailableRadiusCalculationModeAdapter("4xLog2(N+1)", RadiusCalculation.Radius4Log2),
        new AvailableRadiusCalculationModeAdapter("4xLog10(N+1)", RadiusCalculation.Radius4Log10),
        new AvailableRadiusCalculationModeAdapter("10xLog10(N+1)", RadiusCalculation.Radius10Log10),
        new AvailableRadiusCalculationModeAdapter("Log(N+1)", RadiusCalculation.RadiusLog),
        new AvailableRadiusCalculationModeAdapter("Linear/10", RadiusCalculation.RadiusLinear10),
        new AvailableRadiusCalculationModeAdapter("Surface based", RadiusCalculation.RadiusSurfaceBased),
    };

    public RadiusModeProvider()
    {
        _selectedRadiusCalculationMode = AvailableRadiusCalculationMode[0];
    }

    public AvailableRadiusCalculationModeAdapter SelectedRadiusCalculationMode
    {
        get => _selectedRadiusCalculationMode;
        set => SetProperty(ref _selectedRadiusCalculationMode, value);
    }

}