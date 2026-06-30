# C# NUnit Test Framework with Playwright

A structured UI test automation framework built on **Playwright for .NET** and **NUnit**, following the [Aquality Automation](https://github.com/aquality-automation) design patterns adapted for Playwright's modern async API.

---

## Architecture Overview

The solution is split into two projects to separate **reusable framework code** from **project-specific tests and page objects**.

```
ExampleProject/                              ← Solution root
├── ExampleProject.slnx                      ← Solution file
│
├── ExampleProject/                          ← Test project (project-specific)
│   ├── ExampleProject.csproj
│   ├── Configuration/
│   │   ├── frameworkSettings.json           ← Environment settings (BaseUrl, timeouts)
│   │   └── testData.json                    ← Test data (credentials, search queries)
│   ├── Pages/                               ← Page Objects (inherit BaseForm)
│   │   ├── LoginPage.cs
│   │   ├── MainPage.cs
│   │   ├── DropdownPage.cs
│   │   └── IFramePage.cs
│   ├── Tests/                               ← NUnit test classes (inherit BaseTest)
│   │   ├── BaseTest.cs                      ← Shared setup and browser configuration
│   │   ├── LoginTests.cs
│   │   ├── ElementsListDemoTests.cs
│   │   └── DropdownAndIFrameTests.cs
│   └── playwright.runsettings               ← Playwright NUnit configuration
│
└── FrameworkCore/                           ← Reusable framework library
    ├── FrameworkCore.csproj
    ├── Elements/                            ← Typed UI element wrappers
    │   ├── Interfaces/                      ← IElement, IButton, IElementsList, etc.
    │   ├── BaseElement.cs                   ← Abstract base with Name, State, Click
    │   ├── Button.cs, TextBox.cs, Label.cs
    │   ├── CheckBox.cs, ComboBox.cs
    │   ├── ElementsList.cs                  ← Wrapper for a list of elements
    │   ├── ElementFactory.cs                ← Creates typed elements from ILocator
    │   └── ElementStateProvider.cs          ← IsDisplayed, WaitForDisplayed, etc.
    ├── Forms/                               ← Page/Form abstraction
    │   ├── Interfaces/                      ← IForm
    │   └── BaseForm.cs                      ← Abstract base with unique locator pattern
    └── Utilities/
        ├── BrowserUtils.cs                  ← Browser helper (navigate, tabs, downloads)
        ├── FrameUtils.cs                    ← Helper for resolving locators in IFrames
        ├── ConfigReader.cs                  ← JSON configuration reader
        └── Logger.cs                        ← Console logger via NUnit TestContext
```

---

## Key Design Patterns

### Aquality Pattern (Adapted for Playwright)

| Concept | Aquality (Selenium) | This Framework (Playwright) |
|---|---|---|
| Browser driver | `AqualityServices.Browser` | Playwright `IPage` via `BaseTest` |
| Page Object base | `Form` / `BaseForm` | `BaseForm` with unique `ILocator` |
| Element wrappers | `Button`, `TextBox` via `ElementFactory` | Same — `Button`, `TextBox` via `ElementFactory` |
| List of elements | `IList<IElement>` | `IElementsList<TElement>` |
| Element state | `element.State.WaitForDisplayed()` | `element.State.WaitForDisplayedAsync()` |
| Configuration | `settings.json` | `frameworkSettings.json` + `playwright.runsettings` |
| Logging | Aquality Logger | `Logger.cs` → NUnit `TestContext.Progress` |
| Browser utilities | `AqualityServices.Browser` methods | `BrowserUtils` static helper class |

### Page Object Model (POM)

Each page is a class inheriting `BaseForm`. Elements are declared as **private** properties using `ElementFactory`, and interaction is exposed via **public** methods:

```csharp
public class LoginPage : BaseForm
{
    public LoginPage(IPage page)
        : base(page, page.GetByRole(AriaRole.Button, new() { Name = "Login" }), "Login Page")
    { }

    private ITextBox UsernameInput => ElementFactory.GetTextBox(Page.Locator("#username"), "Username Field");
    private IButton SubmitButton   => ElementFactory.GetButton(Page.GetByRole(AriaRole.Button, new() { Name = "Login" }), "Submit Button");

    public async Task LoginAsAsync(string username, string password) 
    { 
        await UsernameInput.TypeAsync(username);
        await SubmitButton.ClickAsync();
    }
}
```

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) (or later)
- PowerShell (for Playwright browser installation)

---

## Setup & Installation

### 1. Clone the repository

```bash
git clone <repository-url>
cd csharp_unit_framework_playwright
```

### 2. Restore NuGet packages

```bash
cd ExampleProject
dotnet restore
```

### 3. Build the solution

```bash
dotnet build
```

### 4. Install Playwright browsers

```powershell
pwsh ExampleProject/bin/Debug/net10.0/playwright.ps1 install
```

