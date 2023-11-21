using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System;
using System.Threading.Tasks;


namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest
{
  
    [Test]
    public async Task PublicGoToPrivateGoTOPublic() {
        
        await using var browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
        });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true,
        });

        var page = await context.NewPageAsync();

        await page.GotoAsync("https://localhost:7040/");
        
        await page.Locator("p").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck now is what we hear the worst. — 01-08-2023 11:17:3" }).GetByRole(AriaRole.Link).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();


    }

    [Test]
    public async Task LoginLoguot() {

        await using var browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
        });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true,
        });

        var page = await context.NewPageAsync();

        await page.GotoAsync("https://localhost:7040/");

        await page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();

        await page.GetByLabel("Username or email address").ClickAsync();

        await page.GetByLabel("Username or email address").FillAsync("github@nyrrdin.com");

        await page.GetByLabel("Username or email address").PressAsync("Tab");

        await page.GetByLabel("Password").FillAsync("23Passwrod");

        await page.GetByRole(AriaRole.Button, new() { Name = "Sign in", Exact = true }).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "logout [UITestAreGreat]" }).ClickAsync();
    }

    [Test]
    public async Task LoginCreateCheepCheckOutPrivateTimeline() {

        await using var browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
        });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true,
        });

        var page = await context.NewPageAsync();

        await page.GotoAsync("https://localhost:7040/");

        await page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();

        await page.GetByLabel("Username or email address").ClickAsync();

        await page.GetByLabel("Username or email address").FillAsync("github@nyrrdin.com");

        await page.GetByLabel("Username or email address").PressAsync("Tab");

        await page.GetByLabel("Password").FillAsync("23Passwrod");

        await page.GetByRole(AriaRole.Button, new() { Name = "Sign in", Exact = true }).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "Make cheep" }).ClickAsync();

        await page.GetByPlaceholder("Cheep here!").ClickAsync();

        await page.GetByPlaceholder("Cheep here!").FillAsync("Test");

        await page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();

    }
}