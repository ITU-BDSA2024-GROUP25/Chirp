
namespace Chirp.CLI.Tests;

public class UnitTest
{
    [Fact]
    public void GetUnixTimestamp_ShouldReturnCorrectTimestamp()
    {
        // Arrange
        var cheepManager = new CheepManager();
        var currentTime = DateTimeOffset.UtcNow;
        
        var expectedTimestamp = currentTime.ToUnixTimeSeconds();

        // Act
        var actualTimestamp = cheepManager.get_UNIX_Timestamp(currentTime);

        // Assert
        Assert.Equal(expectedTimestamp, actualTimestamp);
    }
}