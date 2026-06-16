using FinalProjectApp.Enums;
using FinalProjectApp.Models;

namespace FinalProjectApp.Services;

public interface IAtmService
{
    BankAccount? FindValidAccount(string cardNumber, string cvc, string expirationDate);
    bool VerifyPin(BankAccount account, string pinCode);
    OperationResult Withdraw(BankAccount account, Currency currency, decimal amount);
    OperationResult Deposit(BankAccount account, Currency currency, decimal amount);
    OperationResult ChangePin(BankAccount account, string oldPin, string newPin, string confirmPin);
    OperationResult ConvertCurrency(BankAccount account, Currency from, Currency to, decimal amount);
    void RecordTransaction(BankAccount account, TransactionType type, Currency currency, decimal amount);
}
