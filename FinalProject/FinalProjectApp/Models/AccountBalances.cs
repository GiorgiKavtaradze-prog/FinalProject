using FinalProjectApp.Enums;

namespace FinalProjectApp.Models;

public sealed class AccountBalances
{
    public decimal GEL { get; set; }
    public decimal USD { get; set; }
    public decimal EUR { get; set; }

    public decimal Get(Currency currency)
    {
        return currency switch
        {
            Currency.GEL => GEL,
            Currency.USD => USD,
            Currency.EUR => EUR,
            _ => throw new ArgumentOutOfRangeException(nameof(currency), currency, null)
        };
    }

    public void Set(Currency currency, decimal value)
    {
        switch (currency)
        {
            case Currency.GEL:
                GEL = decimal.Round(value, 2);
                break;
            case Currency.USD:
                USD = decimal.Round(value, 2);
                break;
            case Currency.EUR:
                EUR = decimal.Round(value, 2);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(currency), currency, null);
        }
    }
}
