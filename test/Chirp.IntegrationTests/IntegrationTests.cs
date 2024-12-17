using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;  
using Xunit;
using Chirp.Core;
using Chirp.Infrastructure;
using Chirp.Web;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Tests
{
    public class TestAPI : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _fixture;
        private readonly HttpClient _client;
        
        private ICheepRepository cheepRepository;
        private IAuthorRepository authorRepository;

        public TestAPI(WebApplicationFactory<Program> fixture)
        {
            _fixture = fixture;
            _client = _fixture.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = true, HandleCookies = true });
        }

        [Fact]
        public async void CanSeePublicTimeline()
        {
            
            var response = await _client.GetAsync("/");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            Assert.Contains("Chirp!", content);
            Assert.Contains("Public Timeline", content);
        }

        [Theory]
        [InlineData("Helge")]
        [InlineData("Adrian")]
        public async void CanSeeMyTimeline(string author)
        {
            var response = await _client.GetAsync($"/{author}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();


            Assert.Contains("Chirp!", content);
            Assert.Contains($"{author}", content);
        }

        /// <summary>
        /// This should test that when a author deletes a cheep in a list the cheepID is no longer in use. 
        /// </summary>
        [Fact]
        public async void deletedCheepRemovesID() 
            // This used to be in Unit test but as it is an integration test it should be here 
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


            // Act 
            await cheepRepository.FindCheepID(cheepDto2); 
            var checkID2 = cheepRepository.FindCheepID(cheepDto2); 
            await cheepRepository.DeleteCheep(cheepDto2);
            await cheepRepository.CreateCheep(cheepDto3, "John Doe");

            await context.SaveChangesAsync();
    
            // after deletion, the new cheep should have a diffrent ID 
            var cheepId3 = cheepRepository.FindCheepID(cheepDto3);
            await context.SaveChangesAsync();
    
            // Assert 
            Assert.False(checkID2 == cheepId3);
        }
   
    }
    
}