using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ClusterizerGui.Views.MainDisplay;
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
        ClusterGlobalResult<IPoint> clusterResults,
        int epsilon,
        int minPointByCluster,
        BitmapImage? sourceImage)
        : base(
            displayImageAndClusterController,
            runNumber,
            duration,
            nbInitialPoints,
            clusterResults,
            sourceImage, Brushes.Green, Color.DarkGreen)
    {
        Epsilon = epsilon;
        MinPointByCluster = minPointByCluster;
    }
}