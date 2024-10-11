using Chirp.Infrastructure;

namespace Chirp.Tests;

public class UnitTests
{
    [Fact]
    public void UnixTimeStampToDateTimeTest()
    {
        CheepService cheepService = new CheepService();

        // Arrange
        double unixTimeStamp = 1633046400; // 10/01/21 0:00:00 in Unix Time Stamp
        string expectedDateTimeString = "10/01/21 0:00:00";

        // Act
        string actualDateTimeString = cheepService.UnixTimeStampToDateTimeString(unixTimeStamp);

        // Assert
        Assert.Equal(expectedDateTimeString, actualDateTimeString);
    }
}