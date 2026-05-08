# AutomationExerciseTests

> End-to-end test automation suite for [automationexercise.com](https://automationexercise.com/) — built with Microsoft Playwright and NUnit.

![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-14.0-239120?logo=csharp&logoColor=white)
![Playwright](https://img.shields.io/badge/Playwright-NUnit-2EAD33?logo=playwright&logoColor=white)
![NUnit](https://img.shields.io/badge/NUnit-test%20runner-brightgreen)

---

## Table of Contents

- [Overview](#overview)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Running Tests](#running-tests)
- [Test Coverage](#test-coverage)
- [Design Decisions](#design-decisions)
- [CI/CD](#cicd)
- [Troubleshooting](#troubleshooting)

---

## Overview

This suite validates critical user-facing workflows on the Automation Exercise demo site across six feature areas:

| Feature | Tests |
|---|---|
| User accounts | Registration, login, logout, duplicate email |
| Product catalog | Navigation, product details, search |
| Shopping cart | Add items, quantity, price calculation, persistence |
| Newsletter | Email subscription |
| Contact form | Form submission with dialog handling |
| Bonus scenarios | Cart price accuracy, session persistence, cross-tab cart synchronization |

---

## Tech Stack

| Tool | Version | Role |
|---|---|---|
| C# | 14.0 | Language |
| .NET | 10 | Target framework |
| NUnit | latest | Test runner |
| Microsoft.Playwright.NUnit | latest | Browser automation via `PageTest` base |

---

## Project Structure

```
AutomationExerciseTests/
├── Tests/
│   ├── BaseTest.cs              # Shared setup: ad blocking, navigation, consent
│   ├── AccountTests.cs          # RegisterUser, LoginCorrect, Logout, RegisterExistingEmail
│   ├── ProductTests.cs          # VerifyTestCase, VerifyProduct, SearchProduct
│   ├── CartTests.cs             # AddProducts
│   ├── ContactTests.cs          # ContactForm
│   ├── SubscriptionTests.cs     # VerifySubscription
│   └── BonusTests.cs            # CheckCartTotal, CartVisibleInNewTab
├── AutomationExerciseTests.csproj
├── .runsettings                 # NUnit and Playwright configuration
├── .gitignore
└── README.md
```

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- PowerShell (Windows) or a POSIX shell (macOS / Linux)
- Git

### Setup

**1. Clone the repository**
```powershell
git clone https://github.com/dinescuuandrei/AutomationExerciseTests.git
cd AutomationExerciseTests
```

**2. Restore packages**
```powershell
dotnet restore
```

**3. Build the project**
```powershell
dotnet build
```

**4. Install Playwright browsers**
```powershell
pwsh bin/Debug/net10/playwright.ps1 install
```

> **Note:** The script path may vary with build configuration and target framework. Alternatively, install the Playwright CLI globally:
> ```powershell
> dotnet tool install --global Microsoft.Playwright.CLI
> playwright install
> ```

---

## Running Tests

**Run all tests**
```powershell
dotnet test
```

**Run a specific test by name**
```powershell
dotnet test --filter "TestName=RegisterUser"
```

**Run a specific test by fully qualified name**
```powershell
dotnet test --filter "FullyQualifiedName=AutomationExerciseTests.Tests.AccountTests.RegisterUser"
```

**Run in headed/debug mode**
```powershell
# Windows PowerShell
$env:PWDEBUG = "1"; dotnet test

# Bash / macOS
PWDEBUG=1 dotnet test
```

**Increase verbosity**
```powershell
dotnet test --verbosity detailed
```

---

## Test Coverage

All test classes inherit from `BaseTest`, which handles:
- Blocking ad-serving domains (`google`, `adservice`, `doubleclick`) to improve reliability
- Navigation to the homepage before each test
- Automatic dismissal of GDPR consent dialogs (2500ms timeout)

---

### AccountTests.cs

| Test | Description |
|---|---|
| `RegisterUser` | Full signup flow: fills the registration form with random email, verifies account creation, then deletes the account |
| `LoginCorrect` | Logs in with valid credentials and verifies "Logged in as Andrei Dinescu" message |
| `Logout` | Logs in then logs out and verifies redirect to the login page |
| `RegisterExistingEmail` | Attempts signup with a duplicate email and verifies the error message |

---

### ProductTests.cs

| Test | Description |
|---|---|
| `VerifyTestCase` | Navigates to the Test Cases page via the navbar link and verifies the URL |
| `VerifyProduct` | Opens the All Products page, clicks the first product, and verifies name, category, availability, and brand are visible |
| `SearchProduct` | Searches for "Polo", verifies the "SEARCHED PRODUCTS" heading and that at least one result is visible |

---

### CartTests.cs

| Test | Description |
|---|---|
| `AddProducts` | Navigates to Products, hovers the first item, adds it to cart, clicks the cart icon, and verifies exactly 1 row in the cart table |

---

### ContactTests.cs

| Test | Description |
|---|---|
| `ContactForm` | Fills the contact form (name, email, subject, message), accepts the browser dialog on submit, and verifies the success status message |

---

### SubscriptionTests.cs

| Test | Description |
|---|---|
| `VerifySubscription` | Locates the Subscription widget on the homepage, verifies the heading text, enters an email, submits, and verifies the success banner is visible |

---

### BonusTests.cs

| Test | Description |
|---|---|
| `CheckCartTotal` | Sets quantity to 4, adds the product to cart, verifies `total = unit price × 4`, reloads the page and confirms the total persists after refresh |
| `CartVisibleInNewTab` | Adds a product with quantity 2, opens the cart in a new browser tab via `Context.NewPageAsync()`, and verifies the cart contains 1 row with quantity `2` — confirming cart state is shared across tabs within the same browser context |

---

## Design Decisions

- **Selector strategy:** Role-based selectors, `data-qa` attributes, and text-based locators are preferred over CSS classes or XPath for better resilience to layout changes.
- **Ad blocking:** Route interception in `BaseTest` aborts requests to `google`, `doubleclick`, and `adservice` domains before each test, reducing flakiness.
- **Hard-coded test data:** Some tests (`LoginCorrect`, `Logout`, `RegisterExistingEmail`) rely on credentials that must already exist on the site. `RegisterUser` generates a random email per run to avoid conflicts.

---

## CI/CD

Example GitHub Actions workflow for running the full suite on every push and pull request:

```yaml
name: Playwright Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: windows-latest

    steps:
      - uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.x'

      - name: Restore dependencies
        run: dotnet restore

      - name: Build
        run: dotnet build --configuration Release

      - name: Install Playwright browsers
        run: pwsh -c "pwsh ./bin/Release/net10/playwright.ps1 install"

      - name: Run tests
        run: dotnet test --configuration Release --verbosity minimal
```

> Adjust paths and matrix configuration as needed for your environment.

---

## Troubleshooting

| Symptom | Fix |
|---|---|
| Elements not found / test flakiness | Run in headed mode (`PWDEBUG=1`) to observe browser behavior |
| Playwright browser not found | Re-run the Playwright installer step |
| `LoginCorrect` or `Logout` fails | The hard-coded credentials must already exist on the site; register them manually first or update the values in the test |
| `RegisterUser` fails at account creation | The random email may have been used before; re-run the test |
| Script path not found after build | Verify build configuration and target framework match the path in the install command |