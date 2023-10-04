using ClusterizerGui.Services.Navigation;

namespace ClusterizerGui.Views.MainDisplay;

internal partial class MainDisplayView : INavigeablePanel
{
    public MainDisplayView(IMainDisplayViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }

    public void OnNavigateTo()
    {
        // nothing
    }

    public void OnNavigateExit()
    {
        // nothing;
    }
}