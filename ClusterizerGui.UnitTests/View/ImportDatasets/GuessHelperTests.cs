using ClusterizerGui.Views.ImportDatasets.Extraction;

namespace ClusterizerGui.UnitTests.View.ImportDatasets;

[TestFixture]
internal sealed class GuessHelperTests
{
    [Test]
    public void TryGuessPosition_nominal()
    {
        //Arrange
        var line = "name;long;cat;lat;other1;other2";

        //Act
        var res = GuessHelper.TryGuessPosition(
            line, 
            ";", 
            DatasetLoader.GUESS_COLUMN_POSITION, 
            DatasetLoader.GUESS_COLUMN_POSITION, 
            DatasetLoader.GUESS_COLUMN_POSITION, 
            DatasetLoader.GUESS_COLUMN_POSITION,
            out var guessedPosition);

        //Assert
        Assert.IsTrue(res);
        Assert.IsNotNull(guessedPosition);
        Assert.AreEqual(2, guessedPosition!.GuessedCategoryPosition);
        Assert.AreEqual(3, guessedPosition.GuessedLatitudePosition);
        Assert.AreEqual(1, guessedPosition.GuessedLongitudePosition);
        Assert.AreEqual(0, guessedPosition.GuessedNamePosition);
    }
}