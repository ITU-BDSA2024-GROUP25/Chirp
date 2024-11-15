using Microsoft.Playwright;
using NUnit;
using static Microsoft.Playwright.Assertions;

namespace PlaywrightTests;

public class EndToEndTest
{
    [Test]
    public async Task IsolateAuthorCheepsTest()
    {
        
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
        });
        var context = await browser.NewContextAsync();

        var page = await context.NewPageAsync();
        await page.GotoAsync("https://bdsagroup25chirprazor1.azurewebsites.net/");
        await page.Locator("p").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck" }).GetByRole(AriaRole.Link).ClickAsync();
        await page.GetByRole(AriaRole.Heading, new() { Name = "Jacqualine Gilcoine's Timeline" }).ClickAsync();
    }

    [Test]
    public async Task IsolateAuthorCheepsTest1()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
        });
        var context = await browser.NewContextAsync();

        var page = await context.NewPageAsync();
        await page.GotoAsync("https://bdsagroup25chirprazor1.azurewebsites.net/");
        await page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
        await page.GetByPlaceholder("name@example.com").ClickAsync();
        await page.GetByPlaceholder("name@example.com").FillAsync("test@test.dk");
        await page.GetByPlaceholder("password").ClickAsync();
        await page.GetByPlaceholder("password").FillAsync("password");
        await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await Expect(page.Locator("body")).ToContainTextAsync("logout [test@test.dk]");
    }
}