using Lab5.Application.Abstractions.Repositories;
using Lab5.Application.Contracts.Users;
using Lab5.Domain.Aggregates;
using Lab5.Domain.Enteties;
using Lab5.Domain.Models;
using Lab5.Domain.ValueObjects;

namespace Lab5.Application.Users;

internal class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly CurrentUserManager _currentUserManager;
    private readonly IBankAccountRepository _bankAccountRepository;

    public UserService(
        IUserRepository userRepository,
        CurrentUserManager currentUserManager,
        IBankAccountRepository bankAccountRepository)
    {
        _userRepository = userRepository;
        _currentUserManager = currentUserManager;
        _bankAccountRepository = bankAccountRepository;
    }

    public LoginResult Login(string accountNumber, PinCode pinCode)
    {
        User? user =
            _userRepository.FindByAccountNumber(accountNumber);

        if (user == null || user.Account.PinCode.Pin != pinCode.Pin || user.Account.AccountNumber != accountNumber)
        {
            return new LoginResult.NotFound();
        }

        _currentUserManager.User = user;

        return new LoginResult.Success();
    }

    public Money Withdraw(Money amount)
    {
        if (_currentUserManager.User == null)
        {
            throw new InvalidOperationException("User is not logged in.");
        }

        User user = _currentUserManager.User;

        user.Account.Withdraw(amount);
        _bankAccountRepository.Update(user.Account, user.Account.Balance, "Withdraw");

        return user.Account.Balance;
    }

    public Money Deposit(Money amount)
    {
        if (_currentUserManager.User == null)
        {
            throw new InvalidOperationException("User is not logged in.");
        }

        User user = _currentUserManager.User;

        user.Account.Deposit(amount);
        _bankAccountRepository.Update(user.Account, user.Account.Balance, "Deposit");
        return user.Account.Balance;
    }

    public Money GetBalance()
    {
        if (_currentUserManager.User == null)
        {
            throw new InvalidOperationException("User is not logged in.");
        }

        User user = _currentUserManager.User;

        return user.Account.Balance;
    }

    public void LinkAccount(BankAccount bankAccount, Name name)
    {
        var user = new User(Guid.NewGuid(), name, bankAccount);
        _currentUserManager.User = user;
        _userRepository.Save(user);
    }

    public IReadOnlyCollection<Transaction> GetTransactionHistory()
    {
        if (_currentUserManager.User == null)
        {
            throw new InvalidOperationException("User is not logged in.");
        }

        User user = _currentUserManager.User;

        return user.Account.Transactions.ToList().AsReadOnly();
    }
}