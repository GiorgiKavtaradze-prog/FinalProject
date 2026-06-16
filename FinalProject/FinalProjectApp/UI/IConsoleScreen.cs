using FinalProjectApp.Enums;
using FinalProjectApp.Models;

namespace FinalProjectApp.UI;

public interface IConsoleScreen
{
    void Initialize(string title);
    void Clear();
    void Header();
    void Message(string message, ConsoleColor color);
    void Balances(AccountBalances balances);
    string ReadRequired(string label);
    decimal ReadPositiveAmount(string label);
    Currency ReadCurrency();
    void WaitForContinue();
}
