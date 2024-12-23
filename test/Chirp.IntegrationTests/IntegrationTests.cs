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
    }
}