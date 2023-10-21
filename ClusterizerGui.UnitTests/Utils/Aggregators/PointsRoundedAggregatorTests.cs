using System.Drawing;
using ClusterizerGui.Utils.Aggregators;
using ClusterizerGui.Views.ImportDatasets.Extraction;
using ClusterizerGui.Views.MainDisplay.Adapters;

namespace ClusterizerGui.UnitTests.Utils.Aggregators;

[TestFixture]
internal sealed class PointsRoundedAggregatorTests
{
    [Test]
    public void AggregateSimilarPoints_returns_empty_when_provided_empty()
    {
        //Arrange
        var points = Array.Empty<PointWrapper>();

        //Act
        var res = PointsRoundedAggregator.AggregateSimilarPoints(points, 10, 20, 5);

        //Assert
        Assert.IsEmpty(res);
    }

    [Test]
    public void AggregateSimilarPoints_aggregate_rounded_point_case1()
    {
        //Arrange
        var points = new[]
        {
            new PointWrapper(1.1, 2.2, 3.3, IconCategory.None),
            new PointWrapper(1.1, 2.2, 3.3, IconCategory.None),
            new PointWrapper(0.9, 1.9, 2.9, IconCategory.None),
        };

        //Act
        var res = PointsRoundedAggregator.AggregateSimilarPoints(points, 10, 20, 5);

        //Assert
        Assert.AreEqual(1, res.Count);
        Assert.AreEqual(new PointRounded(1, 2, 3, Color.Aqua), res.Single());
    }

    [Test]
    public void AggregateSimilarPoints_aggregate_rounded_point_case2()
    {
        //Arrange
        var points = new[]
        {
            new PointWrapper(1.1, 2.2, 3.3, IconCategory.None),
            new PointWrapper(1.1, 2.2, 4.9, IconCategory.None),
            new PointWrapper(0.9, 1.9, 2.9, IconCategory.None),
        };

        //Act
        var res = PointsRoundedAggregator.AggregateSimilarPoints(points, 10, 20, 5);

        //Assert
        Assert.AreEqual(2, res.Count);
        Assert.IsTrue(res.Contains(new PointRounded(1, 2, 3, Color.Azure)));
        Assert.IsTrue(res.Contains(new PointRounded(1, 2, 5, Color.Aquamarine)));
    }

    [Test]
    public void AggregateSimilarPoints_cap_points()
    {
        //Arrange
        var points = new[]
        {
            new PointWrapper(100, 100, 100, IconCategory.None),
            new PointWrapper(-1, -1, -1, IconCategory.None),
        };

        //Act
        var res = PointsRoundedAggregator.AggregateSimilarPoints(points, 10, 20, 5);

        //Assert
        Assert.AreEqual(2, res.Count);
        Assert.IsTrue(res.Contains(new PointRounded(10, 20, 5, Color.Azure)));
        Assert.IsTrue(res.Contains(new PointRounded(0, 0, 0, Color.Aquamarine)));
    }

    [Test]
    public void CreatePointArrayFromAggregatedData_nominal_case()
    {
        //Arrange
        var points = new[]
        {
            new PointRounded(1, 2, 99, Color.Azure),
            new PointRounded(3, 3, 56, Color.Aquamarine),
        };

        //Act
        var res = PointsRoundedAggregator.CreatePointArrayFromAggregatedData(points, 10, 20);

        //Assert
        var point1Found = false;
        var point2Found = false;
        for (var i = 0; i < 10; i++)
        {
            for (var j = 0; j < 20; j++)
            {
                if (i == 1 && j == 2)
                {
                    var altitudePoint = res.GetPointAltitude(i, j).Altitude;
                    Assert.AreNotEqual(PointsRoundedAggregator.ALTITUDE_NO_VALUE, altitudePoint);
                    Assert.AreEqual(99, altitudePoint);
                    point1Found = true;
                }
                else if (i == 3 && j == 3)
                {
                    var altitudePoint = res.GetPointAltitude(i, j).Altitude;
                    Assert.AreNotEqual(PointsRoundedAggregator.ALTITUDE_NO_VALUE, altitudePoint);
                    Assert.AreEqual(56, altitudePoint);
                    point2Found = true;
                }
                else
                {
                    Assert.AreEqual(PointsRoundedAggregator.ALTITUDE_NO_VALUE, res.GetPointAltitude(i, j).Altitude);
                }
            }
        }

        Assert.IsTrue(point1Found);
        Assert.IsTrue(point2Found);
    }
}