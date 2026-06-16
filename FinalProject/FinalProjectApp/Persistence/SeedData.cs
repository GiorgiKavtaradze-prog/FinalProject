using FinalProjectApp.Enums;
using FinalProjectApp.Models;

namespace FinalProjectApp.Persistence;

public static class SeedData
{
    public static List<BankAccount> CreateAccounts()
    {
        return
        [
            new BankAccount
            {
                FirstName = "John",
                LastName = "Doe",
                CardDetails = new CardDetails
                {
                    CardNumber = "1234-5678-9101-1121",
                    ExpirationDate = "07/28",
                    CVC = "000"
                },
                PinCode = "1234",
                Balances = new AccountBalances
                {
                    GEL = 1000,
                    USD = 250,
                    EUR = 100
                },
                TransactionHistory =
                [
                    new Transaction
                    {
                        TransactionDate = new DateTime(2024, 1, 3, 10, 15, 30, DateTimeKind.Utc),
                        TransactionType = TransactionType.ChangePin,
                        Currency = Currency.GEL,
                        Amount = 0,
                        BalanceGEL = 1000,
                        BalanceUSD = 250,
                        BalanceEUR = 100
                    },
                    new Transaction
                    {
                        TransactionDate = new DateTime(2024, 1, 2, 15, 45, 0, DateTimeKind.Utc),
                        TransactionType = TransactionType.Deposit,
                        Currency = Currency.GEL,
                        Amount = 100,
                        BalanceGEL = 1000,
                        BalanceUSD = 250,
                        BalanceEUR = 100
                    },
                    new Transaction
                    {
                        TransactionDate = new DateTime(2024, 1, 1, 8, 20, 45, DateTimeKind.Utc),
                        TransactionType = TransactionType.Withdraw,
                        Currency = Currency.GEL,
                        Amount = -500,
                        BalanceGEL = 900,
                        BalanceUSD = 250,
                        BalanceEUR = 100
                    }
                ]
            }
        ];
    }
}
