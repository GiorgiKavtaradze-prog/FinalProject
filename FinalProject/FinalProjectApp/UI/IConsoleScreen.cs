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
    
    Task InitializeAsync(string title, CancellationToken ct = default);
    Task<string> ReadRequiredAsync(string label, CancellationToken ct = default);
    Task<decimal> ReadPositiveAmountAsync(string label, CancellationToken ct = default);
    Task<Currency> ReadCurrencyAsync(CancellationToken ct = default);
    Task WaitForContinueAsync(CancellationToken ct = default);
}
