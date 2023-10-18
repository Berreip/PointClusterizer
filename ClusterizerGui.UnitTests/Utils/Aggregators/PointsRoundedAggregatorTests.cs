using ClusterizerGui.Utils.Aggregators;
using ClusterizerGui.Views.MainDisplay.Adapters;
using ClusterizerLib;

namespace ClusterizerGui.UnitTests.Utils.Aggregators;

[TestFixture]
internal sealed class PointsRoundedAggregatorTests
{
    [Test]
    public void AggregateSimilarPoints_returns_empty_when_provided_empty()
    {
        //Arrange
        var points = Array.Empty<IPoint>();

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
            new PointWrapper(1.1, 2.2, 3.3),
            new PointWrapper(1.1, 2.2, 3.3),
            new PointWrapper(0.9, 1.9, 2.9),
        };

        //Act
        var res = PointsRoundedAggregator.AggregateSimilarPoints(points, 10, 20, 5);

        //Assert
        Assert.AreEqual(1, res.Count);
        Assert.AreEqual(new PointRounded(1, 2, 3), res.Single());
    }

    [Test]
    public void AggregateSimilarPoints_aggregate_rounded_point_case2()
    {
        //Arrange
        var points = new[]
        {
            new PointWrapper(1.1, 2.2, 3.3),
            new PointWrapper(1.1, 2.2, 4.9),
            new PointWrapper(0.9, 1.9, 2.9),
        };

        //Act
        var res = PointsRoundedAggregator.AggregateSimilarPoints(points, 10, 20, 5);

        //Assert
        Assert.AreEqual(2, res.Count);
        Assert.IsTrue(res.Contains(new PointRounded(1, 2, 3)));
        Assert.IsTrue(res.Contains(new PointRounded(1, 2, 5)));
    }

    [Test]
    public void AggregateSimilarPoints_cap_points()
    {
        //Arrange
        var points = new[]
        {
            new PointWrapper(100, 100, 100),
            new PointWrapper(-1, -1, -1),
        };

        //Act
        var res = PointsRoundedAggregator.AggregateSimilarPoints(points, 10, 20, 5);

        //Assert
        Assert.AreEqual(2, res.Count);
        Assert.IsTrue(res.Contains(new PointRounded(10, 20, 5)));
        Assert.IsTrue(res.Contains(new PointRounded(0, 0, 0)));
    }
    
    [Test]
    public void CreatePointArrayFromAggregatedData_nominal_case()
    {
        //Arrange
        var points = new[]
        {
            new PointRounded(1, 2, 99),
            new PointRounded(3, 3, 56),
        };

        //Act
        var res = PointsRoundedAggregator.CreatePointArrayFromAggregatedData(points, 10, 20);

        //Assert
        var point1Found = false;
        var point2Found = false;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                if (i == 1 && j == 2)
                {
                    Assert.IsTrue(res.TryGetPointAltitude(i, j, out var altitudePoint));
                    Assert.AreEqual(99, altitudePoint);
                    point1Found = true;
                }
                else if (i == 3 && j == 3)
                {
                        Assert.IsTrue(res.TryGetPointAltitude(i, j, out var altitudePoint));
                        Assert.AreEqual(56, altitudePoint);
                        point2Found = true;
                }
                else
                {
                    Assert.IsFalse(res.TryGetPointAltitude(i, j, out _));
                }
            }
        }

        Assert.IsTrue(point1Found);
        Assert.IsTrue(point2Found);
    }
}