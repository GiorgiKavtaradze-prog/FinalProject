namespace FinalProjectApp.Configuration;

public sealed record AtmConfiguration
{
    public string AccountsFilePath { get; init; } = "accounts.json";
    public string LogFilePath { get; init; } = "logs/atm-log.txt";
    public string AppTitle { get; init; } = "Modern Bank ATM Simulator v2.0";
    public int MaxLoginAttempts { get; init; } = 3;
    public decimal MinWithdrawalAmount { get; init; } = 1m;
    public decimal MaxWithdrawalAmount { get; init; } = 10000m;
}