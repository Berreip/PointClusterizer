using ClusterizerGui.Utils.Aggregators;

// ReSharper disable UseObjectOrCollectionInitializer

namespace ClusterizerGui.UnitTests.Utils.Aggregators;

[TestFixture]
internal sealed class PointRoundedTests
{
    [Test]
    public void Check_That_Equals_point_are_filtered_in_a_hashset()
    {
        //Arrange
        var hash = new HashSet<PointRounded>();

        //Act
        hash.Add(new PointRounded(1, 2, 3));
        hash.Add(new PointRounded(1, 2, 3));

        //Assert
        Assert.AreEqual(1, hash.Count);
    }

    [Test]
    public void Check_That_Not_Equals_point_are_filtered_in_a_hashset_for_X_not_equals()
    {
        //Arrange
        var hash = new HashSet<PointRounded>();

        //Act
        hash.Add(new PointRounded(7, 2, 3));
        hash.Add(new PointRounded(1, 2, 3));

        //Assert
        Assert.AreEqual(2, hash.Count);
    }

    [Test]
    public void Check_That_Not_Equals_point_are_filtered_in_a_hashset_for_Y_not_equals()
    {
        //Arrange
        var hash = new HashSet<PointRounded>();

        //Act
        hash.Add(new PointRounded(1, 7, 3));
        hash.Add(new PointRounded(1, 2, 3));

        //Assert
        Assert.AreEqual(2, hash.Count);
    }

    [Test]
    public void Check_That_Not_Equals_point_are_filtered_in_a_hashset_for_Z_not_equals()
    {
        //Arrange
        var hash = new HashSet<PointRounded>();

        //Act
        hash.Add(new PointRounded(1, 2, 7));
        hash.Add(new PointRounded(1, 2, 3));

        //Assert
        Assert.AreEqual(2, hash.Count);
    }
}