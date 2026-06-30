using FrameworkCore.Utilities;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace ExampleProject.Tests
{
    /// <summary>
    /// Base class for all test fixtures.
    /// Inherits Playwright's PageTest which provides Page, Context, and Browser instances.
    ///
    /// Browser selection priority (handled natively by Playwright NUnit):
    ///   1. BROWSER environment variable (e.g. set BROWSER=firefox)
    ///   2. BrowserName from playwright.runsettings
    ///   3. Default: chromium
    ///
    /// Examples:
    ///   Local:  set BROWSER=firefox &amp;&amp; dotnet test
    ///   CI:     env BROWSER=webkit dotnet test
    /// </summary>
    [TestFixture]
    internal abstract class BaseTest : PageTest
    {
        protected string BaseUrl { get; private set; } = null!;

        [SetUp]
        public async Task BaseSetUp()
        {
            BaseUrl = ConfigReader.GetFrameworkSetting<string>("BaseUrl");
            Logger.Info($"Browser: {BrowserName}, BaseUrl: {BaseUrl}");
        }
    }
}
