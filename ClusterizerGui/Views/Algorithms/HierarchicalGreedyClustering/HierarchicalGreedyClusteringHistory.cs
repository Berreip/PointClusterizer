using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ClusterizerGui.Views.ImportDatasets.Extraction;
using ClusterizerGui.Views.MainDisplay;
using ClusterizerGui.Views.MainDisplay.Adapters;
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
        ClusterGlobalResult<PointWrapper> clusterResults,
        BitmapImage? sourceImage,
        int searchRadius, 
        int numberOfLevels,
        IconCategory category)
        : base(
            displayImageAndClusterController,
            runNumber,
            duration,
            nbInitialPoints,
            clusterResults,
            sourceImage, 
            Brushes.Indigo, 
            Color.Indigo,
            category)
    {
        SearchRadius = searchRadius;
        NumberOfLevels = numberOfLevels;
    }
}