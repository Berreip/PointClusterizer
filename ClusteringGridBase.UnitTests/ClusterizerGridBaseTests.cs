using ClusteringModels;

namespace ClusteringGridBase.UnitTests;

[TestFixture]
internal sealed class ClusterizerGridBaseTests
{
    [Test]
    public void empty_clustering()
    {
        //Arrange
        var points = Array.Empty<IPoint>();

        //Act
        var res = Clusterizer.Run(
            points: points,
            origin: new PointUnitTest(0, 0, 0),
            xLenght: 600,
            yLenght: 300,
            zLenght: 20,
            nbPartX: 20,
            nbPartY: 10,
            nbPartZ: 1,
            clusteringDensityThreshold: 20,
            neighbouringMergingDistance: 1);

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
            new PointUnitTest(1, 1, 19),
            new PointUnitTest(1, 1, 19),
            new PointUnitTest(1, 1, 19),
            new PointUnitTest(10, 17, 19),
        };

        //Act
        var res = Clusterizer.Run(
            points: points,
            origin: new PointUnitTest(0, 0, 0),
            xLenght: 600,
            yLenght: 300,
            zLenght: 20,
            nbPartX: 20,
            nbPartY: 10,
            nbPartZ: 1,
            clusteringDensityThreshold: 20,
            neighbouringMergingDistance: 1);

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
            new PointUnitTest(-1, 1, 19),
            new PointUnitTest(1, -1, 19),
            new PointUnitTest(601, 1, 19),
            new PointUnitTest(10, 301, 19),
        };

        //Act
        var res = Clusterizer.Run(
            points: points,
            origin: new PointUnitTest(0, 0, 0),
            xLenght: 600,
            yLenght: 300,
            zLenght: 20,
            nbPartX: 20,
            nbPartY: 10,
            nbPartZ: 1,
            clusteringDensityThreshold: 20,
            neighbouringMergingDistance: 1);

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
            new PointUnitTest(599, 299, 19),
            new PointUnitTest(0, 0, 19),
        };

        //Act
        var res = Clusterizer.Run(
            points: points,
            origin: new PointUnitTest(0, 0, 0),
            xLenght: 600,
            yLenght: 300,
            zLenght: 20,
            nbPartX: 20,
            nbPartY: 10,
            nbPartZ: 1,
            clusteringDensityThreshold: 20,
            neighbouringMergingDistance: 1);

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
            .Select(_ => new PointUnitTest(40, 40, 19))
            .ToArray();

        //Act
        var res = Clusterizer.Run(
            points: points,
            origin: new PointUnitTest(0, 0, 0),
            xLenght: 600,
            yLenght: 300,
            zLenght: 20,
            nbPartX: 20,
            nbPartY: 10,
            nbPartZ: 1,
            clusteringDensityThreshold: 20,
            neighbouringMergingDistance: 1);

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
            .Select(_ => new PointUnitTest(40, 40, 19))
            .ToArray();

        //Act
        var res = Clusterizer.Run(
            points: points,
            origin: new PointUnitTest(0, 0, 0),
            xLenght: 600,
            yLenght: 300,
            zLenght: 20,
            nbPartX: 20,
            nbPartY: 10,
            nbPartZ: 1,
            clusteringDensityThreshold: 80,
            neighbouringMergingDistance: 3);

        //Assert
        Assert.AreEqual(1, res.ClusterResults.Count);
        Assert.AreEqual(0, res.UnClusteredPoint.Count);
        Assert.AreEqual(2000, res.ClusterResults.Single().Points.Count);
    }

    [Test]
    public void points_clustering_multi_passes_nominal_single_point()
    {
        //Arrange
        var points = new[] { new PointUnitTest(40, 40, 19) };

        //Act
        var res = Clusterizer.Run(
            points: points,
            origin: new PointUnitTest(0, 0, 0),
            xLenght: 600,
            yLenght: 300,
            zLenght: 20,
            nbPartX: 20,
            nbPartY: 10,
            nbPartZ: 1,
            clusteringDensityThreshold: 200,
            neighbouringMergingDistance: 3);

        //Assert
        Assert.AreEqual(0, res.ClusterResults.Count);
        Assert.AreEqual(1, res.UnClusteredPoint.Count);
    }


    [Test]
    public void points_clustering_multi_passes_should_merge_cluster()
    {
        //Arrange
        var points = Enumerable
            .Range(0, 2000)
            .Select(i =>
            {
                if (i % 2 == 0)
                {
                    // into cell 1
                    return new PointUnitTest(35, 35, 0);
                }

                // into neighbour cell 2
                return new PointUnitTest(45, 35, 0);
            })
            .ToArray();

        //Act
        // here, each cell is 10/10
        var res = Clusterizer.Run(
            points: points,
            origin: new PointUnitTest(0, 0, 0),
            xLenght: 200,
            yLenght: 100,
            zLenght: 20,
            nbPartX: 20,
            nbPartY: 10,
            nbPartZ: 1,
            clusteringDensityThreshold: 80,
            neighbouringMergingDistance: 2);

        //Assert
        Assert.AreEqual(1, res.ClusterResults.Count);
        Assert.AreEqual(0, res.UnClusteredPoint.Count);
        Assert.AreEqual(2000, res.ClusterResults.Single().Points.Count);
    }

    [Test]
    public void points_clustering_multi_passes_should_merge_only_on_two_passes()
    {
        //Arrange
        var points = Enumerable
            .Range(0, 3000)
            .Select(i =>
            {
                if (i % 3 == 0)
                {
                    // into cell 1
                    return new PointUnitTest(35, 35, 0);
                }

                if (i % 3 == 1)
                {
                    // into cell 2 (close)
                    return new PointUnitTest(45, 35, 0);
                }

                // into neighbour cell 3 (close to 2)
                return new PointUnitTest(55, 35, 0);
            })
            .ToArray();

        //Act
        // here, each cell is 10/10
        var res = Clusterizer.Run(
            points: points,
            origin: new PointUnitTest(0, 0, 0),
            xLenght: 200,
            yLenght: 100,
            zLenght: 20,
            nbPartX: 20,
            nbPartY: 10,
            nbPartZ: 1,
            clusteringDensityThreshold: 80,
            neighbouringMergingDistance: 2);

        //Assert
        Assert.AreEqual(2, res.ClusterResults.Count); // all cluster should not be fused
        Assert.AreEqual(0, res.UnClusteredPoint.Count);
        Assert.AreEqual(2000, res.ClusterResults[0].Points.Count);
        Assert.AreEqual(1000, res.ClusterResults[1].Points.Count);
    }

    // Same case than previous, but this times, distance is 3 so it should be merged
    [Test]
    public void points_clustering_multi_passes_should_merge_on_three_passes()
    {
        //Arrange
        var points = Enumerable
            .Range(0, 3000)
            .Select(i =>
            {
                if (i % 3 == 0)
                {
                    // into cell 1
                    return new PointUnitTest(35, 35, 0);
                }

                if (i % 3 == 1)
                {
                    // into cell 2 (close)
                    return new PointUnitTest(45, 35, 0);
                }

                // into neighbour cell 3 (close to 2)
                return new PointUnitTest(55, 35, 0);
            })
            .ToArray();

        //Act
        // here, each cell is 10/10
        var res = Clusterizer.Run(
            points: points,
            origin: new PointUnitTest(0, 0, 0),
            xLenght: 200,
            yLenght: 100,
            zLenght: 20,
            nbPartX: 20,
            nbPartY: 10,
            nbPartZ: 1,
            clusteringDensityThreshold: 80,
            neighbouringMergingDistance: 3);

        //Assert
        Assert.AreEqual(1, res.ClusterResults.Count); // all cluster should not be fused
        Assert.AreEqual(0, res.UnClusteredPoint.Count);
        Assert.AreEqual(3000, res.ClusterResults.Single().Points.Count);
    }
}