using System.Windows;
using ClusterizerGui.Main;
using PRF.Utils.Injection.Containers;
using PRF.Utils.Injection.Utils;

namespace ClusterizerGui.Services.Injection;

public class ClusterizerGuiBootstrapper
{
    private readonly InjectionContainerSimpleInjector _internalContainer;

    public ClusterizerGuiBootstrapper()
    {
        _internalContainer = new InjectionContainerSimpleInjector();
    }

    public TMainWindow Run<TMainWindow>() where TMainWindow : class
    {
        Register();
        Initialize();
        return _internalContainer.Resolve<TMainWindow>();
    }

    private void Register()
    {
        _internalContainer.RegisterType<MainWindowView>(LifeTime.Singleton);
        _internalContainer.Register<IMainWindowViewModel, MainWindowViewModel>(LifeTime.Singleton);
    }

    private void Initialize()
    {
       
    }

    public void OnExit(object sender, ExitEventArgs e)
    {
        _internalContainer.Dispose();
    }
}