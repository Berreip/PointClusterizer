using System;
using System.Threading.Tasks;

namespace ClusterizerGui.Views.Algorithms;

/// <summary>
/// Encapsulate an algorithm execution
/// </summary>
public interface IAlgorithmExecutor
{
    Task ExecuteAsync(Action<IPoint[]> action);
}

public interface IPoint
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }
}