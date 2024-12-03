using Chirp.Infrastructure;
using Chirp.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Tests;

public class UnitTests
{
    private ICheepRepository cheepRepository;
    private IAuthorRepository authorRepository;

    [Fact]
    // AuthorId 13 because Id reaches 12 in original Database, same for CheepId 
    public async void GetCheeps_ShouldReturnAllCheeps()
    {
        // Arrange
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDbContext>().UseSqlite(connection);

        using var context = new ChirpDbContext(builder.Options);
        await context.Database.EnsureCreatedAsync(); // Applies the schema to the database

        cheepRepository = new CheepRepository(context);

        context.Cheeps.ExecuteDelete();
        context.Authors.ExecuteDelete();

        Author author1 = new Author() { AuthorId = 13, Name = "John Doe", Email = "John+Doe@hotmail.com", Cheeps = new List<Cheep>() };
        var authors = new List<Author>() { author1 };

        var c1 = new Cheep() { CheepId = 701, AuthorId = author1.AuthorId, Author = author1, Text = "They were married in Chicago, with old Smith, and was expected aboard every day; meantime, the two went past me.", TimeStamp = DateTime.Parse("2023-08-01 13:14:37") };
        var c2 = new Cheep() { CheepId = 702, AuthorId = author1.AuthorId, Author = author1, Text = "And then, as he listened to all that''s left o'' twenty-one people.", TimeStamp = DateTime.Parse("2023-08-01 13:15:21") };
        var cheeps = new List<Cheep>() { c1, c2 };

        context.Authors.AddRange(authors);
        context.Cheeps.AddRange(cheeps);
        await context.SaveChangesAsync();

        // Act
        var cheepList = await cheepRepository.GetCheeps(null, 1);
        var cheepAmount = cheepList.ToList().Count;

        // Assert
        Assert.Equal(2, cheepAmount);
    }

    [Fact]
    public async void GetTotalCheeps_ShouldCorrectAmount() // this should return 
    {
        // Arrange
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDbContext>().UseSqlite(connection);

        using var context = new ChirpDbContext(builder.Options);
        await context.Database.EnsureCreatedAsync(); // Applies the schema to the database

        cheepRepository = new CheepRepository(context);

        context.Cheeps.ExecuteDelete();
        context.Authors.ExecuteDelete();

        Author author1 = new Author() { AuthorId = 13, Name = "John Doe", Email = "John+Doe@hotmail.com", Cheeps = new List<Cheep>() };
        var authors = new List<Author>() { author1 };

        var c1 = new Cheep() { CheepId = 701, AuthorId = author1.AuthorId, Author = author1, Text = "They were married in Chicago, with old Smith, and was expected aboard every day; meantime, the two went past me.", TimeStamp = DateTime.Parse("2023-08-01 13:14:37") };
        var c2 = new Cheep() { CheepId = 702, AuthorId = author1.AuthorId, Author = author1, Text = "And then, as he listened to all that''s left o'' twenty-one people.", TimeStamp = DateTime.Parse("2023-08-01 13:15:21") };
        var c3 = new Cheep() { CheepId = 703, AuthorId = author1.AuthorId, Author = author1, Text = "Test string ", TimeStamp = DateTime.Parse("2023-08-01 13:15:22") };

        var cheeps = new List<Cheep>() { c1, c2, c3 };

        context.Authors.AddRange(authors);
        context.Cheeps.AddRange(cheeps);
        await context.SaveChangesAsync();

        // Act 
        var cheepcount = cheepRepository.GetTotalCheepsCount(null);
        // Assert 
        Assert.Equal(3, cheepcount);
    }

