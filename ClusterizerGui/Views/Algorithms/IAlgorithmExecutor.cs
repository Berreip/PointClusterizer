using System;
using System.Threading.Tasks;
using ClusterizerGui.Views.MainDisplay.Adapters;

namespace ClusterizerGui.Views.Algorithms;

/// <summary>
/// Encapsulate an algorithm execution
/// </summary>
internal interface IAlgorithmExecutor
{
    Task ExecuteAsync(Action<PointWrapper[]> action);
}