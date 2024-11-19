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

    //account that is tested on: username: "kat", email: "kat@test.dk" and password: "password"
    [Test]
    public async Task validLoginTest()
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
        await page.GetByPlaceholder("Username").ClickAsync();
        await page.GetByPlaceholder("Username").FillAsync("kat");
        await page.GetByPlaceholder("password").ClickAsync();
        await page.GetByPlaceholder("password").FillAsync("password");
        await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await page.GetByRole(AriaRole.Link, new() { Name = "logout [kat]" }).ClickAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = "Click here to Logout" }).ClickAsync();
        await Expect(page.GetByRole(AriaRole.Paragraph)).ToContainTextAsync("You have successfully logged out of the application.");
    }

    //test that check that it is not possible to login with an account that dont exist
    [Test]
    public async Task invalidLoginattemptTest()
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
        await page.GetByPlaceholder("Username").ClickAsync();
        await page.GetByPlaceholder("Username").FillAsync("ThisIsNotAReaLAcCount");
        await page.GetByPlaceholder("password").ClickAsync();
        await page.GetByPlaceholder("password").FillAsync("password");
        await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await page.GetByText("Invalid login attempt.").ClickAsync();
    }
    //Test that a user cant make a username with a "/" in front that redirects to another page
    [Test]
    public async Task TestingForRedirectionAttempt()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
        });
        var context = await browser.NewContextAsync();

        var page = await context.NewPageAsync();
        await page.GotoAsync("https://bdsagroup25chirprazor1.azurewebsites.net/");
        await page.GetByRole(AriaRole.Link, new() { Name = "register" }).ClickAsync();
        await page.GetByPlaceholder("username ").ClickAsync();
        await page.GetByPlaceholder("username ").FillAsync("/redirect");
        await page.GetByPlaceholder("name@example.com").ClickAsync();
        await page.GetByPlaceholder("name@example.com").FillAsync("test");
        await page.GetByPlaceholder("name@example.com").FillAsync("test@tester.dk");
        await page.GetByLabel("Password", new() { Exact = true }).ClickAsync();
        await page.GetByLabel("Password", new() { Exact = true }).FillAsync("password");
        await page.GetByLabel("Password", new() { Exact = true }).PressAsync("Tab");
        await page.GetByLabel("Confirm Password").FillAsync("password");
        await page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
        await page.GetByText("Username '/redirect' is").ClickAsync();
    }

    [Test]
    public async Task ValidCheepTest()
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
        await page.GetByPlaceholder("Username").ClickAsync();
        await page.GetByPlaceholder("Username").FillAsync("kat");
        await page.GetByPlaceholder("password").ClickAsync();
        await page.GetByPlaceholder("password").FillAsync("password");
        await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await page.Locator("#Message").ClickAsync();
        await page.Locator("#Message").FillAsync("this is a valid cheep");
        await page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();
        await Expect(page.Locator("#messagelist")).ToContainTextAsync("this is a valid cheep");
    }
}