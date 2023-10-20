using ClusterizerGui.Views.ImportDatasets.Extraction;

namespace ClusterizerGui.UnitTests.View.ImportDatasets.Extraction;

[TestFixture]
internal sealed class PointExtractorTests
{
    [Test]
    [TestCase(125.27609985188199, 37.793341118739001, 508.7934997531367, 87.011098135435006)]
    [TestCase(0, 45, 300, 75)]
    [TestCase(0, 30, 300, 100)]
    [TestCase(0, 60, 300, 50)]
    public void ConvertAsSimpleProjection_returns_expected_values(double longitude, double latitude, double x, double y)
    {
        //Arrange

        //Act
        var res = PointExtractor.ConvertAsSimpleProjection(latitude, longitude, IconCategory.None);

        //Assert
        Assert.AreEqual(x, res.X, 0.000000000001);
        Assert.AreEqual(y, res.Y, 0.000000000001);
    }
}