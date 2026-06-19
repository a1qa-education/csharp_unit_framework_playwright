using Microsoft.Playwright;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FrameworkCore.Utilities
{
    /// <summary>
    /// Centralized browser utility class providing convenience methods for common
    /// Playwright IPage operations. Analogous to Aquality's AqualityServices.Browser
    /// helper methods, adapted for Playwright's async API.
    /// All methods include logging for traceability.
    /// </summary>
    public static class BrowserUtils
    {
        // ───────────────────────────────────────────────
        //  Navigation
        // ───────────────────────────────────────────────

        /// <summary>
        /// Navigates the page to the specified URL.
        /// </summary>
        public static async Task NavigateToAsync(IPage page, string url)
        {
            Logger.Info($"Navigating to URL: {url}");
            await page.GotoAsync(url);
        }

        /// <summary>
        /// Refreshes the current page.
        /// </summary>
        public static async Task RefreshAsync(IPage page)
        {
            Logger.Info("Refreshing the current page.");
            await page.ReloadAsync();
        }

        /// <summary>
        /// Navigates back in the browser history.
        /// </summary>
        public static async Task GoBackAsync(IPage page)
        {
            Logger.Info("Navigating back in browser history.");
            await page.GoBackAsync();
        }

        /// <summary>
        /// Navigates forward in the browser history.
        /// </summary>
        public static async Task GoForwardAsync(IPage page)
        {
            Logger.Info("Navigating forward in browser history.");
            await page.GoForwardAsync();
        }

        /// <summary>
        /// Returns the current page URL.
        /// </summary>
        public static string GetCurrentUrl(IPage page)
        {
            string url = page.Url;
            Logger.Info($"Current page URL: {url}");
            return url;
        }

        // ───────────────────────────────────────────────
        //  Wait Strategies
        // ───────────────────────────────────────────────

        /// <summary>
        /// Waits for the page to reach the "networkidle" load state,
        /// meaning there are no more than 0 network connections for at least 500ms.
        /// </summary>
        public static async Task WaitForPageLoadAsync(IPage page)
        {
            Logger.Info("Waiting for page to reach network idle state.");
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        /// <summary>
        /// Waits for the page to finish loading (the DOMContentLoaded event).
        /// </summary>
        public static async Task WaitForDomContentLoadedAsync(IPage page)
        {
            Logger.Info("Waiting for DOMContentLoaded event.");
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }

        /// <summary>
        /// Waits for a specific URL pattern to be reached.
        /// </summary>
        public static async Task WaitForUrlAsync(IPage page, string urlPattern)
        {
            Logger.Info($"Waiting for URL to match pattern: {urlPattern}");
            await page.WaitForURLAsync(urlPattern);
        }

        // ───────────────────────────────────────────────
        //  Screenshots
        // ───────────────────────────────────────────────

        /// <summary>
        /// Takes a full-page screenshot and saves it to the specified path.
        /// Parent directories are created automatically if they do not exist.
        /// </summary>
        public static async Task<byte[]> TakeScreenshotAsync(IPage page, string filePath)
        {
            Logger.Info($"Taking screenshot and saving to: {filePath}");

            string? directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            byte[] screenshot = await page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = filePath,
                FullPage = true
            });

            Logger.Info($"Screenshot saved successfully ({screenshot.Length} bytes).");
            return screenshot;
        }

        /// <summary>
        /// Takes a screenshot of a specific element.
        /// </summary>
        public static async Task<byte[]> TakeElementScreenshotAsync(ILocator locator, string filePath)
        {
            Logger.Info($"Taking element screenshot and saving to: {filePath}");

            string? directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            byte[] screenshot = await locator.ScreenshotAsync(new LocatorScreenshotOptions
            {
                Path = filePath
            });

            Logger.Info($"Element screenshot saved successfully ({screenshot.Length} bytes).");
            return screenshot;
        }

        // ───────────────────────────────────────────────
        //  JavaScript Execution
        // ───────────────────────────────────────────────

        /// <summary>
        /// Executes a JavaScript expression on the page and returns the result.
        /// </summary>
        public static async Task<T> ExecuteJavaScriptAsync<T>(IPage page, string script)
        {
            Logger.Info($"Executing JavaScript: {script}");
            return await page.EvaluateAsync<T>(script);
        }

        /// <summary>
        /// Executes a JavaScript expression on the page without returning a result.
        /// </summary>
        public static async Task ExecuteJavaScriptAsync(IPage page, string script)
        {
            Logger.Info($"Executing JavaScript: {script}");
            await page.EvaluateAsync(script);
        }

        // ───────────────────────────────────────────────
        //  Scrolling
        // ───────────────────────────────────────────────

        /// <summary>
        /// Scrolls to the top of the page.
        /// </summary>
        public static async Task ScrollToTopAsync(IPage page)
        {
            Logger.Info("Scrolling to the top of the page.");
            await page.EvaluateAsync("window.scrollTo(0, 0)");
        }

        /// <summary>
        /// Scrolls to the bottom of the page.
        /// </summary>
        public static async Task ScrollToBottomAsync(IPage page)
        {
            Logger.Info("Scrolling to the bottom of the page.");
            await page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");
        }

        /// <summary>
        /// Scrolls a specific element into view.
        /// </summary>
        public static async Task ScrollIntoViewAsync(ILocator locator)
        {
            Logger.Info("Scrolling element into view.");
            await locator.ScrollIntoViewIfNeededAsync();
        }

        // ───────────────────────────────────────────────
        //  Tabs / Pages
        // ───────────────────────────────────────────────

        /// <summary>
        /// Opens a new tab and navigates to the specified URL.
        /// Returns the new IPage instance.
        /// </summary>
        public static async Task<IPage> OpenNewTabAsync(IBrowserContext context, string url)
        {
            Logger.Info($"Opening new tab and navigating to: {url}");
            IPage newPage = await context.NewPageAsync();
            await newPage.GotoAsync(url);
            return newPage;
        }

        /// <summary>
        /// Closes the specified page/tab.
        /// </summary>
        public static async Task CloseTabAsync(IPage page)
        {
            Logger.Info($"Closing tab with URL: {page.Url}");
            await page.CloseAsync();
        }

        // ───────────────────────────────────────────────
        //  Page Info
        // ───────────────────────────────────────────────

        /// <summary>
        /// Returns the title of the current page.
        /// </summary>
        public static async Task<string> GetTitleAsync(IPage page)
        {
            string title = await page.TitleAsync();
            Logger.Info($"Page title: {title}");
            return title;
        }

        /// <summary>
        /// Returns the full HTML content of the page.
        /// </summary>
        public static async Task<string> GetPageSourceAsync(IPage page)
        {
            Logger.Info("Retrieving page source HTML.");
            return await page.ContentAsync();
        }

        // ───────────────────────────────────────────────
        //  Alerts / Dialogs
        // ───────────────────────────────────────────────

        /// <summary>
        /// Registers a one-time dialog handler that accepts the dialog.
        /// Must be called BEFORE the action that triggers the dialog.
        /// </summary>
        public static void AcceptNextDialog(IPage page, string? promptText = null)
        {
            Logger.Info("Registering handler to accept the next dialog.");
            page.Dialog += async (_, dialog) =>
            {
                Logger.Info($"Dialog appeared — type: {dialog.Type}, message: '{dialog.Message}'. Accepting.");
                await dialog.AcceptAsync(promptText);
            };
        }

        /// <summary>
        /// Registers a one-time dialog handler that dismisses the dialog.
        /// Must be called BEFORE the action that triggers the dialog.
        /// </summary>
        public static void DismissNextDialog(IPage page)
        {
            Logger.Info("Registering handler to dismiss the next dialog.");
            page.Dialog += async (_, dialog) =>
            {
                Logger.Info($"Dialog appeared — type: {dialog.Type}, message: '{dialog.Message}'. Dismissing.");
                await dialog.DismissAsync();
            };
        }
    }
}