    [Fact]
    public async void GetAuthorByName_ShouldReturnAuthor()
    {
        // Arrange 
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDbContext>().UseSqlite(connection);

        using var context = new ChirpDbContext(builder.Options);
        await context.Database.EnsureCreatedAsync(); // Applies the schema to the database
        
        authorRepository = new AuthorRepository(context);

        context.Cheeps.ExecuteDelete();
        context.Authors.ExecuteDelete();

        Author author1 = new Author() { AuthorId = 13, Name = "John Doe", Email = "John+Doe@hotmail.com", Cheeps = new List<Cheep>() };
        Author author2 = new Author() { AuthorId = 14, Name = "Jane Dig", Email = "Jane+Dig@hotmail.com", Cheeps = new List<Cheep>() };
        Author author3 = new Author() { AuthorId = 15, Name = " Amalia testPerson", Email = "Amalia+tpe@hotmail.com", Cheeps = new List<Cheep>() };
        var authors = new List<Author>() { author1, author2, author3 };

        var c1 = new Cheep() { CheepId = 701, AuthorId = author1.AuthorId, Author = author1, Text = "They were married in Chicago, with old Smith, and was expected aboard every day; meantime, the two went past me.", TimeStamp = DateTime.Parse("2023-08-01 13:14:37") };
        var c2 = new Cheep() { CheepId = 702, AuthorId = author1.AuthorId, Author = author1, Text = "And then, as he listened to all that''s left o'' twenty-one people.", TimeStamp = DateTime.Parse("2023-08-01 13:15:21") };
        var c3 = new Cheep() { CheepId = 703, AuthorId = author1.AuthorId, Author = author1, Text = "Test string ", TimeStamp = DateTime.Parse("2023-08-01 13:15:22") };

        var c4 = new Cheep() { CheepId = 704, AuthorId = author2.AuthorId, Author = author2, Text = "What does the fox say? .", TimeStamp = DateTime.Parse("2023-08-01 13:14:37") };
        var c5 = new Cheep() { CheepId = 705, AuthorId = author2.AuthorId, Author = author2, Text = "*fox sounds** ", TimeStamp = DateTime.Parse("2023-08-01 13:14:40") };

        var c6 = new Cheep() { CheepId = 706, AuthorId = author3.AuthorId, Author = author3, Text = "Test string TP", TimeStamp = DateTime.Parse("2023-08-02 13:16:22") };

        var cheeps = new List<Cheep>() { c1, c2, c3, c4, c5, c6 };

        context.Authors.AddRange(authors);
        context.Cheeps.AddRange(cheeps);
        await context.SaveChangesAsync();

        // Act 
        var findAuthorByName = authorRepository.FindAuthorByName("Jane Dig");
        var cheepByauthorList = new List<Cheep>();
        foreach (var cheep in cheeps)
        {
            // Not sure if .Result is the right comand however when I ran it expecting 3 like if I was seaching for John doe it faild therefore I think it works as intended  :) 
            // also gave correct results when ajusting for john doe 
            if (cheep.Author == findAuthorByName.Result)
            {
                cheepByauthorList.Add(cheep);
            }
        }
        var cheepAmount = cheepByauthorList.Count;

        // Assert 
        Assert.Equal(2, cheepAmount);
    }

    [Fact]
    public async void CreateCheep_ShouldCreateCheep()
    {
        //Arrange
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDbContext>().UseSqlite(connection);

        using var context = new ChirpDbContext(builder.Options);
        await context.Database.EnsureCreatedAsync(); // Applies the schema to the database

        cheepRepository = new CheepRepository(context);

        context.Cheeps.ExecuteDelete();
        context.Authors.ExecuteDelete();

        Author author1 = new Author() { AuthorId = 13, Name = "John Doe", Email = "John+Doe@hotmail.com", Cheeps = new List<Cheep>() };
        Author author2 = new Author() { AuthorId = 14, Name = "Jane Dig", Email = "Jane+Dig@hotmail.com", Cheeps = new List<Cheep>() };
        var authors = new List<Author>() { author1, author2 };

        var c1 = new Cheep() { CheepId = 701, AuthorId = author1.AuthorId, Author = author1, Text = "They were married in Chicago, with old Smith, and was expected aboard every day; meantime, the two went past me.", TimeStamp = DateTime.Parse("2023-08-01 13:14:37") };
        var c2 = new Cheep() { CheepId = 702, AuthorId = author1.AuthorId, Author = author1, Text = "And then, as he listened to all that''s left o'' twenty-one people.", TimeStamp = DateTime.Parse("2023-08-01 13:15:21") };
        var c3 = new Cheep() { CheepId = 703, AuthorId = author1.AuthorId, Author = author1, Text = "Test string ", TimeStamp = DateTime.Parse("2023-08-01 13:15:22") };

        var c4 = new Cheep() { CheepId = 704, AuthorId = author2.AuthorId, Author = author2, Text = "What does the fox say? .", TimeStamp = DateTime.Parse("2023-08-01 13:14:37") };
        var c5 = new Cheep() { CheepId = 705, AuthorId = author2.AuthorId, Author = author2, Text = "*fox sounds** ", TimeStamp = DateTime.Parse("2023-08-01 13:14:40") };

        var cheepsList = new List<Cheep>() { c1, c2, c3, c4, c5 };

        context.Authors.AddRange(authors);
        context.Cheeps.AddRange(cheepsList);
        await context.SaveChangesAsync();

        Author author3 = new Author() { AuthorId = 15, Name = " Amalia testPerson", Email = "Amalia+tpe@hotmail.com", Cheeps = new List<Cheep>() };

        var addedCheep = new Cheep() { CheepId = 706, AuthorId = author3.AuthorId, Author = author3, Text = "Test string TP", TimeStamp = DateTime.Parse("2023-08-02 13:16:22") };

        // Act 
        var newCheep = cheepRepository.CreateCheep(addedCheep);
        await context.SaveChangesAsync();

        var cheepAmount = await context.Cheeps.CountAsync();


        // Assert 
        Assert.Equal(6, cheepAmount);
    }

