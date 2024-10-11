using Chirp.Infrastructure;
using Chirp.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Tests;

public class UnitTests
{
    private ICheepRepository cheepRepository;

    [Fact]
    // AuthorId 13 because Id reaches 12 in original Database, same for CheepId 
    public async void GetCheeps_ShouldReturnAllCheeps()
    {
        // Arrange
        using var connection = new SqliteConnection("Filename=:memory:");
        connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDbContext>().UseSqlite(connection);

        using var context = new ChirpDbContext(builder.Options);
        context.Database.EnsureCreatedAsync(); // Applies the schema to the database
        
        cheepRepository = new CheepRepository(context);

        context.Cheeps.ExecuteDelete();
        context.Authors.ExecuteDelete();
        
        Author author1 = new Author() {AuthorId = 13, Name = "John Doe", Email = "John+Doe@hotmail.com", Cheeps = new List<Cheep>() };
        var authors = new List<Author>() { author1 };
        
        var c1 = new Cheep() { CheepId = 701, AuthorId = author1.AuthorId, Author = author1, Text = "They were married in Chicago, with old Smith, and was expected aboard every day; meantime, the two went past me.", TimeStamp = DateTime.Parse("2023-08-01 13:14:37") };
        var c2 = new Cheep() { CheepId = 702, AuthorId = author1.AuthorId, Author = author1, Text = "And then, as he listened to all that''s left o'' twenty-one people.", TimeStamp = DateTime.Parse("2023-08-01 13:15:21") };
        var cheeps = new List<Cheep>() { c1, c2 };
        
        context.Authors.AddRange(authors);
        context.Cheeps.AddRange(cheeps);
        await context.SaveChangesAsync();
        
        // Act
        var cheepList = await cheepRepository.GetCheeps(null);
        var cheepAmount = cheepList.ToList().Count;
        
        // Assert
        Assert.Equal(2, cheepAmount);
    }

    [Fact]
    public async void GetTotalCheeps_ShouldCorrectAmount()
    {
        
    }
}