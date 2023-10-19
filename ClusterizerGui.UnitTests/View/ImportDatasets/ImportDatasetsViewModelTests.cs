using ClusterizerGui.Services;
using ClusterizerGui.Views.ImportDatasets;
using ClusterizerGui.Views.ImportDatasets.Extraction;
using Moq;

namespace ClusterizerGui.UnitTests.View.ImportDatasets;

[TestFixture]
internal sealed class ImportDatasetsViewModelTests
{
    private ImportDatasetsViewModel _sut = null!;
    private Mock<IDatasetManager> _datasetManager = null!;

    [SetUp]
    public void TestInitialize()
    {
        // mock:
        _datasetManager = new Mock<IDatasetManager>();
        _datasetManager.Setup(o => o.GetAllDatasets()).Returns(Array.Empty<IDataset>());

        
        // software under test:
        _sut = new ImportDatasetsViewModel(_datasetManager.Object);
    }

    [Test]
    public void ctor_setup_initial_expected_values()
    {
        //Arrange

        //Act

        //Assert
        Assert.AreEqual(DatasetLoader.GUESS_COLUMN_POSITION, _sut.CategoryHeaderPosition);
        Assert.AreEqual(DatasetLoader.GUESS_COLUMN_POSITION, _sut.LatitudeHeaderPosition);
        Assert.AreEqual(DatasetLoader.GUESS_COLUMN_POSITION, _sut.LongitudeHeaderPosition);
        Assert.AreEqual(DatasetLoader.GUESS_COLUMN_POSITION, _sut.NameHeaderPosition);
        Assert.AreEqual(1, _sut.DataStartingPosition);
    }
}