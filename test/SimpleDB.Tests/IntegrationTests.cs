using Chirp.CLI;

namespace SimpleDB.Tests;

public class IntegrationTests
{
    public record Cheep(string Author, string Message, long Timestamp);

    [Fact]
    public void CSVDatabaseStoreAndReadTest()
    {
        // Arrange
        CSVDatabase<Cheep> csv = CSVDatabase<Cheep>.Instance;
        csv.path = "../../../../../data/chirp_cli_db.csv";
        
        Cheep cheep = new Cheep("John Doe", "I am John Doeeee", DateTimeOffset.Now.ToUnixTimeSeconds());

        var amount = csv.Read().Count();

        // Act
        csv.Store(cheep);

        // Assert
        Assert.Equal(amount + 1, csv.Read().Count());
    }
}