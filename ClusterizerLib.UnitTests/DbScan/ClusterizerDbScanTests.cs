using ClusterizerLib.DbScan;

namespace ClusterizerLib.UnitTests.DbScan;

[TestFixture]
internal sealed class ClusterizerDbScanTests
{
    [Test]
    public void empty_clustering()
    {
        //Arrange

        //Act
        var res = ClusterizerDbScan.Run(Array.Empty<IPoint>());

        //Assert
        Assert.AreEqual(0, res.Count);
    }
}