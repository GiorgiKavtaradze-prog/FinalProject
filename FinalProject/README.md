# ATM Simulator 💳

![C# CommSchool image](../public/CommSchool.png)

---

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)](LICENSE)
[![Build Status](https://img.shields.io/badge/build-passing-brightgreen?style=for-the-badge)](https://github.com/GiorgiKavtaradze-prog/FinalProject/actions)
[![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-blue?style=for-the-badge)]()
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?style=for-the-badge&logo=docker)]()

> **Modern C# console application** (2026) that simulates an ATM workflow with card validation, PIN verification, transactions, JSON persistence, and file logging. Built with .NET 10.0 and modern development practices.

## 📚 Table of Contents

- [Features](#-features)
- [Demo](#-demo)
- [Installation](#-installation)
- [Usage](#-usage)
- [Test Card](#-test-card)
- [Project Structure](#-project-structure)
- [Architecture](#-architecture)
- [API Documentation](#-api-documentation)
- [Configuration](#-configuration)
- [Logging](#-logging)
- [Docker Support](#-docker-support)
- [CI/CD](#-cicd)
- [Contributing](#-contributing)
- [License](#-license)

## ✨ Features

- ✅ **Card Validation** - Validates card number, CVC, and expiration date
- ✅ **PIN Verification** - Secure PIN code verification with session management
- ✅ **Session Management** - Automatic session closure after failed verification attempts
- ✅ **Balance Inquiry** - Check account balance in multiple currencies
- ✅ **Withdrawals** - Withdraw money with sufficient funds validation
- ✅ **Deposits** - Deposit money into account
- ✅ **Transaction History** - View last 5 transactions
- ✅ **PIN Change** - Secure PIN code modification
- ✅ **Currency Conversion** - Convert between GEL, USD, and EUR
- ✅ **JSON Persistence** - All transactions saved to `accounts.json`
- ✅ **Exception Handling** - Graceful error handling without application crashes
- ✅ **File Logging** - Runtime logs written to `logs/atm-log.txt`
- ✅ **Docker Support** - Containerized deployment ready
- ✅ **GitHub Actions** - Automated CI/CD pipeline

## 🎮 Demo

```
=====================================
         ATM SIMULATOR v2.0
=====================================

Please enter your card number: 1234-5678-9101-1121
Please enter CVC: 000
Please enter expiration date (MM/YY): 07/28
Please enter your PIN: 1234

✅ Authentication successful!

Main Menu:
1. Check Balance
2. Withdraw
3. Deposit
4. Transaction History
5. Change PIN
6. Currency Converter
7. Exit

Select an option: 1

Current Balance:
- GEL: 1000.00
- USD: 350.00
- EUR: 300.00
```

## 📦 Installation

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download) or later
- A compatible IDE (Visual Studio, VS Code, or Rider)
- Docker (optional, for containerized deployment)

### Clone the Repository

```bash
git clone https://github.com/GiorgiKavtaradze-prog/FinalProject.git
cd FinalProject
```

### Build the Project

```powershell
dotnet build
```

## 🚀 Usage

### Running the Application

```powershell
dotnet run --project .\FinalProjectApp\FinalProjectApp.csproj
```

### Building for Production

```powershell
dotnet publish -c Release -o ./publish
```

### Docker Deployment

```bash
docker build -t atm-simulator .
docker run -it atm-simulator
```

## 🧪 Test Card

Use the following test credentials to explore the application:

```text
┌─────────────────────────────────────┐
│  Card number: 1234-5678-9101-1121  │
│  CVC:         000                  │
│  Expiration:   07/28                │
│  PIN:          1234                 │
└─────────────────────────────────────┘
```

## 📁 Project Structure

```text
FinalProject/
├── FinalProjectApp/
│   ├── Enums/
│   │   ├── Currency.cs          # Supported currencies (GEL, USD, EUR)
│   │   └── TransactionType.cs   # Transaction types enum
│   ├── Models/
│   │   ├── BankAccount.cs       # Main account model
│   │   ├── CardDetails.cs       # Card information model
│   │   ├── AccountBalances.cs   # Multi-currency balance model
│   │   ├── Transaction.cs       # Transaction record model
│   │   └── OperationResult.cs   # Operation result wrapper
│   ├── Services/
│   │   ├── IAtmService.cs       # ATM service interface
│   │   └── AtmService.cs        # ATM business logic implementation
│   ├── UI/
│   │   ├── IConsoleScreen.cs    # Console UI interface
│   │   ├── ConsoleScreen.cs     # Console UI implementation
│   │   └── AtmApplication.cs    # Main application orchestrator
│   ├── Logging/
│   │   ├── IAppLogger.cs        # Logger interface
│   │   └── FileLogger.cs        # File-based logger implementation
│   ├── Persistence/
│   │   ├── IAccountStore.cs     # Account store interface
│   │   ├── JsonAccountStore.cs  # JSON file persistence
│   │   └── SeedData.cs          # Initial data seeding
│   ├── FinalProjectApp.csproj   # Project configuration
│   ├── Program.cs               # Application entry point
│   └── accounts.json            # Account data storage
├── .github/
│   └── workflows/
│       └── ci.yml               # GitHub Actions CI/CD
├── Dockerfile                   # Docker configuration
├── FinalProject.slnx            # Solution file
├── README.md                    # This file
└── .gitignore                   # Git ignore rules
```

## 🏗️ Architecture

The project follows **Clean Architecture** principles with clear separation of concerns:

```
┌─────────────────────────────────────────────────────────┐
│                    Presentation Layer                     │
│  ┌───────────────────────────────────────────────────┐  │
│  │              AtmApplication.cs                    │  │
│  └───────────────────────────────────────────────────┘  │
├─────────────────────────────────────────────────────────┤
│                     Service Layer                       │
│  ┌───────────────────────────────────────────────────┐  │
│  │              IAtmService.cs                       │  │
│  │              AtmService.cs                          │  │
│  └───────────────────────────────────────────────────┘  │
├─────────────────────────────────────────────────────────┤
│                  Infrastructure Layer                     │
│  ┌───────────────────┐  ┌──────────────────────────┐  │
│  │  IAccountStore.cs  │  │      IAppLogger.cs       │  │
│  │ JsonAccountStore.cs│  │      FileLogger.cs       │  │
│  └───────────────────┘  └──────────────────────────┘  │
└─────────────────────────────────────────────────────────┘
```

### Design Patterns Used

- **Dependency Injection** - Loose coupling between components
- **Repository Pattern** - Abstracted data persistence
- **Strategy Pattern** - Flexible logging and storage implementations
- **Result Pattern** - Type-safe operation results

## 📖 API Documentation

### IAtmService

Main service interface for ATM operations:

| Method | Description |
|--------|-------------|
| `ValidateCard(CardDetails card)` | Validates card information |
| `VerifyPin(string cardNumber, string pin)` | Verifies PIN for a card |
| `CheckBalance(string cardNumber)` | Returns account balances |
| `Withdraw(string cardNumber, decimal amount, Currency currency)` | Withdraws money |
| `Deposit(string cardNumber, decimal amount, Currency currency)` | Deposits money |
| `GetTransactionHistory(string cardNumber)` | Returns last 5 transactions |
| `ChangePin(string cardNumber, string oldPin, string newPin)` | Changes PIN |
| `ConvertCurrency(decimal amount, Currency from, Currency to)` | Currency conversion |

## ⚙️ Configuration

### accounts.json

The application uses a JSON file for data persistence:

```json
{
  "accounts": [
    {
      "cardNumber": "1234-5678-9101-1121",
      "cvc": "000",
      "expirationDate": "07/28",
      "pin": "1234",
      "balances": {
        "gel": 1000.00,
        "usd": 350.00,
        "eur": 300.00
      },
      "transactions": []
    }
  ]
}
```

## 📝 Logging

All application events are logged to `logs/atm-log.txt`:

```
[2026-07-05 13:00:00] INFO: Application started
[2026-07-05 13:00:15] INFO: Card validated: 1234-5678-9101-1121
[2026-07-05 13:00:20] INFO: PIN verified for card: 1234-5678-9101-1121
[2026-07-05 13:00:30] INFO: Withdrawal: 100 GEL from card 1234-5678-9101-1121
```

## 🐳 Docker Support

The application includes Docker support for easy deployment:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FinalProjectApp.dll"]
```

## 🔄 CI/CD

GitHub Actions workflow automatically builds and tests the application on every push.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

<div align="center">
  <sub>Built with ❤️ by Giorgi Kavtaradze | Updated 2026</sub>
</div>