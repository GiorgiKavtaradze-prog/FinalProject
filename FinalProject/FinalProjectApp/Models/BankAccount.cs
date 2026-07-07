namespace FinalProjectApp.Models;

public sealed class BankAccount
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public CardDetails CardDetails { get; set; } = new();
    public string PinCode { get; set; } = string.Empty;
    public AccountBalances Balances { get; set; } = new();
    public List<Transaction> TransactionHistory { get; set; } = [];

    public string FullName => $"{FirstName} {LastName}";
    public string MaskedCardNumber => MaskCard(CardDetails.CardNumber);

    public void AddTransaction(Transaction transaction)
    {
        TransactionHistory.Add(transaction);
    }

    public IEnumerable<Transaction> GetRecentTransactions(int count = 5)
    {
        return TransactionHistory
            .OrderByDescending(t => t.TransactionDate)
            .Take(count);
    }

    private static string MaskCard(string cardNumber)
    {
        var digits = string.Concat(cardNumber.Where(char.IsDigit));
        return digits.Length >= 4 ? $"****-****-****-{digits[^4..]}" : "****";
    }
}