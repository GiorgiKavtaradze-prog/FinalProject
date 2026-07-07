using Xunit;

namespace FinalProjectApp.Tests;

public class AccountBalancesTests
{
    [Fact]
    public void Get_ReturnsCorrectValue_ForGEL()
    {
        var balances = new AccountBalances { GEL = 1000m };
        var result = balances.Get(Currency.GEL);
        Assert.Equal(1000m, result);
    }

    [Fact]
    public void Set_UpdatesCorrectValue_ForUSD()
    {
        var balances = new AccountBalances();
        balances.Set(Currency.USD, 500m);
        Assert.Equal(500m, balances.USD);
    }

    [Fact]
    public void Add_IncreasesBalance()
    {
        var balances = new AccountBalances { EUR = 100m };
        balances.Add(Currency.EUR, 50m);
        Assert.Equal(150m, balances.EUR);
    }

    [Fact]
    public void Subtract_DecreasesBalance()
    {
        var balances = new AccountBalances { GEL = 1000m };
        balances.Subtract(Currency.GEL, 250m);
        Assert.Equal(750m, balances.GEL);
    }

    [Fact]
    public void HasSufficientFunds_ReturnsTrue_WhenBalanceIsSufficient()
    {
        var balances = new AccountBalances { USD = 1000m };
        var result = balances.HasSufficientFunds(Currency.USD, 500m);
        Assert.True(result);
    }

    [Fact]
    public void HasSufficientFunds_ReturnsFalse_WhenBalanceIsInsufficient()
    {
        var balances = new AccountBalances { EUR = 100m };
        var result = balances.HasSufficientFunds(Currency.EUR, 200m);
        Assert.False(result);
    }

    [Fact]
    public void AsCurrencyAmounts_ReturnsAllCurrencies()
    {
        var balances = new AccountBalances { GEL = 100m, USD = 200m, EUR = 300m };
        var amounts = balances.AsCurrencyAmounts().ToList();
        
        Assert.Equal(3, amounts.Count);
        Assert.Contains(amounts, a => a.Currency == Currency.GEL && a.Amount == 100m);
        Assert.Contains(amounts, a => a.Currency == Currency.USD && a.Amount == 200m);
        Assert.Contains(amounts, a => a.Currency == Currency.EUR && a.Amount == 300m);
    }
}