namespace ClusterizerGui.Main
{
    internal partial class MainWindowView 
    {
        public MainWindowView(IMainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            DataContext = mainWindowViewModel;
        }
    }
}