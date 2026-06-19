using FinalProjectApp.Enums;
using FinalProjectApp.Models;

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
        Console.WriteLine("MODERN BANK ATM SYSTEM");
        Console.ResetColor();
        Console.WriteLine();
    }

    public void Message(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public void Balances(AccountBalances balances)
    {
        balances.AsCurrencyAmounts()
            .Select(balance => $"{balance.Currency}: {balance.Amount:N2}")
            .ToList()
            .ForEach(Console.WriteLine);
    }

    public string ReadRequired(string label)
    {
        return InputHelper.ReadRequired(label);
    }

    public decimal ReadPositiveAmount(string label)
    {
        return InputHelper.ReadPositiveDecimal(label);
    }

    public Currency ReadCurrency()
    {
        var currencies = Enum.GetValues<Currency>()
            .ToArray();

        currencies
            .Select((currency, index) => $"{index + 1}. {currency}")
            .ToList()
            .ForEach(Console.WriteLine);

        return InputHelper.ReadEnumChoice("Currency: ", currencies, "Invalid currency.");
    }

    public void WaitForContinue()
    {
        Console.WriteLine();
        Console.Write("Press Enter to continue...");
        Console.ReadLine();
    }
}
