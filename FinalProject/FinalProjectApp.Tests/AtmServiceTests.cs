using Xunit;

namespace FinalProjectApp.Tests;

public class AtmServiceTests
{
    private readonly IAccountStore _accountStore;
    private readonly IAppLogger _logger;
    private readonly IAtmService _atmService;

    public AtmServiceTests()
    {
        _accountStore = new InMemoryAccountStore();
        _logger = new MockLogger();
        _atmService = new AtmService(_accountStore, _logger);
    }

    [Fact]
    public async Task WithdrawAsync_ReturnsSuccess_WhenSufficientFunds()
    {
        var account = CreateTestAccount();
        _accountStore.Accounts.Add(account);

        var result = await _atmService.WithdrawAsync(account, Currency.GEL, 100m);

        Assert.True(result.Success);
        Assert.Equal(900m, account.Balances.GEL);
    }

    [Fact]
    public async Task WithdrawAsync_ReturnsFailure_WhenInsufficientFunds()
    {
        var account = CreateTestAccount();
        _accountStore.Accounts.Add(account);

        var result = await _atmService.WithdrawAsync(account, Currency.GEL, 2000m);

        Assert.False(result.Success);
        Assert.Equal(1000m, account.Balances.GEL);
    }

    [Fact]
    public async Task DepositAsync_IncreasesBalance()
    {
        var account = CreateTestAccount();
        _accountStore.Accounts.Add(account);

        var result = await _atmService.DepositAsync(account, Currency.USD, 500m);

        Assert.True(result.Success);
        Assert.Equal(750m, account.Balances.USD);
    }

    [Fact]
    public async Task ChangePinAsync_ReturnsSuccess_WhenValid()
    {
        var account = CreateTestAccount();
        _accountStore.Accounts.Add(account);

        var result = await _atmService.ChangePinAsync(account, "1234", "5678", "5678");

        Assert.True(result.Success);
        Assert.Equal("5678", account.PinCode);
    }

    [Fact]
    public async Task ChangePinAsync_ReturnsFailure_WhenPinsDontMatch()
    {
        var account = CreateTestAccount();
        _accountStore.Accounts.Add(account);

        var result = await _atmService.ChangePinAsync(account, "1234", "5678", "9999");

        Assert.False(result.Success);
        Assert.Equal("1234", account.PinCode);
    }

    [Fact]
    public async Task ConvertCurrencyAsync_ConvertsCorrectly()
    {
        var account = CreateTestAccount();
        _accountStore.Accounts.Add(account);

        var result = await _atmService.ConvertCurrencyAsync(account, Currency.USD, Currency.GEL, 100m);

        Assert.True(result.Success);
        Assert.Equal(150m, account.Balances.USD); // 250 - 100
        Assert.Equal(1285m, account.Balances.GEL); // 1000 + 285
    }

    private static BankAccount CreateTestAccount()
    {
        return new BankAccount
        {
            FirstName = "Test",
            LastName = "User",
            CardDetails = new CardDetails
            {
                CardNumber = "1234-5678-9101-1121",
                ExpirationDate = "12/30",
                CVC = "123"
            },
            PinCode = "1234",
            Balances = new AccountBalances
            {
                GEL = 1000m,
                USD = 250m,
                EUR = 100m
            }
        };
    }
}

// Test helpers
internal sealed class InMemoryAccountStore : IAccountStore
{
    public List<BankAccount> Accounts { get; } = [];
    public void SaveChanges() { }
}

internal sealed class MockLogger : IAppLogger
{
    public void Info(string message) { }
    public void Warning(string message) { }
    public void Error(string message, Exception exception) { }
}