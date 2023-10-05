using System;
using ClusterizerLib;
using ClusterizerLib.Results;
using PRF.WPFCore;

namespace ClusterizerGui.Views.Algorithms.DbScan;

internal sealed class DbScanHistory : ViewModelBase
{
    public int RunNumber { get; }
    public TimeSpan Duration { get; }
    public int NbInitialPoints { get; }
    public int Epsilon { get; }
    public int MinPointByCluster { get; }
    public int NbClusters { get; }
    public int UnClusteredPoint { get; }
    public string DurationInSecond { get; }

    public DbScanHistory(
        int runNumber, 
        TimeSpan duration, 
        int nbInitialPoints,
        ClusterGlobalResult<IPoint> clusterResults,
        int epsilon,
        int minPointByCluster)
    {
        RunNumber = runNumber;
        Duration = duration;
        DurationInSecond = $"{duration.TotalSeconds:0.##}";
        NbInitialPoints = nbInitialPoints;
        Epsilon = epsilon;
        MinPointByCluster = minPointByCluster;
        NbClusters = clusterResults.ClusterResults.Count;
        UnClusteredPoint = clusterResults.UnClusteredPoint.Count;
    }

}