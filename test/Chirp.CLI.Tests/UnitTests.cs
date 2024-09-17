
namespace Chirp.CLI.Tests;

public class UnitTests
{
    [Fact]
    public void UnixTimeStampToDateTimeTest()
    {
        // Arrange
        CheepManager cheepManager = new CheepManager();
        DateTimeOffset now = DateTimeOffset.Now;
        
        long expectedTimestamp = now.ToUnixTimeSeconds();
        
        // Act
        long actualTimestamp = cheepManager.getTimestampUNIX(now);

        // Assert
        Assert.Equal(expectedTimestamp, actualTimestamp);
    }
}