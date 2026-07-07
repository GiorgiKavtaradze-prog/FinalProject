namespace FinalProjectApp.UI;

public sealed class ConsoleScreen : IConsoleScreen
{
    public void Initialize(string title)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        try
        {
            Console.Title = title;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting console title: {ex.Message}");
        }
    }

    public Task InitializeAsync(string title, CancellationToken ct = default)
    {
        Initialize(title);
        return Task.CompletedTask;
    }

    public void Clear()
    {
        try
        {
            if (!Console.IsOutputRedirected)
            {
                Console.Clear();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error clearing console screen: {ex.Message}");
        }
    }

    public void Header()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║           🏦 MODERN BANK ATM SYSTEM v2.0                     ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();
    }

    public void Message(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine($"  → {message}");
        Console.ResetColor();
    }

    public void Balances(AccountBalances balances)
    {
        Console.WriteLine();
        Console.WriteLine("  Current Balances:");
        
        foreach (var (currency, amount) in balances.AsCurrencyAmounts())
        {
            var currencySymbol = GetCurrencySymbol(currency);
            Console.WriteLine($"  {currency,-3} {currencySymbol,-2} {amount,15:N2}");
        }
       
    }

    public string ReadRequired(string label)
    {
        return InputHelper.ReadRequired(label);
    }

    public Task<string> ReadRequiredAsync(string label, CancellationToken ct = default)
    {
        return Task.FromResult(ReadRequired(label));
    }

    public decimal ReadPositiveAmount(string label)
    {
        return InputHelper.ReadPositiveDecimal(label);
    }

    public Task<decimal> ReadPositiveAmountAsync(string label, CancellationToken ct = default)
    {
        return Task.FromResult(ReadPositiveAmount(label));
    }

    public Currency ReadCurrency()
    {
        var currencies = Enum.GetValues<Currency>()
            .ToArray();
        
        Console.WriteLine();
        Console.WriteLine("  Select Currency:");
        
        for (int i = 0; i < currencies.Length; i++)
        {
            var symbol = GetCurrencySymbol(currencies[i]);
            Console.WriteLine($"  {i + 1}. {currencies[i]} ({symbol})");
        }
       
        
        return InputHelper.ReadEnumChoice("  Currency: ", currencies, "  Invalid currency selection.");
    }

    public Task<Currency> ReadCurrencyAsync(CancellationToken ct = default)
    {
        return Task.FromResult(ReadCurrency());
    }

    public void WaitForContinue()
    {
        Console.WriteLine();
        Console.Write("  Press Enter to continue...");
        Console.ReadLine();
    }

    public Task WaitForContinueAsync(CancellationToken ct = default)
    {
        WaitForContinue();
        return Task.CompletedTask;
    }

    private static string GetCurrencySymbol(Currency currency)
    {
        return currency switch
        {
            Currency.GEL => "₾",
            Currency.USD => "$",
            Currency.EUR => "€",
            _ => currency.ToString()
        };
    }
}
