using FinalProjectApp.Enums;
using FinalProjectApp.Logging;
using FinalProjectApp.Models;
using FinalProjectApp.Persistence;
using FinalProjectApp.Services;

namespace FinalProjectApp.UI;

public sealed class AtmApplication
{
    private readonly IAccountStore _accountStore;
    private readonly IAtmService _atmService;
    private readonly IAppLogger _logger;
    private readonly IConsoleScreen _screen;

    public AtmApplication(
        IAccountStore accountStore,
        IAtmService atmService,
        IAppLogger logger,
        IConsoleScreen screen)
    {
        _accountStore = accountStore;
        _atmService = atmService;
        _logger = logger;
        _screen = screen;
    }

    public void Run()
    {
        _screen.Initialize("Modern Bank ATM Simulator");

        while (true)
        {
            try
            {
                _screen.Header();
                Console.WriteLine("1. Start ATM session");
                Console.WriteLine("0. Exit");

                var choice = _screen.ReadRequired("Choose action: ");
                if (choice == "0")
                {
                    Console.WriteLine("Goodbye.");
                    return;
                }

                if (choice != "1")
                {
                    _screen.Message("Invalid menu option.", ConsoleColor.Yellow);
                    _screen.WaitForContinue();
                    continue;
                }

                StartSession();
            }
            catch (Exception ex)
            {
                _logger.Error("Unexpected application error.", ex);
                _screen.Message("Something went wrong. Please try again later.", ConsoleColor.Red);
                _screen.WaitForContinue();
            }
        }
    }

    private void StartSession()
    {
        _screen.Clear();
        _screen.Header();
        Console.WriteLine("Enter card details");
        Console.WriteLine("------------------");

        var cardNumber = _screen.ReadRequired("Card number: ");
        var cvc = _screen.ReadRequired("CVC: ");
        var expirationDate = _screen.ReadRequired("Expiration date (MM/YY): ");

        var account = _atmService.FindValidAccount(cardNumber, cvc, expirationDate);
        if (account is null)
        {
            _logger.Warning($"Failed card verification for card ending {MaskCard(cardNumber)}.");
            _screen.Message("Card information is not valid. Session closed.", ConsoleColor.Red);
            _screen.WaitForContinue();
            return;
        }

        var pin = _screen.ReadRequired("PIN code: ");
        if (!_atmService.VerifyPin(account, pin))
        {
            _logger.Warning($"Failed PIN verification for card ending {MaskCard(account.CardDetails.CardNumber)}.");
            _screen.Message("PIN code is incorrect. Session closed.", ConsoleColor.Red);
            _screen.WaitForContinue();
            return;
        }

        _logger.Info($"Successful login for {account.FirstName} {account.LastName}.");
        ShowMainMenu(account);
    }

    private void ShowMainMenu(BankAccount account)
    {
        while (true)
        {
            _screen.Clear();
            _screen.Header();
            Console.WriteLine($"Hello {account.FirstName} {account.LastName}");
            Console.WriteLine("-------------------------");
            Console.WriteLine("1. Check balance");
            Console.WriteLine("2. Withdraw amount");
            Console.WriteLine("3. Last 5 transactions");
            Console.WriteLine("4. Deposit amount");
            Console.WriteLine("5. Change PIN");
            Console.WriteLine("6. Currency conversion");
            Console.WriteLine("0. Finish session");

            var choice = _screen.ReadRequired("Choose action: ");
            var shouldEndSession = choice switch
            {
                "1" => CheckBalance(account),
                "2" => Withdraw(account),
                "3" => ShowLastTransactions(account),
                "4" => Deposit(account),
                "5" => ChangePin(account),
                "6" => ConvertCurrency(account),
                "0" => true,
                _ => InvalidMenuOption()
            };

            if (shouldEndSession)
            {
                _accountStore.SaveChanges();
                _screen.Message("Session finished. Returning to the start screen.", ConsoleColor.Green);
                _screen.WaitForContinue();
                return;
            }

            _accountStore.SaveChanges();
            _screen.WaitForContinue();
        }
    }

