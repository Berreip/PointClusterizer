using ClusterizerLib.HierarchicalGreedy;

namespace ClusterizerLib.UnitTests.HierarchicalGreedy;

public class ClusterizerHierarchicalGreedyTests
{
    [Test]
    public void empty_clustering()
    {
        //Arrange
        var points = Array.Empty<IPoint>();

        //Act
        var res = ClusterizerHierarchicalGreedy.Run(
            points: points,
            searchRadius: 20,
            numberOfLevels: 5);

        //Assert
        Assert.AreEqual(0, res.ClusterResults.Count);
        Assert.AreEqual(0, res.UnClusteredPoint.Count);
    }
}