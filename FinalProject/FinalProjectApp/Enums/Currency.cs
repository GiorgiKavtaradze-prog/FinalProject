namespace FinalProjectApp.Enums;

public enum Currency
{
    GEL,
    USD,
    EUR
}

public static class CurrencyExtensions
{
    public static string GetSymbol(this Currency currency)
    {
        return currency switch
        {
            Currency.GEL => "₾",
            Currency.USD => "$",
            Currency.EUR => "€",
            _ => currency.ToString()
        };
    }
    
    public static string GetDescription(this Currency currency)
    {
        return currency switch
        {
            Currency.GEL => "Georgian Lari",
            Currency.USD => "US Dollar",
            Currency.EUR => "Euro",
            _ => currency.ToString()
        };
    }
}