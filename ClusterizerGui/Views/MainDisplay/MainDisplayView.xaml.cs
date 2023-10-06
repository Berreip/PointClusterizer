using System;
using System.Drawing;
using System.Windows.Input;
using ClusterizerGui.Services.Navigation;
using ClusterizerGui.Utils;

namespace ClusterizerGui.Views.MainDisplay;

internal partial class MainDisplayView : INavigeablePanel
{
    private readonly IMainDisplayViewModel _vm;

    public MainDisplayView(IMainDisplayViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
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

    private void CanvasEarth_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        var mousePosition = Mouse.GetPosition(CanvasEarth);
        // Compute the position of the mouse relatively to the current display size of the earth:
        var absoluteMousePositionFromImageReference = new Point(
            Math.Clamp((int)((mousePosition.X/CanvasEarth.ActualWidth) * ClusterizerGuiConstants.IMAGE_WIDTH), 0, ClusterizerGuiConstants.IMAGE_WIDTH),
            Math.Clamp((int)((mousePosition.Y/CanvasEarth.ActualHeight) * ClusterizerGuiConstants.IMAGE_HEIGHT), 0, ClusterizerGuiConstants.IMAGE_HEIGHT));
        _vm.GeneratePointsOnClick(absoluteMousePositionFromImageReference);
    }
}