using Lab5.Application.Abstractions.Repositories;
using Lab5.Domain.Aggregates;
using Lab5.Domain.Models;
using Lab5.Infrastucture.Mappers.BankAccounts;

namespace Lab5.Infrastucture.Repositories;

public class BankAccountRepository : IBankAccountRepository
{
    private readonly IBankAccountDataMapper _dataMapper;

    public BankAccountRepository(IBankAccountDataMapper dataMapper)
    {
        _dataMapper = dataMapper;
    }

    public void Save(BankAccount account)
    {
        _dataMapper.Save(account);
    }

    public BankAccount? GetByAccountId(Guid accountId)
    {
        return _dataMapper.GetByAccountId(accountId);
    }

    public void Update(BankAccount account, Money amount, string transactionDescription)
    {
        _dataMapper.Update(account, amount, transactionDescription);
    }
}