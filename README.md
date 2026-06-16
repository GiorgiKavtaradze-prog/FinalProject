# ATM Simulator

Modern C# console application that simulates an ATM workflow with card validation, PIN verification, transactions, JSON persistence, and file logging.

## Features

- Validate card number, CVC, and expiration date
- Verify PIN code
- Close the session after failed verification
- Check account balance
- Withdraw money
- Deposit money
- Show last 5 transactions
- Change PIN code
- Convert currency between GEL, USD, and EUR
- Save all transactions in `accounts.json`
- Handle exceptions without crashing the application
- Write runtime logs to `logs/atm-log.txt`

## Test Card

```text
Card number: 1234-5678-9101-1121
CVC: 000
Expiration date: 07/28
PIN: 1234
```

## Run

```powershell
dotnet run --project .\FinalProjectApp\FinalProjectApp.csproj
```

## Build

```powershell
dotnet build
```

## Project Structure

```text
final-project/
  FinalProjectApp/
    Enums/
      Currency.cs
      TransactionType.cs
    Models/
      BankAccount.cs
      CardDetails.cs
      AccountBalances.cs
      Transaction.cs
      OperationResult.cs
    Services/
      IAtmService.cs
      AtmService.cs
    UI/
      IConsoleScreen.cs
      ConsoleScreen.cs
      AtmApplication.cs
    Logging/
      IAppLogger.cs
      FileLogger.cs
    Persistence/
      IAccountStore.cs
      JsonAccountStore.cs
      SeedData.cs
    FinalProjectApp.csproj
    Program.cs
    accounts.json
  FinalProject.slnx
  README.md
```

## Architecture

The project is separated into small components with clear responsibilities:

- `IAtmService` contains ATM business operations.
- `IAccountStore` abstracts account persistence.
- `IAppLogger` abstracts logging.
- `IConsoleScreen` keeps console-specific input and output separate from business logic.

This makes the application easier to read, test, and extend.
