using Chirp.CLI;
using System.Net;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace SimpleDB.Tests;

public class IntegrationTests
{
    public record Cheep(string Author, string Message, long Timestamp);
    
        [Fact]
        public async Task GetCheeps_ReturnOK()
        {
            // Arrange
            using var client = new HttpClient { BaseAddress = new Uri("https://bdsagroup25chirpremotedb1.azurewebsites.net/") };
    
            // Act
            var response = await client.GetAsync("/cheeps");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PostCheeps_ReturnOK()
        {
            // Arrange
            using var client = new HttpClient { BaseAddress = new Uri("https://bdsagroup25chirpremotedb1.azurewebsites.net/") };
            Cheep cheep = new Cheep("ImATester", "Hej", 123456);
            
            // Act
            var response = await client.PostAsJsonAsync("cheep", cheep);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Should cleanup afterwards here
        }
}