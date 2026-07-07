namespace FinalProjectApp.Models;

public sealed class CardDetails
{
    public string CardNumber { get; set; } = string.Empty;
    public string ExpirationDate { get; set; } = string.Empty;
    public string CVC { get; set; } = string.Empty;

    public bool IsExpired => !IsValidExpiration();
    public bool IsValid => ValidateCard();

    private bool IsValidExpiration()
    {
        if (!DateTime.TryParseExact(ExpirationDate, "MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
        {
            return false;
        }
        var expiresAt = new DateTime(parsed.Year, parsed.Month, DateTime.DaysInMonth(parsed.Year, parsed.Month), 23, 59, 59);
        return expiresAt >= DateTime.Today;
    }

    private bool ValidateCard()
    {
        return !string.IsNullOrWhiteSpace(CardNumber) &&
               !string.IsNullOrWhiteSpace(ExpirationDate) &&
               !string.IsNullOrWhiteSpace(CVC) &&
               CardNumber.Length >= 13 &&
               CVC.Length is 3 or 4 &&
               IsValidExpiration();
    }
}
