namespace FinalProjectApp.Models;

public sealed class BankAccount
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public CardDetails CardDetails { get; set; } = new();
    public string PinCode { get; set; } = string.Empty;
    public AccountBalances Balances { get; set; } = new();
    public List<Transaction> TransactionHistory { get; set; } = [];
}
