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
        Console.WriteLine($"GEL: {balances.GEL:N2}");
        Console.WriteLine($"USD: {balances.USD:N2}");
        Console.WriteLine($"EUR: {balances.EUR:N2}");
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
        while (true)
        {
            Console.WriteLine("1. GEL");
            Console.WriteLine("2. USD");
            Console.WriteLine("3. EUR");
            var choice = ReadRequired("Currency: ");

            if (choice == "1") return Currency.GEL;
            if (choice == "2") return Currency.USD;
            if (choice == "3") return Currency.EUR;

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
