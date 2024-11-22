using Microsoft.Playwright;
using NUnit;
using static Microsoft.Playwright.Assertions;

namespace PlaywrightTests;

public class EndToEndTest
{
    //Registers the user with username: test, email: test@test.dk, password: password
    //If username is already taken try to discard changes on database in git
    [Test]
    public async Task RegisterUserTest()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
        });
        var context = await browser.NewContextAsync();

        var page = await context.NewPageAsync();
        await page.GotoAsync("http://localhost:5273/");
        await page.GetByRole(AriaRole.Link, new() { Name = "register" }).ClickAsync();
        await page.GetByPlaceholder("username ").ClickAsync();
        await page.GetByPlaceholder("username ").FillAsync("test");
        await page.GetByPlaceholder("name@example.com").ClickAsync();
        var page1 = await context.NewPageAsync();
        await page1.CloseAsync();
        await page.GetByPlaceholder("name@example.com").ClickAsync();
        await page.GetByPlaceholder("name@example.com").FillAsync("test@test.dk");
        await page.GetByLabel("Password", new() { Exact = true }).ClickAsync();
        await page.GetByLabel("Password", new() { Exact = true }).FillAsync("password");
        await page.GetByLabel("Password", new() { Exact = true }).PressAsync("Tab");
        await page.GetByLabel("Confirm Password").FillAsync("password");
        await page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
        await Expect(page.Locator("body")).ToContainTextAsync("logout [test]");
    }

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
        await page.GotoAsync("http://localhost:5273/");
        await page.Locator("p").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck" }).GetByRole(AriaRole.Link).ClickAsync();
        await page.GetByRole(AriaRole.Heading, new() { Name = "Jacqualine Gilcoine's Timeline" }).ClickAsync();
    }

    //tries to login and out with the test-user. If failed try to run RegisterUserTest() first
    [Test]
    public async Task validLoginAndOutTest()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
        });
        var context = await browser.NewContextAsync();

        var page = await context.NewPageAsync();
        await page.GotoAsync("http://localhost:5273/");
        await page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
        await page.GetByPlaceholder("Username").ClickAsync();
        await page.GetByPlaceholder("Username").FillAsync("test");
        await page.GetByPlaceholder("Username").PressAsync("Tab");
        await page.GetByPlaceholder("password").FillAsync("password");
        await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await Expect(page.Locator("body")).ToContainTextAsync("logout [test]");
        await page.GetByRole(AriaRole.Link, new() { Name = "logout [test]" }).ClickAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = "Click here to Logout" }).ClickAsync();
        await Expect(page.GetByRole(AriaRole.Paragraph)).ToContainTextAsync("You have successfully logged out of the application.");
    }

    //test that check that it is not possible to login with an account that doesnt exist
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
        await page.GotoAsync("http://localhost:5273/");
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
        await page.GotoAsync("http://localhost:5273/");
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
    //tries to cheep with the test-user. If failed try to run RegisterUserTest() first
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
        await page.GotoAsync("http://localhost:5273/");
        await page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
        await page.GetByPlaceholder("Username").ClickAsync();
        await page.GetByPlaceholder("Username").FillAsync("test");
        await page.GetByPlaceholder("Username").PressAsync("Tab");
        await page.GetByPlaceholder("password").FillAsync("password");
        await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await page.Locator("#Message").ClickAsync();
        await page.Locator("#Message").FillAsync("This is a valid cheep");
        await page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();
        await Expect(page.Locator("#messagelist")).ToContainTextAsync("This is a valid cheep");
        await Expect(page.Locator("h2")).ToContainTextAsync("test's Timeline");
    }
}