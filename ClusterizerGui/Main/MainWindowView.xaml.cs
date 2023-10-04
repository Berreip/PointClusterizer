namespace ClusterizerGui.Main
{
    public partial class MainWindowView 
    {
        public MainWindowView(IMainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            DataContext = mainWindowViewModel;
        }
    }
}