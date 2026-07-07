namespace FinalProjectApp.Services;

public sealed class AtmService : IAtmService
{
    private readonly IAccountStore _accountStore;
    private readonly IAppLogger _logger;
    private readonly Dictionary<(Currency From, Currency To), decimal> _rates = new()
    {
        [(Currency.USD, Currency.GEL)] = 2.85m,
        [(Currency.EUR, Currency.GEL)] = 3.10m,
        [(Currency.GEL, Currency.USD)] = 0.35m,
        [(Currency.GEL, Currency.EUR)] = 0.32m,
        [(Currency.USD, Currency.EUR)] = 0.92m,
        [(Currency.EUR, Currency.USD)] = 1.09m
    };

    public AtmService(IAccountStore accountStore, IAppLogger logger)
    {
        _accountStore = accountStore;
        _logger = logger;
    }

    public BankAccount? FindValidAccount(string cardNumber, string cvc, string expirationDate)
    {
        return FindValidAccountAsync(cardNumber, cvc, expirationDate, CancellationToken.None)
            .GetAwaiter()
            .GetResult();
    }

    public bool VerifyPin(BankAccount account, string pinCode)
    {
        return account.PinCode == pinCode.Trim();
    }

    public OperationResult Withdraw(BankAccount account, Currency currency, decimal amount)
    {
        return WithdrawAsync(account, currency, amount, CancellationToken.None)
            .GetAwaiter()
            .GetResult();
    }

    public OperationResult Deposit(BankAccount account, Currency currency, decimal amount)
    {
        return DepositAsync(account, currency, amount, CancellationToken.None)
            .GetAwaiter()
            .GetResult();
    }

    public OperationResult ChangePin(BankAccount account, string oldPin, string newPin, string confirmPin)
    {
        return ChangePinAsync(account, oldPin, newPin, confirmPin, CancellationToken.None)
            .GetAwaiter()
            .GetResult();
    }

    public OperationResult ConvertCurrency(BankAccount account, Currency from, Currency to, decimal amount)
    {
        return ConvertCurrencyAsync(account, from, to, amount, CancellationToken.None)
            .GetAwaiter()
            .GetResult();
    }

    public void RecordTransaction(BankAccount account, TransactionType type, Currency currency, decimal amount)
    {
        var transaction = new Transaction
        {
            TransactionDate = DateTime.UtcNow,
            TransactionType = type,
            Currency = currency,
            Amount = decimal.Round(amount, 2),
            BalanceGEL = account.Balances.GEL,
            BalanceUSD = account.Balances.USD,
            BalanceEUR = account.Balances.EUR
        };
        account.AddTransaction(transaction);
    }

    // Async implementations
    public async Task<BankAccount?> FindValidAccountAsync(string cardNumber, string cvc, string expirationDate, CancellationToken ct = default)
    {
        await Task.Delay(1, ct); // Simulate async I/O
        
        var normalizedCard = NormalizeCardNumber(cardNumber);
        var normalizedExpiration = expirationDate.Trim();
        var normalizedCvc = cvc.Trim();
        
        return _accountStore.Accounts
            .Where(account => !account.CardDetails.IsExpired)
            .Select(account => new
            {
                Account = account,
                CardNumber = NormalizeCardNumber(account.CardDetails.CardNumber),
                Cvc = account.CardDetails.CVC.Trim(),
                ExpirationDate = account.CardDetails.ExpirationDate.Trim()
            })
            .Where(card => card.CardNumber == normalizedCard)
            .Where(card => card.Cvc == normalizedCvc)
            .Where(card => card.ExpirationDate == normalizedExpiration)
            .Select(card => card.Account)
            .FirstOrDefault();
    }

