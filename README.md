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
│   │   ├── frameworkSettings.json           ← Environment settings (BaseUrl, browser, timeouts)
│   │   └── testData.json                    ← Test data (credentials, search queries)
│   ├── Pages/                               ← Page Objects (inherit BaseForm)
│   │   ├── LoginPage.cs
│   │   └── MainPage.cs
│   ├── Tests/                               ← NUnit test classes
│   │   └── LoginTests.cs
│   └── playwright.runsettings               ← Playwright NUnit configuration
│
└── FrameworkCore/                           ← Reusable framework library
    ├── FrameworkCore.csproj
    ├── Elements/                            ← Typed UI element wrappers
    │   ├── Interfaces/                      ← IElement, IButton, ITextBox, ILabel, ...
    │   ├── BaseElement.cs                   ← Abstract base with Name, State, Click
    │   ├── Button.cs, TextBox.cs, Label.cs
    │   ├── CheckBox.cs, ComboBox.cs
    │   ├── ElementFactory.cs                ← Creates typed elements from ILocator
    │   └── ElementStateProvider.cs          ← IsDisplayed, WaitForDisplayed, etc.
    ├── Forms/                               ← Page/Form abstraction
    │   ├── Interfaces/                      ← IForm
    │   └── BaseForm.cs                      ← Abstract base with unique locator pattern
    └── Utilities/
        ├── BrowserUtils.cs                  ← Browser helper (navigate, screenshot, JS, scroll)
        ├── ConfigReader.cs                  ← JSON configuration reader
        └── Logger.cs                        ← Console logger via NUnit TestContext
```

---

## Key Design Patterns

### Aquality Pattern (Adapted for Playwright)

| Concept | Aquality (Selenium) | This Framework (Playwright) |
|---|---|---|
| Browser driver | `AqualityServices.Browser` | Playwright `IPage` via NUnit `PageTest` base class |
| Page Object base | `Form` / `BaseForm` | `BaseForm` with unique `ILocator` |
| Element wrappers | `Button`, `TextBox`, etc. via `ElementFactory` | Same — `Button`, `TextBox`, etc. via `ElementFactory` |
| Element state | `element.State.WaitForDisplayed()` | `element.State.WaitForDisplayedAsync()` |
| Configuration | `settings.json` | `frameworkSettings.json` + `playwright.runsettings` |
| Logging | Aquality Logger | `Logger.cs` → NUnit `TestContext.Progress` |
| Browser utilities | `AqualityServices.Browser` methods | `BrowserUtils` static helper class |

### Page Object Model (POM)

Each page is a class inheriting `BaseForm`. Elements are declared as properties using `ElementFactory`:

```csharp
public class LoginPage : BaseForm
{
    public LoginPage(IPage page)
        : base(page, page.GetByRole(AriaRole.Button, new() { Name = "Login" }), "Login Page")
    { }

    private ITextBox UsernameInput => ElementFactory.GetTextBox(Page.Locator("#username"), "Username Field");
    private IButton SubmitButton   => ElementFactory.GetButton(Page.GetByRole(AriaRole.Button, new() { Name = "Login" }), "Submit Button");

    public async Task LoginAsAsync(string username, string password) { /* ... */ }
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

### Run a specific test method

```bash
dotnet test --settings ExampleProject/playwright.runsettings --filter "FullyQualifiedName=ExampleProject.Tests.LoginTests.UserCanLoginWithValidCredentials"
```

### Run in headed mode (default) vs. headless

Edit `playwright.runsettings` → `<Headless>`:
- `false` — browser window is visible (default, useful for debugging)
- `true` — headless execution (recommended for CI)

### Run with a different browser

Edit `playwright.runsettings` → `<BrowserName>`:
- `chromium` (default)
- `firefox`
- `webkit`

---

## Configuration

### `frameworkSettings.json`

Environment-level settings consumed by `ConfigReader`:

```json
{
    "BaseUrl": "https://the-internet.herokuapp.com/",
    "Browser": "chromium",
    "Headless": false,
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
    },
    "InvalidCredentials": {
        "Username": "invalid_user",
        "Password": "invalid_password"
    }
}
```

### `playwright.runsettings`

Playwright NUnit runtime configuration (browser type, headless mode, timeouts, parallelism). This is the file that directly controls Playwright behavior at test execution time.

---

## How to Add New Tests

### 1. Create a Page Object

Create a new class in `ExampleProject/Pages/` inheriting `BaseForm`:

```csharp
public class MyNewPage : BaseForm
{
    public MyNewPage(IPage page)
        : base(page, page.Locator("#unique-element"), "My New Page")
    { }

    private IButton SomeButton => ElementFactory.GetButton(Page.Locator("#btn"), "Some Button");

    public async Task ClickSomeButtonAsync()
    {
        await SomeButton.ClickAsync();
    }
}
```

### 2. Create a Test Class

Create a new class in `ExampleProject/Tests/` inheriting `PageTest`:

```csharp
[TestFixture]
internal class MyNewTests : PageTest
{
    private MyNewPage _page;

    [SetUp]
    public async Task Setup()
    {
        string baseUrl = ConfigReader.GetFrameworkSetting<string>("BaseUrl");
        await Page.GotoAsync($"{baseUrl}my-route");
        _page = new MyNewPage(Page);
        await _page.WaitForDisplayedAsync();
    }

    [Test]
    public async Task SomeFeatureWorksCorrectly()
    {
        await _page.ClickSomeButtonAsync();
        // Assert...
    }
}
```

---

## BrowserUtils

The `BrowserUtils` static class provides logged convenience methods for common browser operations:

```csharp
// Navigation
await BrowserUtils.NavigateToAsync(Page, "https://example.com");
await BrowserUtils.RefreshAsync(Page);
await BrowserUtils.GoBackAsync(Page);

// Screenshots
await BrowserUtils.TakeScreenshotAsync(Page, "screenshots/login.png");

// JavaScript
string title = await BrowserUtils.ExecuteJavaScriptAsync<string>(Page, "document.title");

// Scrolling
await BrowserUtils.ScrollToBottomAsync(Page);
await BrowserUtils.ScrollIntoViewAsync(someLocator);

// Wait strategies
await BrowserUtils.WaitForPageLoadAsync(Page);
await BrowserUtils.WaitForUrlAsync(Page, "**/dashboard");

// Tab management
IPage newTab = await BrowserUtils.OpenNewTabAsync(Context, "https://example.com");
await BrowserUtils.CloseTabAsync(newTab);

// Dialog handling (register BEFORE triggering the dialog)
BrowserUtils.AcceptNextDialog(Page);
```

---

## Framework Element Types

| Element | Interface | Key Methods |
|---|---|---|
| `Button` | `IButton` | `ClickAsync()` |
| `TextBox` | `ITextBox` | `TypeAsync(text)`, `ClearAsync()` |
| `Label` | `ILabel` | `GetTextAsync()` |
| `CheckBox` | `ICheckBox` | `CheckAsync()`, `UncheckAsync()`, `IsCheckedAsync()` |
| `ComboBox` | `IComboBox` | `SelectByTextAsync(text)`, `SelectByValueAsync(value)` |

All elements inherit from `BaseElement` and expose a `.State` property (`IElementStateProvider`) with:
- `IsDisplayedAsync()`, `IsExistAsync()`, `IsEnabledAsync()`
- `WaitForDisplayedAsync()`, `WaitForNotDisplayedAsync()`, `WaitForEnabledAsync()`

---

## License

This project is for educational and internal use.
