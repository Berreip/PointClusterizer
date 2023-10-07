using ClusterizerGui.Views.MainDisplay.Helpers;

namespace ClusterizerGui.UnitTests.View.MainDisplay.Helpers;

public class RadiusCalculationTests
{
    [Test]
    [TestCase(1, 0)]
    [TestCase(10, 2)]
    [TestCase(100, 4)]
    [TestCase(1_000, 6)]
    [TestCase(10_000, 9)]
    [TestCase(100_000, 11)]
    public void RadiusCalculation_cases(int pointsCount, int expectedRadius)
    {
        //Arrange

        //Act
        var res = RadiusCalculation.ComputeRadiusFromPointCounts(pointsCount);

        //Assert
        Assert.AreEqual(expectedRadius, res);
    }

}