    public async Task<OperationResult> WithdrawAsync(BankAccount account, Currency currency, decimal amount, CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();
            
            if (!account.Balances.HasSufficientFunds(currency, amount))
            {
                _logger.Warning($"Withdraw rejected. Currency: {currency}, amount: {amount:N2}.");
                return OperationResult.Fail("Insufficient balance.");
            }
            
            account.Balances.Subtract(currency, amount);
            RecordTransaction(account, TransactionType.Withdraw, currency, -amount);
            _logger.Info($"Withdraw completed. Currency: {currency}, amount: {amount:N2}.");
            
            return OperationResult.Ok($"Please take your money: {currency} {amount:N2}");
        }
        catch (OperationCanceledException)
        {
            _logger.Warning("Withdraw operation was cancelled.");
            return OperationResult.Fail("Operation cancelled.");
        }
        catch (Exception ex)
        {
            _logger.Error("Withdraw failed.", ex);
            return OperationResult.Fail("Withdraw failed. Please try again.");
        }
    }

    public async Task<OperationResult> DepositAsync(BankAccount account, Currency currency, decimal amount, CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();
            
            account.Balances.Add(currency, amount);
            RecordTransaction(account, TransactionType.Deposit, currency, amount);
            _logger.Info($"Deposit completed. Currency: {currency}, amount: {amount:N2}.");
            
            return OperationResult.Ok($"Deposit completed: {currency} {amount:N2}");
        }
        catch (OperationCanceledException)
        {
            _logger.Warning("Deposit operation was cancelled.");
            return OperationResult.Fail("Operation cancelled.");
        }
        catch (Exception ex)
        {
            _logger.Error("Deposit failed.", ex);
            return OperationResult.Fail("Deposit failed. Please try again.");
        }
    }

    public async Task<OperationResult> ChangePinAsync(BankAccount account, string oldPin, string newPin, string confirmPin, CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();
            
            if (!VerifyPin(account, oldPin))
            {
                _logger.Warning("PIN change rejected because current PIN was wrong.");
                return OperationResult.Fail("Current PIN is incorrect.");
            }

            if (newPin != confirmPin)
            {
                return OperationResult.Fail("New PIN confirmation does not match.");
            }

            if (newPin.Length != 4 || !newPin.All(char.IsDigit))
            {
                return OperationResult.Fail("New PIN must contain exactly 4 digits.");
            }
            
            account.PinCode = newPin;
            RecordTransaction(account, TransactionType.ChangePin, Currency.GEL, 0);
            _logger.Info("PIN changed successfully.");
            
            return OperationResult.Ok("PIN changed successfully.");
        }
        catch (OperationCanceledException)
        {
            _logger.Warning("PIN change operation was cancelled.");
            return OperationResult.Fail("Operation cancelled.");
        }
        catch (Exception ex)
        {
            _logger.Error("PIN change failed.", ex);
            return OperationResult.Fail("PIN change failed. Please try again.");
        }
    }

    public async Task<OperationResult> ConvertCurrencyAsync(BankAccount account, Currency from, Currency to, decimal amount, CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();
            
            if (!_rates.TryGetValue((from, to), out var rate))
            {
                return OperationResult.Fail("Conversion rate is not available.");
            }
            
            if (!account.Balances.HasSufficientFunds(from, amount))
            {
                return OperationResult.Fail($"Insufficient {from} balance.");
            }
            
            var convertedAmount = decimal.Round(amount * rate, 2);
            account.Balances.Subtract(from, amount);
            account.Balances.Add(to, convertedAmount);
            
            RecordTransaction(account, TransactionType.CurrencyConversion, from, -amount);
            RecordTransaction(account, TransactionType.CurrencyConversion, to, convertedAmount);
            
            _logger.Info($"Converted {from} {amount:N2} to {to} {convertedAmount:N2}.");
            
            return OperationResult.Ok($"Converted {from} {amount:N2} to {to} {convertedAmount:N2}.");
        }
        catch (OperationCanceledException)
        {
            _logger.Warning("Currency conversion operation was cancelled.");
            return OperationResult.Fail("Operation cancelled.");
        }
        catch (Exception ex)
        {
            _logger.Error("Currency conversion failed.", ex);
            return OperationResult.Fail("Currency conversion failed. Please try again.");
        }
    }

    private static string NormalizeCardNumber(string cardNumber)
    {
        return string.Concat(cardNumber.Where(char.IsDigit));
    }
}