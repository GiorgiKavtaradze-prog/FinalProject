using FinalProjectApp.Logging;
using FinalProjectApp.Persistence;
using FinalProjectApp.Services;
using FinalProjectApp.UI;

IAccountStore accountStore = new JsonAccountStore("accounts.json");
IAppLogger logger = new FileLogger("logs/atm-log.txt");
IAtmService atmService = new AtmService(accountStore, logger);

var app = new AtmApplication(accountStore, atmService, logger, new ConsoleScreen());
app.Run();
