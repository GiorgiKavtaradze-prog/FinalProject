namespace FinalProjectApp.Enums;

public enum TransactionType
{
    BalanceInquiry,
    Withdraw,
    LastTransactions,
    Deposit,
    ChangePin,
    CurrencyConversion
}

public static class TransactionTypeExtensions
{
    public static string GetDescription(this TransactionType type)
    {
        return type switch
        {
            TransactionType.BalanceInquiry => "Balance Inquiry",
            TransactionType.Withdraw => "Cash Withdrawal",
            TransactionType.Deposit => "Cash Deposit",
            TransactionType.ChangePin => "PIN Change",
            TransactionType.CurrencyConversion => "Currency Conversion",
            TransactionType.LastTransactions => "Transaction History",
            _ => type.ToString()
        };
    }
}