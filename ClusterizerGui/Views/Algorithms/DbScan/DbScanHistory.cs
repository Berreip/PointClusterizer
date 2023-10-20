using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ClusterizerGui.Views.ImportDatasets.Extraction;
using ClusterizerGui.Views.MainDisplay;
using ClusterizerGui.Views.MainDisplay.Adapters;
using ClusterizerLib;
using ClusterizerLib.Results;
using Color = System.Drawing.Color;

namespace ClusterizerGui.Views.Algorithms.DbScan;

internal sealed class DbScanHistory : HistoryBase
{
    public int Epsilon { get; }
    public int MinPointByCluster { get; }

    public DbScanHistory(
        IDisplayImageAndClusterController displayImageAndClusterController,
        int runNumber,
        TimeSpan duration,
        int nbInitialPoints,
        ClusterGlobalResult<PointWrapper> clusterResults,
        int epsilon,
        int minPointByCluster,
        BitmapImage? sourceImage,
        IconCategory category)
        : base(
            displayImageAndClusterController,
            runNumber,
            duration,
            nbInitialPoints,
            clusterResults,
            sourceImage,
            Brushes.Green,
            Color.Green, 
            category)
    {
        Epsilon = epsilon;
        MinPointByCluster = minPointByCluster;
    }
}