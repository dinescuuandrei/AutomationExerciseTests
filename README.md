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
│   ├── AccountTests.cs          # TC1, TC2, TC4, TC5 — registration, login, logout
│   ├── ProductTests.cs          # TC7, TC8, TC9 — catalog navigation and search
│   ├── CartTests.cs             # TC12 — cart operations
│   ├── ContactTests.cs          # TC6 — contact form submission
│   ├── SubscriptionTests.cs     # TC10 — newsletter subscription
│   └── BonusTests.cs            # TC_BONUS — cart price and persistence checks
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
dotnet test --filter "TestName=TC1"
```

**Run a specific test by fully qualified name**
```powershell
dotnet test --filter "FullyQualifiedName=AutomationExerciseTests.Tests.TC1"
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
- Blocking ad-serving domains (`googlesyndication`, `adservice`, `doubleclick`) to improve reliability
- Navigation to the homepage before each test
- Automatic dismissal of GDPR consent dialogs

---

### AccountTests.cs

| Test | Description |
|---|---|
| `TC1_RegisterUser` | Full signup flow: fill registration form, verify account created, then delete account |
| `TC2_LoginCorrect` | Login with valid credentials and verify "Logged in as" message |
| `TC4_Logout` | Login then logout and verify redirect to login page |
| `TC5_RegisterExistingEmail` | Attempt signup with a duplicate email and verify error message |

---

### ProductTests.cs

| Test | Description |
|---|---|
| `TC7_VerifyTestCasesPage` | Navigate to the Test Cases page via navbar and verify URL |
| `TC8_VerifyProductDetails` | Open a product detail page and verify name, category, availability, and brand |
| `TC9_SearchProduct` | Search for "Polo", verify results heading and at least one product visible |

---

### CartTests.cs

| Test | Description |
|---|---|
| `TC12_AddProductsInCart` | Hover first product, add to cart, confirm modal appears, view cart and verify 1 item |

---

### ContactTests.cs

| Test | Description |
|---|---|
| `TC6_ContactUsForm` | Fill the contact form, accept the browser dialog on submit, verify success message |

---

### SubscriptionTests.cs

| Test | Description |
|---|---|
| `TC10_VerifySubscription` | Locate subscription widget on homepage, enter email, submit and verify success banner |

---

### BonusTests.cs

| Test | Description |
|---|---|
| `TC_BONUS_PriceAndPersistence` | Set quantity to 4, add to cart, verify `total = unit price × 4`, reload page and verify total persists |
| `TC_BONUS_CrossTabCartSynchronization` | Add a product with quantity 2, open the cart in a new browser tab via `Context.NewPageAsync()`, verify the item count is 1 and quantity shows `2` — confirming cart state is shared across tabs within the same browser context |

---

## Design Decisions

- **Selector strategy:** Role-based selectors and `data-qa` attributes are preferred over CSS classes or XPath for better resilience to layout changes.
- **Ad blocking:** Route interception in `BaseTest` aborts ad network requests before each test, reducing flakiness.
- **Hard-coded test data:** Current tests use fixed credentials and user data. For improved repeatability consider externalizing this into a config file or test fixtures.

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
| Login tests fail (`TC2`, `TC4`) | The hard-coded credentials must already exist on the site; register them manually first or update the values |
| Script path not found after build | Verify build configuration and target framework match the path in the install command |

---

