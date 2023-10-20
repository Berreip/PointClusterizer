using ClusterizerGui.Views.ImportDatasets.Extraction;

namespace ClusterizerGui.UnitTests.View.ImportDatasets.Extraction;

[TestFixture]
internal sealed class PointExtractorTests
{
    [Test]
    [TestCase(37.793341118739001, 125.27609985188199, 508.7934997531367, 87.011098135435006)]
    public void ConvertAsSimpleProjection_returns_expected_values(double latitude, double longitude, double x, double y)
    {
        //Arrange

        //Act
        var res = PointExtractor.ConvertAsSimpleProjection(latitude, longitude);

        //Assert
        Assert.AreEqual(x, res.X, 0.000000000001);
        Assert.AreEqual(y, res.Y, 0.000000000001);
    }

    
}