using Lab5.Domain.Enteties;

namespace Lab5.Infrastucture.Mappers.Transactions;

public interface ITransactionDataMapper
{
    void Save(Transaction transaction, Guid accountId);

    Transaction? GetByTransactionId(Guid transactionId);
}