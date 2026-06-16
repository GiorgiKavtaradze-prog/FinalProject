using System.Text.Json;
using System.Text.Json.Serialization;
using FinalProjectApp.Models;

namespace FinalProjectApp.Persistence;

public sealed class JsonAccountStore : IAccountStore
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public JsonAccountStore(string filePath)
    {
        _filePath = filePath;
        Accounts = Load();
    }

    public List<BankAccount> Accounts { get; }

    public void SaveChanges()
    {
        var json = JsonSerializer.Serialize(Accounts, _jsonOptions);
        File.WriteAllText(_filePath, json);
    }

    private List<BankAccount> Load()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                var seed = SeedData.CreateAccounts();
                File.WriteAllText(_filePath, JsonSerializer.Serialize(seed, _jsonOptions));
                return seed;
            }

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<BankAccount>>(json, _jsonOptions) ?? SeedData.CreateAccounts();
        }
        catch
        {
            return SeedData.CreateAccounts();
        }
    }
}
