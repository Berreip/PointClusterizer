namespace ClusteringGridBase.UnitTests;

[TestFixture]
internal sealed class CubeTests
{
    [Test]
    [TestCase(0d, 10d, 10d)]
    [TestCase(5d, 10d, 10d)]
    [TestCase(-5d, 10.6d, 10.6d)]
    public void Cube_XSize_valid(double x, double xLenght, double expected)
    {
        //Arrange

        //Act
        var sut = new Cube(new PointUnitTest(x, 0, 0), xLenght, 10, 10);

        //Assert
        Assert.AreEqual(expected, sut.XSize);
    }

    [Test]
    [TestCase(0d, 10d, 10d)]
    [TestCase(5d, 10d, 10d)]
    [TestCase(-5d, 10.6d, 10.6d)]
    public void Cube_YSize_valid(double y, double yLenght, double expected)
    {
        //Arrange

        //Act
        var sut = new Cube(new PointUnitTest(0, y, 0), 10, yLenght, 10);

        //Assert
        Assert.AreEqual(expected, sut.YSize);
    }

    [Test]
    [TestCase(0d, 10d, 10d)]
    [TestCase(5d, 10d, 10d)]
    [TestCase(-5d, 10.6d, 10.6d)]
    public void Cube_ZSize_valid(double z, double zLenght, double expected)
    {
        //Arrange

        //Act
        var sut = new Cube(new PointUnitTest(0, 0, z), 10, 10, zLenght);

        //Assert
        Assert.AreEqual(expected, sut.ZSize);
    }

    [Test]
    [TestCase(0d, 10d, 10d, false)]
    [TestCase(5d, 10d, 5d, true)]
    [TestCase(-5d, 6d, 10d, false)]
    public void Cube_Contains_valid(double x, double y, double z, bool expectedContains)
    {
        //Arrange
        // do not use a zero based cube (1, 2, 3)
        var sut = new Cube(new PointUnitTest(1, 2, 3), 10, 11, 12);

        //Act
        var res = sut.Contains(x, y, z);

        //Assert
        Assert.AreEqual(expectedContains, res);
    }

    [Test]
    public void Cube_Contains_Contains_validation()
    {
        //Arrange
        var origin = new PointUnitTest(
            x: 50.187194460196928,
            y: 33.996499951342798,
            z: 25.804881899325871);
        var xLenght = 2000d;
        var yLenght = 1500d;
        var zLenght = 1200d;
        // do not use a zero based cube (1, 2, 3)
        var sut = new Cube(origin, xLenght, yLenght, zLenght);

        var point = new PointUnitTest(
            x: 1029.3810417205302,
            y: 1470.6445954636843,
            z: 588.1512710630974);

        //Act
        var res = sut.Contains(point.X, point.Y, point.Z);

        //Assert
        Assert.IsTrue(res);
    }
}