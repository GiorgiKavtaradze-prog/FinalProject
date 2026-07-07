using System.Text.Json;
using FinalProjectApp.Configuration;

AtmConfiguration config = LoadConfiguration();

IAccountStore accountStore = new JsonAccountStore(config.AccountsFilePath);
IAppLogger logger = new FileLogger(config.LogFilePath);
IAtmService atmService = new AtmService(accountStore, logger);
IConsoleScreen consoleScreen = new ConsoleScreen();

AtmApplication app = new AtmApplication(accountStore, atmService, logger, consoleScreen);
app.Run();

return;

static AtmConfiguration LoadConfiguration()
{
    string configPath = Path.Combine(AppContext.BaseDirectory, "atm-config.json");
    
    if (File.Exists(configPath))
    {
        try
        {
            string json = File.ReadAllText(configPath);
            return JsonSerializer.Deserialize<AtmConfiguration>(json) ?? new AtmConfiguration();
        }
        catch
        {
            return new AtmConfiguration();
        }
    }
    
    return new AtmConfiguration();
}