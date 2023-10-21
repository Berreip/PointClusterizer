using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ClusterizerGui.Views.ImportDatasets.Extraction;
using ClusterizerGui.Views.MainDisplay;
using ClusterizerGui.Views.MainDisplay.Adapters;
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
        ClusterGlobalResult<PointWrapper> clusterResults,
        BitmapImage? sourceImage,
        int minimumDensity, 
        int selectedPassesNumber, 
        int columns,
        int rows,
        IconCategory category)
        : base(
            displayImageAndClusterController,
            runNumber,
            duration,
            nbInitialPoints,
            clusterResults,
            sourceImage, 
            Brushes.Orange, 
            Color.Orange,
            category)
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