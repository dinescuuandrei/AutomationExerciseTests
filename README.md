# AutomationExerciseTests

A polished, end-to-end test automation suite using Microsoft Playwright and NUnit to validate workflows on the Automation Exercise demo site: https://automationexercise.com/.

Key areas covered:
- User registration, login, and account lifecycle
- Contact form submission
- Product browsing, search and details
- Shopping cart operations and calculations
- Newsletter subscription

---

## Quick facts

- Language: C# 14.0
- Target framework: .NET 10
- Test runner: NUnit with `Microsoft.Playwright.NUnit` (`PageTest` base)
- Browser automation: Microsoft Playwright

---

## Getting started

Prerequisites
- .NET 10 SDK
- PowerShell (Windows) or a POSIX shell (macOS / Linux)
- Git

Local setup
1. Clone the repository
   ```powershell
   git clone https://github.com/dinescuuandrei/AutomationExerciseTests.git
   cd AutomationExerciseTests
   ```

2. Restore packages
   ```powershell
   dotnet restore
   ```

3. Build the project
   ```powershell
   dotnet build
   ```

4. Install Playwright browsers
   - Recommended (from build output script):
     ```powershell
     pwsh bin/Debug/net10/playwright.ps1 install
     ```
   - Or install the Playwright CLI globally and run the installer:
     ```powershell
     dotnet tool install --global Microsoft.Playwright.CLI
     playwright install
     ```

Note: the generated script path can vary with build configuration and target framework.

---

## Running tests

Run all tests
```powershell
dotnet test
```

Run a specific test by name or fully qualified name
```powershell
# by NUnit test name (replace TC1 with the test name)
dotnet test --filter "TestName=TC1"

# by fully qualified name
dotnet test --filter "FullyQualifiedName=AutomationExerciseTests.Tests.TC1"
```

Run in headed/debug mode
```powershell
# Windows PowerShell
$env:PWDEBUG = "1"; dotnet test

# Bash/macOS
PWDEBUG=1 dotnet test
```

Change logging/verbosity
```powershell
dotnet test --verbosity detailed
```

---

## Tests overview

The main test file is `UnitTest1.cs` and uses `Playwright.NUnit.PageTest`. Tests include (high level):

- `TC1` - Full signup, login verification and account deletion
- `TC2` - Login with existing credentials
- `TC4` - Login and logout
- `TC5` - Signup with an existing email (validation)
- `TC6` - Contact form (with dialog handling)
- `TC7` - Navigation to the test cases page
- `TC8` - Product details page verification
- `TC9` - Product search flow
- `TC10` - Newsletter subscription
- `TC12` - Add multiple products to cart and verify count
- `TC_BONUS` - Cart price calculations and persistence after refresh

For exact selectors and implementation details, open `UnitTest1.cs`.

---

## Configuration & tips

- Consent & ads: The test suite routes traffic and aborts requests to common ad domains to improve reliability.
- Selectors: Tests prefer role-based and `data-qa` selectors where available for stability.
- Test data: Tests currently use hard-coded test data. Consider extracting test data into a configuration or fixtures for repeatability.

Common troubleshooting
- If tests fail because elements are not found, try running in headed mode (`PWDEBUG=1`) to observe browser behavior.
- If Playwright cannot find browsers, re-run the Playwright installer step.

---

## Continuous Integration (example)

You can run the tests in GitHub Actions. Basic steps:
- use a runner with .NET 10 installed
- restore, build, install Playwright browsers, and run `dotnet test`

Example snippet (workflow):
```yaml
name: .NET Playwright Tests
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
      - name: Restore
        run: dotnet restore
      - name: Build
        run: dotnet build --configuration Release
      - name: Install Playwright browsers
        run: pwsh -c "pwsh ./bin/Release/net10/playwright.ps1 install"
      - name: Run tests
        run: dotnet test --configuration Release --verbosity minimal
```

Adjust paths and configuration as needed for your build matrix.

---

## Contributing

Contributions, issue reports, and improvements are welcome. Please open issues or PRs on the repository.

Guidelines
- Keep tests deterministic where possible
- Avoid fragile selectors; prefer `data-qa` or role-based selectors

---

## License

This repository is provided for educational purposes. Add a license file if you plan to publish or share widely.

---

If you'd like, I can also:
- add a GitHub Actions workflow file to the repo
- move test data into a configuration file
- add a short `CONTRIBUTING.md` or `ISSUE_TEMPLATE`

Tell me which of the above you want next and I will add it.