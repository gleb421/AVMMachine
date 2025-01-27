using Lab5.Domain.Aggregates;
using Lab5.Domain.Models;

namespace Lab5.Application.Abstractions.Repositories;

public interface IBankAccountRepository
{
    void Save(BankAccount account);

    BankAccount? GetByAccountId(Guid accountId);

    void Update(BankAccount account, Money amount, string transactionDescription);
}