> **Note:** This downloads Chromium, Firefox, and WebKit binaries managed by Playwright. Run this once per machine or after updating the Playwright NuGet package.

---

## Running Tests

### Run all tests

```bash
dotnet test --settings ExampleProject/playwright.runsettings
```

### Run a specific test class

```bash
dotnet test --settings ExampleProject/playwright.runsettings --filter "ClassName=ExampleProject.Tests.LoginTests"
```

### Run in headed mode (default) vs. headless

Edit `playwright.runsettings` → `<Headless>`:
- `false` — browser window is visible (default, useful for debugging)
- `true` — headless execution (recommended for CI)

### Cross-Browser Testing

The framework natively supports running tests on different browsers by specifying the `BROWSER` environment variable. `BaseTest.cs` uses this variable to dynamically select the browser, overriding default settings.

Supported values: `chromium` (default), `firefox`, `webkit`.

**Example (PowerShell):**
```powershell
$env:BROWSER="firefox"; dotnet test --settings ExampleProject/playwright.runsettings
```

**Example (Bash/CI):**
```bash
BROWSER=webkit dotnet test --settings ExampleProject/playwright.runsettings
```

---

## Configuration

### `frameworkSettings.json`

Framework-specific settings consumed by `ConfigReader` (e.g. environment URL, custom explicit timeouts):

```json
{
    "BaseUrl": "https://the-internet.herokuapp.com/",
    "Timeouts": {
        "ExplicitWaitMs": 15000,
        "PageLoadMs": 30000
    }
}
```

### `testData.json`

Test data consumed by `ConfigReader.GetTestDataSection()`:

```json
{
    "ValidCredentials": {
        "Username": "tomsmith",
        "Password": "SuperSecretPassword!"
    }
}
```

### `playwright.runsettings`

Playwright NUnit runtime configuration (engine, headless mode, assertion timeouts, test parallelization).

---

## Utilities

### BrowserUtils

The `BrowserUtils` static class provides logged convenience methods for common browser operations:

```csharp
// Navigation & Info
await BrowserUtils.NavigateToAsync(Page, "https://example.com");
await BrowserUtils.RefreshAsync(Page);
string url = BrowserUtils.GetCurrentUrl(Page);

// Tabs / Popups
IPage newTab = await BrowserUtils.OpenNewTabAsync(Context, "https://example.com");
IPage popup = await BrowserUtils.SwitchToNewTabAsync(Page, () => link.ClickAsync());

// File Downloads
string filePath = await BrowserUtils.DownloadFileAsync(Page, () => downloadBtn.ClickAsync(), @"C:\Downloads");

// JavaScript & Scrolling
await BrowserUtils.ExecuteJavaScriptAsync(Page, "window.scrollTo(0, 500)");
await BrowserUtils.ScrollIntoViewAsync(someLocator);

// Dialog handling (register BEFORE triggering the dialog)
BrowserUtils.AcceptNextDialog(Page);
```

### FrameUtils

Provides elegant support for IFrame interactions. Instead of switching contexts, you resolve locators *through* the frame and pass them to the `ElementFactory`.

```csharp
// In a Page Object:
private ITextBox EditorTextBox => _elementFactory.GetTextBox(
    FrameUtils.GetLocatorInFrame(Page, "#mce_0_ifr", "#tinymce"), 
    "TinyMCE Editor Body");

// Now you can interact with it transparently:
await EditorTextBox.GetTextAsync();
```

---

## Framework Element Types

| Element | Interface | Key Methods |
|---|---|---|
| `Button` | `IButton` | `ClickAsync()` |
| `TextBox` | `ITextBox` | `TypeAsync(text)`, `ClearAsync()`, `GetTextAsync()` |
| `Label` | `ILabel` | `GetTextAsync()` |
| `CheckBox` | `ICheckBox` | `CheckAsync()`, `UncheckAsync()`, `IsCheckedAsync()` |
| `ComboBox` | `IComboBox` | `SelectByTextAsync(text)`, `SelectByValueAsync(value)`, `SelectByIndexAsync(index)`, `GetSelectedOptionTextAsync()` |

### ElementsList
To work with multiple identical elements (e.g. a list of links), use `IElementsList<T>`:

```csharp
// In Page Object:
private IElementsList<Button> MenuLinks => ElementFactory.GetList<Button>(Page.Locator("ul li a"), "Menu Links");

// Usage:
int count = await MenuLinks.CountAsync();
IReadOnlyList<string> texts = await MenuLinks.GetTextsAsync();
await MenuLinks.GetElementByIndex(0).ClickAsync();
```

All elements inherit from `BaseElement` and expose a `.State` property (`IElementStateProvider`) with:
- `IsDisplayedAsync()`, `IsExistAsync()`, `IsEnabledAsync()`
- `WaitForDisplayedAsync()`, `WaitForNotDisplayedAsync()`, `WaitForEnabledAsync()`

---

## License

This project is for educational and internal use.
