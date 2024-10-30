using System.Diagnostics;
using Humanizer;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace DefaultNamespace;

[TestFixture]
public class EndToEndTests : PageTest
{
    private Process _serverProcess;

    /*[SetUp]
    public async Task Init()
    {
        _serverProcess = await MyEndToEndUtil.StartServer(); // Custom utility class - not part of Playwright
    }*/

    [Test]
    public async Task IsolateAoutherCheepsTest()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
        });
        var context = await browser.NewContextAsync();

        var page = await context.NewPageAsync();
        await page.GotoAsync("https://bdsagroup25chirprazor1.azurewebsites.net/");
        await page.GetByRole(AriaRole.Link, new() { Name = "Mellie Yost" }).ClickAsync();

        
    }

    [TearDown]
    public async Task Cleanup()
    {
        _serverProcess.Kill();
        _serverProcess.Dispose();
    }

    // Test cases ...
}