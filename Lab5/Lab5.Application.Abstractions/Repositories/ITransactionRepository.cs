using Lab5.Domain.Enteties;

namespace Lab5.Application.Abstractions.Repositories;

public interface ITransactionRepository
{
    void Save(Transaction transaction, Guid accountId);

    Transaction? GetByTransactionId(Guid transactionId);
}