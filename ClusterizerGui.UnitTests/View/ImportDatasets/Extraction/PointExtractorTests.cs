using ClusterizerGui.Utils;
using ClusterizerGui.Views.ImportDatasets.Extraction;

namespace ClusterizerGui.UnitTests.View.ImportDatasets.Extraction;

[TestFixture]
internal sealed class PointExtractorTests
{
    private const double DELTA = 0.000000000001;
    private const double DELTA_GEOGUESSR = 1;
    private const int MAP_WIDTH = ClusterizerGuiConstants.IMAGE_WIDTH;
    private const int MAP_HEIGHT = ClusterizerGuiConstants.IMAGE_HEIGHT;

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
        Assert.AreEqual(x, res.X, DELTA);
        Assert.AreEqual(y, res.Y, DELTA);
    }

    [Test]
    [TestCase(0, 0, MAP_WIDTH / 2, MAP_HEIGHT / 2)]
    [TestCase(-180, 0, 0, MAP_HEIGHT / 2)]
    [TestCase(0, -85, MAP_WIDTH / 2, MAP_HEIGHT)]
    [TestCase(180, 0, MAP_WIDTH, MAP_HEIGHT / 2)]
    [TestCase(0, 85, MAP_WIDTH / 2, 0)]
    public void ConvertAsMercator_Returns_Expected_Value(double inputLongitude, double inputLatitude,
        double expectedImageX, double expectedImageY)
    {
        //Arrange

        //Act
        var res = PointExtractor.ConvertAsMercator(inputLatitude, inputLongitude, IconCategory.None);

        //Assert
        Assert.AreEqual(expectedImageX, res.X, DELTA);
        Assert.AreEqual(expectedImageY, res.Y, 0.1);
    }

    [Test]
    [TestCase("Void island", 0, 0, MAP_WIDTH / 2, MAP_HEIGHT / 2)]
    [TestCase("Min Long", -180, 0, 0, MAP_HEIGHT / 2)]
    [TestCase("Min Lat", 0, -90, MAP_WIDTH / 2, MAP_HEIGHT)]
    [TestCase("Max Long", 180, 0, MAP_WIDTH, MAP_HEIGHT / 2)]
    [TestCase("Max Lat", 0, 90, MAP_WIDTH / 2, 0)]
    [TestCase("Brest", -4.485220906190697, 48.39223109744845, 292, 90)]
    [TestCase("Paris", 2.355260214639132, 48.85454190162207, 303, 90)]
    [TestCase("Lisbonne", -9.142403308261855, 38.72138403605182, 285, 104)]
    [TestCase("Chicago", -87.62720816918997, 41.877991828130725, 153, 99)]
    [TestCase("San Francisco", -122.41456502351738, 37.77582841039767, 95, 104)]
    [TestCase("Ushuaïa", -68.30351620094814, -54.799912906657894, 186, 220)]
    [TestCase("Le Cap", 18.42649225898852, -33.92732806455349, 331, 190)]
    [TestCase("Sydney", 151.2210638695313, -33.87024933348673, 552, 190)]
    [TestCase("Tokyo", 139.76817548923103, 35.68334815548932, 533, 107)]
    [TestCase("Anchorage", -149.89491036000098, 61.217583623307625, 50, 70)]
    public void ConvertAsMillerCylindrical_Returns_Expected_Value(string positionName, double inputLongitude,
        double inputLatitude, double expectedImageX, double expectedImageY)
    {
        //Arrange

        //Act
        var res = PointExtractor.ConvertAsMillerCylindrical(inputLatitude, inputLongitude, IconCategory.None);

        //Assert
        Assert.AreEqual(expectedImageX, res.X, DELTA_GEOGUESSR,
            $"Wrong longitude calculation for position {positionName}, relative error is: {(expectedImageX - res.X) / MAP_HEIGHT:P0}");
        Assert.AreEqual(expectedImageY, res.Y, DELTA_GEOGUESSR,
            $"Wrong latitude calculation for position {positionName}, relative error is: {(expectedImageY - res.Y) / MAP_HEIGHT:P0}");
    }

    [Test]
    public void ConvertLongitudeAsMillerCylindrical()
    {
        var result = new Dictionary<int, double>();

        for (var latitude = -90; latitude <= 90; latitude += 5)
        {

            // convert from degrees to radians
            var latRad = ((double)latitude).ToRadian();

            // get y value
            var millerLat = 1.25d * Math.Log(Math.Tan(Math.PI / 4d + 0.4d * latRad));

            result.Add(latitude, millerLat);
        }
        
        Assert.IsNotNull(result);
    }
    
    [Test]
    public void ConvertLongitudeAsMercator()
    {
        var result = new Dictionary<int, double>();

        for (var latitude = -85; latitude <= 85; latitude += 5)
        {

            // convert from degrees to radians
            var latRad = ((double)latitude).ToRadian();

            // get y value
            var mercatorN = Math.Log(Math.Tan(Math.PI / 4d + latRad / 2d));

            result.Add(latitude, mercatorN);
        }
        
        Assert.IsNotNull(result);
    }
    


}