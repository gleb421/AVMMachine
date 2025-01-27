using Lab5.Domain.Aggregates;
using Lab5.Domain.Models;

namespace Lab5.Infrastucture.Mappers.BankAccounts;

public interface IBankAccountDataMapper
{
    void Save(BankAccount account);

    BankAccount? GetByAccountId(Guid accountId);

    void Update(BankAccount account, Money amount, string transactionDescription);
}