using Lab5.Application.Abstractions.Repositories;
using Lab5.Domain.Enteties;
using Lab5.Infrastucture.Mappers.Transactions;

namespace Lab5.Infrastucture.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly ITransactionDataMapper _transactionDataMapper;

    public TransactionRepository(ITransactionDataMapper transactionDataMapper)
    {
        _transactionDataMapper = transactionDataMapper;
    }

    public void Save(Transaction transaction, Guid accountId)
    {
        _transactionDataMapper.Save(transaction, accountId);
    }

    public Transaction? GetByTransactionId(Guid transactionId)
    {
        return _transactionDataMapper.GetByTransactionId(transactionId);
    }
}