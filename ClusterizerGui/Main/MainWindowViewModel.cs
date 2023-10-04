using ClusterizerGui.Services.Navigation;
using PRF.WPFCore;

namespace ClusterizerGui.Main;

internal interface IMainWindowViewModel
{
}

internal sealed class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
    public IMenuNavigator MenuNavigator { get; }
    public MainWindowViewModel(IMenuNavigator menuNavigator)
    {
        MenuNavigator = menuNavigator;
    }
}