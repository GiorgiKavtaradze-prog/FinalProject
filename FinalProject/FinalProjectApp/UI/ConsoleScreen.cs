using System.Globalization;
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
        while (true)
        {
            Console.Write(label);
            var input = Console.ReadLine()?.Trim() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            Message("Value is required.", ConsoleColor.Yellow);
        }
    }

    public decimal ReadPositiveAmount(string label)
    {
        while (true)
        {
            var input = ReadRequired(label);
            if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out var amount) && amount > 0)
            {
                return decimal.Round(amount, 2);
            }

            Message("Please enter a positive numeric amount. Example: 100.50", ConsoleColor.Yellow);
        }
    }

    public Currency ReadCurrency()
    {
        var currencies = Enum.GetValues<Currency>()
            .Select((currency, index) => new { Number = index + 1, Currency = currency })
            .ToArray();

        while (true)
        {
            currencies
                .Select(option => $"{option.Number}. {option.Currency}")
                .ToList()
                .ForEach(Console.WriteLine);

            var choice = ReadRequired("Currency: ");

            var selectedCurrency = currencies
                .Where(option => option.Number.ToString(CultureInfo.InvariantCulture) == choice)
                .Select(option => (Currency?)option.Currency)
                .SingleOrDefault();

            if (selectedCurrency is not null)
            {
                return selectedCurrency.Value;
            }

            Message("Invalid currency.", ConsoleColor.Yellow);
        }
    }

    public void WaitForContinue()
    {
        Console.WriteLine();
        Console.Write("Press Enter to continue...");
        Console.ReadLine();
    }
}
