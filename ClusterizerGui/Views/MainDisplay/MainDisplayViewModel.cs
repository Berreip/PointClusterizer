using PRF.WPFCore;
using PRF.WPFCore.Commands;

namespace ClusterizerGui.Views.MainDisplay;

internal interface IMainDisplayViewModel
{
}

internal sealed class MainDisplayViewModel : ViewModelBase, IMainDisplayViewModel
{
    public IDelegateCommandLight AddPointsCommand { get; }
    public MainDisplayViewModel()
    {
        AddPointsCommand = new DelegateCommandLight(ExecuteAddPointsCommand);

    }

    private void ExecuteAddPointsCommand()
    {
        
    }
}