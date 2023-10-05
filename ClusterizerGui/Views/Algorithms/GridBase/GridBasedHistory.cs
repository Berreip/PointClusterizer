using System;
using ClusterizerLib;
using ClusterizerLib.Results;
using PRF.WPFCore;

namespace ClusterizerGui.Views.Algorithms.GridBase;

internal sealed class GridBasedHistory : ViewModelBase
{
    public int RunNumber { get; }
    public TimeSpan Duration { get; }
    public int NbInitialPoints { get; }
    public int NbClusters { get; }
    public int UnClusteredPoint { get; }
    public string DurationInSecond { get; }

    public GridBasedHistory(
        int runNumber, 
        TimeSpan duration, 
        int nbInitialPoints,
        ClusterGlobalResult<IPoint> clusterResults)
    {
        RunNumber = runNumber;
        Duration = duration;
        DurationInSecond = $"{duration.TotalSeconds:0.##}";
        NbInitialPoints = nbInitialPoints;
        NbClusters = clusterResults.ClusterResults.Count;
        UnClusteredPoint = clusterResults.UnClusteredPoint.Count;
    }

}