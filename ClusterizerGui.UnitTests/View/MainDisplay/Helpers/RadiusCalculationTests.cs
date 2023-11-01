using System.Drawing;
using ClusterizerGui.Views.MainDisplay.Helpers;

namespace ClusterizerGui.UnitTests.View.MainDisplay.Helpers;

public class RadiusCalculationTests
{
    [Test]
    [TestCase(1, 4)]
    [TestCase(10, 12)]
    [TestCase(100, 24)]
    [TestCase(1_000, 36)]
    [TestCase(10_000, 52)]
    [TestCase(100_000, 64)]
    public void RadiusCalculation_cases(int pointsCount, int expectedRadius)
    {
        //Arrange

        //Act
        var res = RadiusCalculation.Radius4Log2(pointsCount, new Rectangle(0, 0, 10, 10));

        //Assert
        Assert.AreEqual(expectedRadius, res);
    }

}