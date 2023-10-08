using System.Drawing;
using ClusterizerLib.GridBased;

namespace ClusterizerLib.UnitTests.GridBase;

[TestFixture]
internal sealed class ClusterizerGridBaseTests
{
    [Test]
    public void empty_clustering()
    {
        //Arrange
        var points = Array.Empty<IPoint>();

        //Act
        var res = ClusterizerGridBase.Run(
            points: points,
            aoi: new Rectangle(x: 0, y: 0, width: 600, height: 300),
            nbRowTargeted: 10,
            nbColumnTargeted: 20,
            clusteringDensityThreshold: 20,
            numberOfPasses: 1);

        //Assert
        Assert.AreEqual(0, res.ClusterResults.Count);
        Assert.AreEqual(0, res.UnClusteredPoint.Count);
    }

    [Test]
    public void only_unclustered_points()
    {
        //Arrange
        var points = new[]
        {
            new PointUnitTest(1, 1, 99),
            new PointUnitTest(1, 1, 99),
            new PointUnitTest(1, 1, 99),
            new PointUnitTest(10, 17, 99),
        };

        //Act
        var res = ClusterizerGridBase.Run(
            points: points,
            aoi: new Rectangle(x: 0, y: 0, width: 600, height: 300),
            nbRowTargeted: 10,
            nbColumnTargeted: 20,
            clusteringDensityThreshold: 20,
            numberOfPasses: 1);

        //Assert
        Assert.AreEqual(0, res.ClusterResults.Count);
        Assert.AreEqual(4, res.UnClusteredPoint.Count);
    }

    [Test]
    public void points_out_of_aoi_are_filtered()
    {
        //Arrange
        var points = new[]
        {
            new PointUnitTest(-1, 1, 99),
            new PointUnitTest(1, -1, 99),
            new PointUnitTest(601, 1, 99),
            new PointUnitTest(10, 301, 99),
        };

        //Act
        var res = ClusterizerGridBase.Run(
            points: points,
            aoi: new Rectangle(x: 0, y: 0, width: 600, height: 300),
            nbRowTargeted: 10,
            nbColumnTargeted: 20,
            clusteringDensityThreshold: 20,
            numberOfPasses: 1);

        //Assert
        Assert.AreEqual(0, res.ClusterResults.Count);
        Assert.AreEqual(0, res.UnClusteredPoint.Count);
    }

    [Test]
    public void points_limit_are_accepted()
    {
        //Arrange
        var points = new[]
        {
            new PointUnitTest(599, 299, 99),
            new PointUnitTest(0, 0, 99),
        };

        //Act
        var res = ClusterizerGridBase.Run(
            points: points,
            aoi: new Rectangle(x: 0, y: 0, width: 600, height: 300),
            nbRowTargeted: 10,
            nbColumnTargeted: 20,
            clusteringDensityThreshold: 20,
            numberOfPasses: 1);

        //Assert
        Assert.AreEqual(0, res.ClusterResults.Count);
        Assert.AreEqual(2, res.UnClusteredPoint.Count);
    }

    [Test]
    public void points_clustering_nominal()
    {
        //Arrange
        var points = Enumerable
            .Range(0, 2000)
            .Select(_ => new PointUnitTest(40, 40, 99))
            .ToArray();

        //Act
        var res = ClusterizerGridBase.Run(
            points: points,
            aoi: new Rectangle(x: 0, y: 0, width: 600, height: 300),
            nbRowTargeted: 10,
            nbColumnTargeted: 20,
            clusteringDensityThreshold: 20,
            numberOfPasses: 1);

        //Assert
        Assert.AreEqual(1, res.ClusterResults.Count);
        Assert.AreEqual(0, res.UnClusteredPoint.Count);
        Assert.AreEqual(2000, res.ClusterResults.Single().Points.Count);
    }

    [Test]
    public void points_clustering_multi_passes_nominal()
    {
        //Arrange
        var points = Enumerable
            .Range(0, 2000)
            .Select(_ => new PointUnitTest(40, 40, 99))
            .ToArray();

        //Act
        var res = ClusterizerGridBase.Run(
            points: points,
            aoi: new Rectangle(x: 0, y: 0, width: 600, height: 300),
            nbRowTargeted: 10,
            nbColumnTargeted: 20,
            clusteringDensityThreshold: 80,
            numberOfPasses: 3);

        //Assert
        Assert.AreEqual(1, res.ClusterResults.Count);
        Assert.AreEqual(0, res.UnClusteredPoint.Count);
        Assert.AreEqual(2000, res.ClusterResults.Single().Points.Count);
    }
    
    [Test]
    public void points_clustering_multi_passes_nominal_single_point()
    {
        //Arrange
        var points = new[] { new PointUnitTest(40, 40, 99) };

        //Act
        var res = ClusterizerGridBase.Run(
            points: points,
            aoi: new Rectangle(x: 0, y: 0, width: 600, height: 300),
            nbRowTargeted: 10,
            nbColumnTargeted: 20,
            clusteringDensityThreshold: 200,
            numberOfPasses: 3);

        //Assert
        Assert.AreEqual(0, res.ClusterResults.Count);
        Assert.AreEqual(1, res.UnClusteredPoint.Count);
    }
}