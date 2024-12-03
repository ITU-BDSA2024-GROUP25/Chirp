using Microsoft.Playwright;
using NUnit;
using static Microsoft.Playwright.Assertions;

namespace PlaywrightTests;
/**
Playwright runs tests in alphabetical order so to ensure the intended order the tests are labeled 
with a letter in front of the name.
*/
public class EndToEndTest
{
    //Registers the user with username: test, email: test@test.dk, password: password
    //If username is already taken try to run the test Z_DeleteUserTest() manually
    [Test]
    public async Task A_RegisterUserTest()
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
        await Expect(page.Locator("body")).ToContainTextAsync("Logout [test]");

    }

    [Test]
    public async Task B_IsolateAuthorCheepsTest()
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
        await page.Locator("div").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck" }).Nth(1).ClickAsync();

    }

    //tries to login and out with the test-user. If failed try to run RegisterUserTest() first
    [Test]
    public async Task C_ValidLoginAndOutTest()
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
        await page.GetByRole(AriaRole.Link, new() { Name = "Logout [test]" }).ClickAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = "Click here to Logout" }).ClickAsync();
        await Expect(page.GetByRole(AriaRole.Paragraph)).ToContainTextAsync("You have successfully logged out of the application.");

    }

    //test that check that it is not possible to login with an account that doesnt exist
    [Test]
    public async Task D_InvalidLoginAttemptTest()
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
    public async Task E_TestingForRedirectionAttempt()
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
    public async Task F_ValidCheepTest()
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
        await Expect(page.Locator("h2")).ToContainTextAsync("My Timeline");

    }
    
    //Testuseren test follows and unfollow Jacqualine
    [Test]
    public async Task G_FolloweringJacqualineTest()
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
        await page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck" }).GetByRole(AriaRole.Button).ClickAsync();
        await page.GetByRole(AriaRole.Link, new() { Name = "My Timeline" }).ClickAsync();
        await page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck" }).GetByRole(AriaRole.Button).ClickAsync();
        await page.GetByRole(AriaRole.Link, new() { Name = "Public Timeline" }).ClickAsync();
        await Expect(page.Locator("#messagelist")).ToContainTextAsync("Follow");

        
    }
    

    //tries to cheep too short with the test-user. If failed try to run RegisterUserTest() first
    [Test]
    public async Task H_TooShortCheepTest()
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
        await page.Locator("#Message").FillAsync("h");
        await page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();
        await page.GetByText("hey man, its too short, needs").ClickAsync();
    }

    [Test]
    public async Task H_DeleteCheepTest()
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
        await page.Locator("#Message").FillAsync("Delete this");
        await page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = "Delete Icon" }).ClickAsync();

    }

    [Test]
    public async Task Z_DeleteUserTest()
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
        await page.GetByPlaceholder("password").ClickAsync();
        await page.GetByPlaceholder("password").FillAsync("password");
        await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await page.GetByRole(AriaRole.Link, new() { Name = "My Page" }).ClickAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = "Delete User" }).ClickAsync();
        await Expect(page.Locator("h2")).ToContainTextAsync("Public Timeline");
    }

}