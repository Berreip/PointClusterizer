using ClusterizerGui.Utils;
using ClusterizerGui.Views.ImportDatasets.Extraction;

namespace ClusterizerGui.UnitTests.View.ImportDatasets.Extraction;

[TestFixture]
internal sealed class CategoryMapperFactoryTests
{
    [Test]
    public void CreateMapping_without_default_categories()
    {
        //Arrange

        //Act
        var res = CategoryMapperFactory.Create(
            blueCategoryMapping: "POINT",
            yellowCategoryMapping: "LINESTRING",
            redCategoryMapping: "POLYGON",
            greenCategoryMapping: string.Empty);

        //Assert
        Assert.AreEqual(IconCategory.Blue, res.GetCategory("POINT"));
        Assert.AreEqual(IconCategory.Yellow, res.GetCategory("LINESTRING"));
        Assert.AreEqual(IconCategory.Red, res.GetCategory("POLYGON"));
        Assert.AreEqual(IconCategory.None, res.GetCategory(""));
        Assert.AreEqual(IconCategory.None, res.GetCategory("foo"));
    }

    [Test]
    public void CreateMapping_with_default_categories()
    {
        //Arrange

        //Act
        var res = CategoryMapperFactory.Create(
            blueCategoryMapping: "POINT",
            yellowCategoryMapping: ClusterizerGuiConstants.ALL_REMAINING_ALIAS_CATEGORY,
            redCategoryMapping: "POLYGON",
            greenCategoryMapping: string.Empty);

        //Assert
        Assert.AreEqual(IconCategory.Blue, res.GetCategory("POINT"));
        Assert.AreEqual(IconCategory.Red, res.GetCategory("POLYGON"));
        Assert.AreEqual(IconCategory.Yellow, res.GetCategory("LINESTRING"));
        Assert.AreEqual(IconCategory.Yellow, res.GetCategory(""));
        Assert.AreEqual(IconCategory.Yellow, res.GetCategory("foo"));
    }
}