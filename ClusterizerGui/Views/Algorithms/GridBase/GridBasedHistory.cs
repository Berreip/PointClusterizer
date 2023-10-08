using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ClusterizerGui.Views.MainDisplay;
using ClusterizerLib;
using ClusterizerLib.Results;
using Color = System.Drawing.Color;

namespace ClusterizerGui.Views.Algorithms.GridBase;

internal sealed class GridBasedHistory : HistoryBase
{
    public GridBasedHistory(
        IDisplayImageAndClusterController displayImageAndClusterController,
        int runNumber,
        TimeSpan duration,
        int nbInitialPoints,
        ClusterGlobalResult<IPoint> clusterResults,
        BitmapImage? sourceImage,
        int minimumDensity, 
        int selectedPassesNumber, 
        int columns,
        int rows)
        : base(
            displayImageAndClusterController,
            runNumber,
            duration,
            nbInitialPoints,
            clusterResults,
            sourceImage, Brushes.Orange, Color.Red)
    {
        MinimumDensity = minimumDensity;
        SelectedPassesNumber = selectedPassesNumber;
        Columns = columns;
        Rows = rows;
    }

    public int SelectedPassesNumber { get; }
    public int MinimumDensity { get; }
    public int Columns { get; }
    public int Rows { get; }
}