    [Fact]
    public async void CreateAuthor_ShouldCreateAuthor()
    {
        // Arrange 
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDbContext>().UseSqlite(connection);

        using var context = new ChirpDbContext(builder.Options);
        await context.Database.EnsureCreatedAsync(); // Applies the schema to the database
        
        authorRepository = new AuthorRepository(context);

        context.Cheeps.ExecuteDelete();
        context.Authors.ExecuteDelete();

        AuthorDto author = new AuthorDto( "John Doe", "John+Doe@hotmail.com");
        
        // Act
        var newAuthor = authorRepository.CreateAuthor(author);
        await context.SaveChangesAsync();
        
        var authorAmount = await context.Authors.CountAsync();

        // Assert
        Assert.Equal(1, authorAmount);
    }      
    
    [Fact]
    public async void deleteAuthor_ShouldDeleteAuthor()
    {
        // Arrange 
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDbContext>().UseSqlite(connection);

        using var context = new ChirpDbContext(builder.Options);
        await context.Database.EnsureCreatedAsync(); // Applies the schema to the database

        authorRepository = new AuthorRepository(context);

        context.Cheeps.ExecuteDelete();
        context.Authors.ExecuteDelete();

        AuthorDto author = new AuthorDto( "John Doe", "John+Doe@hotmail.com");

        var newAuthor = authorRepository.CreateAuthor(author);
        await context.SaveChangesAsync();

        //Act
        authorRepository.DeleteAuthor("John Doe");
        var authorAmount = await context.Authors.CountAsync();

        // Assert 
        Assert.Equal(0, authorAmount);
    }

    [Fact]
    public async void deleteCheep_ShouldDeleteCheep()
    {
        // Arrange 
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<ChirpDbContext>().UseSqlite(connection);

        using var context = new ChirpDbContext(builder.Options);
        await context.Database.EnsureCreatedAsync(); // Applies the schema to the database

        authorRepository = new AuthorRepository(context);
        cheepRepository = new CheepRepository(context);

        context.Cheeps.ExecuteDelete();
        context.Authors.ExecuteDelete();

        AuthorDto author = new AuthorDto( "John Doe", "John+Doe@hotmail.com");
        var newAuthor = authorRepository.CreateAuthor(author);

        CheepDto cheepDto1 = new CheepDto("test cheep", "2023-08-01 13:15:22","John Doe");
        CheepDto cheepDto2 = new CheepDto("test cheep2", "2023-08-01 13:15:24","John Doe");
        CheepDto cheepDto3 = new CheepDto("test cheep3", "2023-08-01 13:15:27","John Doe");
        
        await cheepRepository.CreateCheep(cheepDto1, "John Doe");
        await cheepRepository.CreateCheep(cheepDto2, "John Doe");
        await cheepRepository.CreateCheep(cheepDto3, "John Doe");


        // Act 
        cheepRepository.DeleteCheep(cheepDto2);
        var newCheep = await context.Cheeps.CountAsync();
        
        await context.SaveChangesAsync();
        
        // Assert 
        Assert.Equal(2, context.Cheeps.Count());
    }
}