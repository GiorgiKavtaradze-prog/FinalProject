using FinalProjectApp.Enums;

namespace FinalProjectApp.Models;

public sealed class Transaction
{
    public DateTime TransactionDate { get; set; }
    public TransactionType TransactionType { get; set; }
    public Currency Currency { get; set; }
    public decimal Amount { get; set; }
    public decimal BalanceGEL { get; set; }
    public decimal BalanceUSD { get; set; }
    public decimal BalanceEUR { get; set; }
}
