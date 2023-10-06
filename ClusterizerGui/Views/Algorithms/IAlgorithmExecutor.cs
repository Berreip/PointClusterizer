using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ClusterizerLib;

namespace ClusterizerGui.Views.Algorithms;

/// <summary>
/// Encapsulate an algorithm execution
/// </summary>
public interface IAlgorithmExecutor
{
    Task ExecuteAsync(Action<IPoint[]> action);
}