using System.Diagnostics;

namespace ClusteringGridBase.UnitTests;

[TestFixture]
internal sealed class PerformancesTests
{

    // Same case than previous, but this times, distance is 3 so it should be merged
    [Test]
    [Repeat(10)]
    public void points_clustering_performances()
    {
        //Arrange
        var random = new Random();
        const int xLenght = 2000;
        const int yLenght = 1500;
        const int zLenght = 1200;
        const int nbPoint = 300_000;
        var points = Enumerable
            .Range(0, nbPoint)
            .Select(_ => new PointUnitTest(
                x: random.NextDouble() * xLenght,
                y: random.NextDouble() * yLenght,
                z: random.NextDouble() * zLenght))
            .ToArray();

        //Act
        var watch = Stopwatch.StartNew();
        Clusterizer.Run(
            points: points,
            // random initial origin
            origin: new PointUnitTest(
                x: random.NextDouble() * 100,
                y: random.NextDouble() * 100,
                z: random.NextDouble() * 100),
            xLenght: xLenght,
            yLenght: yLenght,
            zLenght: zLenght,
            nbPartX: random.Next(4, 40),
            nbPartY: random.Next(4, 40),
            nbPartZ: random.Next(4, 40),
            clusteringDensityThreshold: random.Next(100, 400),
            neighbouringMergingDistance: 2);
        watch.Stop();

        //Assert
        Console.WriteLine($"Elapsed time for {nbPoint} points is {watch.ElapsedMilliseconds} ms");
        // no exception
    }
}