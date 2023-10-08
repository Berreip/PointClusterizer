using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ClusterizerGui.Views.MainDisplay;
using ClusterizerLib;
using ClusterizerLib.Results;
using Color = System.Drawing.Color;


namespace ClusterizerGui.Views.Algorithms.HierarchicalGreedyClustering;

internal sealed class HierarchicalGreedyClusteringHistory : HistoryBase
{
    public int SearchRadius { get; }
    public int NumberOfLevels { get; }

    public HierarchicalGreedyClusteringHistory(
        IDisplayImageAndClusterController displayImageAndClusterController,
        int runNumber,
        TimeSpan duration,
        int nbInitialPoints,
        ClusterGlobalResult<IPoint> clusterResults,
        BitmapImage? sourceImage,
        int searchRadius, 
        int numberOfLevels)
        : base(
            displayImageAndClusterController,
            runNumber,
            duration,
            nbInitialPoints,
            clusterResults,
            sourceImage, Brushes.LightBlue, Color.Red)
    {
        SearchRadius = searchRadius;
        NumberOfLevels = numberOfLevels;
    }
}