    private bool CheckBalance(BankAccount account)
    {
        _screen.Clear();
        _screen.Header();
        Console.WriteLine("Current balance");
        Console.WriteLine("---------------");
        _screen.Balances(account.Balances);

        _atmService.RecordTransaction(account, TransactionType.BalanceInquiry, Currency.GEL, 0);
        return false;
    }

    private bool Withdraw(BankAccount account)
    {
        _screen.Clear();
        _screen.Header();
        Console.WriteLine("Withdraw amount");
        Console.WriteLine("---------------");

        var currency = _screen.ReadCurrency();
        var amount = _screen.ReadPositiveAmount("Amount: ");
        var result = _atmService.Withdraw(account, currency, amount);

        _screen.Message(result.Message, result.Success ? ConsoleColor.Green : ConsoleColor.Red);
        return false;
    }

    private bool Deposit(BankAccount account)
    {
        _screen.Clear();
        _screen.Header();
        Console.WriteLine("Deposit amount");
        Console.WriteLine("--------------");

        var currency = _screen.ReadCurrency();
        var amount = _screen.ReadPositiveAmount("Amount: ");
        var result = _atmService.Deposit(account, currency, amount);

        _screen.Message(result.Message, result.Success ? ConsoleColor.Green : ConsoleColor.Red);
        return false;
    }

    private bool ShowLastTransactions(BankAccount account)
    {
        _screen.Clear();
        _screen.Header();
        Console.WriteLine("Last 5 transactions");
        Console.WriteLine("-------------------");

        var transactions = account.TransactionHistory
            .OrderByDescending(t => t.TransactionDate)
            .Take(5)
            .ToList();

        if (transactions.Count == 0)
        {
            Console.WriteLine("No transactions yet.");
        }
        else
        {
            foreach (var transaction in transactions)
            {
                Console.WriteLine($"{transaction.TransactionDate:yyyy-MM-dd HH:mm:ss} | {transaction.TransactionType,-18} | {transaction.Currency} {transaction.Amount:N2}");
            }
        }

        return false;
    }

    private bool ChangePin(BankAccount account)
    {
        _screen.Clear();
        _screen.Header();
        Console.WriteLine("Change PIN");
        Console.WriteLine("----------");

        var oldPin = _screen.ReadRequired("Current PIN: ");
        var newPin = _screen.ReadRequired("New PIN (4 digits): ");
        var confirmPin = _screen.ReadRequired("Confirm new PIN: ");

        var result = _atmService.ChangePin(account, oldPin, newPin, confirmPin);
        _screen.Message(result.Message, result.Success ? ConsoleColor.Green : ConsoleColor.Red);
        return false;
    }

    private bool ConvertCurrency(BankAccount account)
    {
        _screen.Clear();
        _screen.Header();
        Console.WriteLine("Currency conversion");
        Console.WriteLine("-------------------");

        Console.WriteLine("From:");
        var from = _screen.ReadCurrency();
        Console.WriteLine("To:");
        var to = _screen.ReadCurrency();

        if (from == to)
        {
            _screen.Message("Choose two different currencies.", ConsoleColor.Yellow);
            return false;
        }

        var amount = _screen.ReadPositiveAmount("Amount: ");
        var result = _atmService.ConvertCurrency(account, from, to, amount);
        _screen.Message(result.Message, result.Success ? ConsoleColor.Green : ConsoleColor.Red);
        return false;
    }

    private bool InvalidMenuOption()
    {
        _screen.Message("Invalid menu option.", ConsoleColor.Yellow);
        return false;
    }

    private static string MaskCard(string cardNumber)
    {
        var digits = new string(cardNumber.Where(char.IsDigit).ToArray());
        return digits.Length >= 4 ? digits[^4..] : "unknown";
    }
}
