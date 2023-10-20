﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Media.Imaging;
using ClusterizerGui.Views.ImportDatasets.Extraction;
using ClusterizerGui.Views.MainDisplay;
using ClusterizerGui.Views.MainDisplay.Adapters;
using ClusterizerLib;
using ClusterizerLib.HierarchicalGreedy;

namespace ClusterizerGui.Views.Algorithms.HierarchicalGreedyClustering;

/// <summary>
/// based on https://blog.mapbox.com/clustering-millions-of-points-on-a-map-with-supercluster-272046ec5c97
/// </summary>
internal sealed class HierarchicalGreedyClusteringViewModel: AlgorithmViewModelBase<HierarchicalGreedyClusteringHistory>
{
    private int _runHGC;
    private int _searchRadius;
    private int _numberOfLevels;
    
    public IReadOnlyList<int> AvailableNumberOfLevels { get; } = Enumerable.Range(1, 20).ToArray();
    
    public HierarchicalGreedyClusteringViewModel(IAlgorithmExecutor algorithmExecutor, IDisplayImageAndClusterController displayImageAndClusterController)
        : base(algorithmExecutor, displayImageAndClusterController)
    {
        _searchRadius = 5;
        _numberOfLevels = 4;
    }
    
    public int SearchRadius
    {
        get => _searchRadius;
        set => SetProperty(ref _searchRadius, value);
    }

    public int NumberOfLevels
    {
        get => _numberOfLevels;
        set => SetProperty(ref _numberOfLevels, value);
    }

    
    protected override HierarchicalGreedyClusteringHistory ExecuteAlgorithmImplementation(BitmapImage? img, PointWrapper[] points, IconCategory category)
    {
        var searchRadius = SearchRadius;
        var numberOfLevels = NumberOfLevels;

        var watch = Stopwatch.StartNew();
        // execute
        var results = ClusterizerHierarchicalGreedy.Run(points: points, searchRadius: searchRadius, numberOfLevels: numberOfLevels);
        watch.Stop();
        return new HierarchicalGreedyClusteringHistory(
            displayImageAndClusterController: _displayImageAndClusterController,
            runNumber: Interlocked.Increment(ref _runHGC),
            duration: watch.Elapsed,
            nbInitialPoints: points.Length,
            clusterResults: results,
            searchRadius: searchRadius,
            numberOfLevels: numberOfLevels,
            sourceImage: img, 
            category: category);
    }
}