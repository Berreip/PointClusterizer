using System.Windows;
using ClusterizerGui.Main;
using ClusterizerGui.Services.Navigation;
using ClusterizerGui.Views.ImportDatasets;
using ClusterizerGui.Views.ImportDatasets.Extraction;
using ClusterizerGui.Views.MainDisplay;
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
        _internalContainer.Register<IMenuNavigator, MenuNavigator>(LifeTime.Singleton);
        _internalContainer.Register<IDatasetManager, DatasetManager>(LifeTime.Singleton);
        
        // main display:
        _internalContainer.RegisterType<MainDisplayView>(LifeTime.Singleton);
        _internalContainer.Register<IMainDisplayViewModel, MainDisplayViewModel>(LifeTime.Singleton);
        
        // import datasets:
        _internalContainer.RegisterType<ImportDatasetsView>(LifeTime.Singleton);
        _internalContainer.Register<IImportDatasetsViewModel, ImportDatasetsViewModel>(LifeTime.Singleton);

    }

    private void Initialize()
    {
        IconExtraction.LoadIconUri();
        _internalContainer.Resolve<IMenuNavigator>().NavigateToFirstView();
    }

    public void OnExit(object sender, ExitEventArgs e)
    {
        _internalContainer.Dispose();
    }
}