using System.Windows.Media.Imaging;
using ClusterizerGui.Views.MainDisplay.Adapters;
using ClusterizerGui.Views.MainDisplay.Helpers;
using PRF.WPFCore;

namespace ClusterizerGui.Views.Algorithms.Adapters;

public class GridImageAdapter : ViewModelBase, IPointImageAdapter
{
    private BitmapImage _bitmapImage;
    public int DisplayIndex { get; } = DisplayIndexConstant.IMAGE_POINTS_FROM_PREVIOUS_RUN;
    public double WidthPercentage { get; } = 0;
    public double HeightPercentage { get; } = 0;
    
    public GridImageAdapter(BitmapImage grid)
    {
        _bitmapImage = grid;
    }

    public void UpdateImage(BitmapImage updatedGrid)
    {
        BitmapImage = updatedGrid;
    }
    
    public BitmapImage BitmapImage
    {
        get => _bitmapImage;
        private set => SetProperty(ref _bitmapImage, value);
    }

}