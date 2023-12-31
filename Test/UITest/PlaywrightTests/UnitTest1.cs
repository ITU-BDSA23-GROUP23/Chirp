using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace PlaywrightTests;

/// <summary>
/// A lot of this code has been generated with the use of playwright's codegen
/// https://playwright.dev/dotnet/docs/codegen-intro
/// For the login part we have made a github account for the test
/// Sometimes the login will require a Verification code from the email 
/// The email and password are the same for the email as for the github
/// The page to login is one.com
/// </summary>


[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest
{
  
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
     public async Task LoginCreateCheepCheckOutPrivateTimeline()
    {
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

        await page.GetByLabel("Password").ClickAsync();

        await page.GetByLabel("Password").FillAsync("23Passwrod");

        await page.GetByRole(AriaRole.Button, new() { Name = "Sign in", Exact = true }).ClickAsync();

        await page.GetByPlaceholder("Cheep here!").ClickAsync();

        await page.GetByPlaceholder("Cheep here!").FillAsync("Test");

        await page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();

    }

    [Test]
    public async Task LoginFollowCheckFollowTimelineUnfollowChecktimelineAgain()
    {
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

        await page.GetByLabel("Username or email address").ClickAsync(new LocatorClickOptions
        {
            Modifiers = new[] { KeyboardModifier.Control },
        });

        await page.GetByLabel("Username or email address").PressAsync("Tab");

        await page.GetByLabel("Username or email address").ClickAsync();

        await page.GetByLabel("Username or email address").FillAsync("github@nyrrdin.com");

        await page.GetByLabel("Password").ClickAsync(new LocatorClickOptions
        {
            Modifiers = new[] { KeyboardModifier.Control },
        });

        await page.GetByLabel("Password").FillAsync("23Passwrod");

        await page.GetByRole(AriaRole.Button, new() { Name = "Sign in", Exact = true }).ClickAsync();

        await page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck now is what we hear the worst. — 01-08-2023 11:17:3" }).GetByRole(AriaRole.Button).First.ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "For You" }).ClickAsync();

        await page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck now is what we hear the worst. — 01-08-2023 11:17:3" }).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();

        await page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck now is what we hear the worst. — 01-08-2023 11:17:3" }).GetByRole(AriaRole.Button).First.ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "For You" }).ClickAsync();

        await page.GetByText("There are no cheeps so far.").ClickAsync();

    }

    [Test]
    public async Task forgetmeTest()
    { 
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

        await page.GetByLabel("Username or email address").ClickAsync(new LocatorClickOptions
        {
            Modifiers = new[] { KeyboardModifier.Control },
        });

        await page.GetByLabel("Username or email address").FillAsync("github@nyrrdin.com");

        await page.GetByLabel("Password").ClickAsync();

        await page.GetByLabel("Password").ClickAsync(new LocatorClickOptions
        {
            Modifiers = new[] { KeyboardModifier.Control },
        });
        
        await page.GetByLabel("Password").FillAsync("23Passwrod");

        await page.GetByRole(AriaRole.Button, new() { Name = "Sign in", Exact = true }).ClickAsync();

        await page.GetByPlaceholder("Cheep here!").ClickAsync();

        await page.GetByPlaceholder("Cheep here!").FillAsync("This is a test");

        await page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();

        await page.GetByRole(AriaRole.Listitem).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "About me" }).ClickAsync();

        await page.GetByRole(AriaRole.Button, new() { Name = "Forget me" }).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();

        await page.GetByText("There are no cheeps so far.").ClickAsync();


    }




}