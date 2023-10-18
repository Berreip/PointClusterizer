using ClusterizerGui.Services.Navigation;

namespace ClusterizerGui.Views.ImportDatasets;

internal partial class ImportDatasetsView : INavigeablePanel
{
    public ImportDatasetsView(IImportDatasetsViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }

    public void OnNavigateTo()
    {
    }

    public void OnNavigateExit()
    {
    }
}