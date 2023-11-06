using System.Diagnostics;

namespace ClusteringGridBase.UnitTests;

[TestFixture]
internal sealed class PerformancesTests
{
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
        var res = Clusterizer.Run(
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
        // Check total number of points
        var totalNumberOfPoints =  res.OutsideAoiPoints.Count + res.UnClusteredPoints.Count + res.ClusterResults.Sum(o => o.Points.Count);
        Assert.AreEqual(nbPoint, totalNumberOfPoints);
    }

    [Test]
    [Repeat(10)]
    public void points_clustering_out_of_cube_repro_bug()
    {
        //Arrange
        var random = new Random();
        const int xLenght = 300;
        const int yLenght = 300;
        const int zLenght = 300;
        const int nbPoint = 30_000;
        var points = Enumerable
            .Range(0, nbPoint)
            .Select(_ => new PointUnitTest(
                x: random.NextDouble() * 1000,
                y: random.NextDouble() * 1000,
                z: random.NextDouble() * 1000))
            .ToArray();

        //Act
        var watch = Stopwatch.StartNew();
        var res = Clusterizer.Run(
            points: points,
            origin: new PointUnitTest(
                x: 100,
                y: 150,
                z: 180),
            xLenght: xLenght,
            yLenght: yLenght,
            zLenght: zLenght,
            nbPartX: random.Next(6, 20),
            nbPartY: random.Next(6, 20),
            nbPartZ: random.Next(6, 20),
            clusteringDensityThreshold: 50,
            neighbouringMergingDistance: 2);
        watch.Stop();

        //Assert
        Console.WriteLine($"Elapsed time for {nbPoint} points is {watch.ElapsedMilliseconds} ms");
        // Check total number of points
        var totalNumberOfPoints = res.OutsideAoiPoints.Count + res.UnClusteredPoints.Count + res.ClusterResults.Sum(o => o.Points.Count);
        Assert.AreEqual(nbPoint, totalNumberOfPoints);
    }
}