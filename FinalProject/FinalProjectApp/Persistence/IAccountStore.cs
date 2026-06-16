using FinalProjectApp.Models;

namespace FinalProjectApp.Persistence;

public interface IAccountStore
{
    List<BankAccount> Accounts { get; }
    void SaveChanges();
}
