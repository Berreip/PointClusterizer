using System;
using System.Windows.Media.Imaging;
using ClusterizerGui.Views.MainDisplay;
using ClusterizerLib;
using ClusterizerLib.Results;

namespace ClusterizerGui.Views.Algorithms.GridBase;

internal sealed class GridBasedHistory : HistoryBase
{
    public GridBasedHistory(
        IDisplayImageAndClusterController displayImageAndClusterController,
        int runNumber,
        TimeSpan duration,
        int nbInitialPoints,
        ClusterGlobalResult<IPoint> clusterResults, BitmapImage? sourceImage)
        : base(
            displayImageAndClusterController,
            runNumber,
            duration,
            nbInitialPoints,
            clusterResults,
            sourceImage)
    {
    }
}