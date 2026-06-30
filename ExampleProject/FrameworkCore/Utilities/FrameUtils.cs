using Microsoft.Playwright;
using System.Threading.Tasks;

namespace FrameworkCore.Utilities
{
    /// <summary>
    /// Utility class for interacting with IFrames using Playwright's FrameLocator API.
    /// In Playwright, you don't explicitly "switch" to a frame like in Selenium.
    /// Instead, you create a FrameLocator and resolve child locators from it.
    /// </summary>
    public static class FrameUtils
    {
        /// <summary>
        /// Gets a FrameLocator for the given frame selector.
        /// Useful when you need to chain multiple locators inside the same frame.
        /// </summary>
        public static IFrameLocator GetFrame(IPage page, string frameSelector)
        {
            Logger.Info($"Targeting frame with selector: {frameSelector}");
            return page.FrameLocator(frameSelector);
        }

        /// <summary>
        /// Gets an ILocator for an element inside a specific frame.
        /// Can be passed directly to ElementFactory or BaseForm.
        /// </summary>
        public static ILocator GetLocatorInFrame(IPage page, string frameSelector, string elementSelector)
        {
            Logger.Info($"Targeting element '{elementSelector}' inside frame '{frameSelector}'");
            return page.FrameLocator(frameSelector).Locator(elementSelector);
        }

        /// <summary>
        /// Gets an ILocator for an element inside a nested frame (Frame within a Frame).
        /// </summary>
        public static ILocator GetLocatorInNestedFrame(IPage page, string parentFrameSelector, string childFrameSelector, string elementSelector)
        {
            Logger.Info($"Targeting element '{elementSelector}' inside nested frame '{parentFrameSelector}' -> '{childFrameSelector}'");
            return page.FrameLocator(parentFrameSelector).FrameLocator(childFrameSelector).Locator(elementSelector);
        }
    }
}
