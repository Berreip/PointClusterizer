using ClusterizerGui.Views.ImportDatasets.Extraction;

namespace ClusterizerGui.UnitTests.View.ImportDatasets;

[TestFixture]
internal sealed class GuessedPositionTests
{
    [Test]
    public void GuessPosition_nominal()
    {
        //Arrange
        const string line = "name;long;cat;lat;other1;other2";
        var sut = new GuessedPosition(
            guessedNamePosition: 0,
            guessedLongitudePosition: 1,
            guessedLatitudePosition: 3,
            guessedCategoryPosition: 2,
            miscHeaders: null);
        var lineSplit = line.Split(";");
        
        //Act
        var res = sut.ExtractName(lineSplit);
        var longitude = sut.ExtractLong(lineSplit);
        var latitude = sut.ExtractLat(lineSplit);
        var category = sut.ExtractCat(lineSplit);
        var misc = GuessHelper.ExtractOtherData(lineSplit, sut.GuessedCategoryPosition, sut.GuessedNamePosition, sut.GuessedLatitudePosition, sut.GuessedLongitudePosition);

        //Assert
        Assert.AreEqual("name", res);
        Assert.AreEqual("long", longitude);
        Assert.AreEqual("lat", latitude);
        Assert.AreEqual("cat", category);
        Assert.AreEqual("other1|other2", misc);
    }

    
}