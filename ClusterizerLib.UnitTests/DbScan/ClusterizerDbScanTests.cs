using ClusteringModels;
using ClusterizerLib.DbScanAlgorithm;

namespace ClusterizerLib.UnitTests.DbScan;

[TestFixture]
internal sealed class ClusterizerDbScanTests
{
    [Test]
    public void empty_clustering()
    {
        //Arrange

        //Act
        var res = ClusterizerDbScan.Run(Array.Empty<IPoint>(), 1.0, 4, o => o);

        //Assert
        Assert.AreEqual(0, res.ClusterResults.Count);
        Assert.AreEqual(0, res.UnClusteredPoints.Count);
    }
}