using Project.Application.Services.Accounts.Contracts;
using Project.Application.Services.Accounts.Helpers;
using Project.Application.Services.Accounts.Models;
using Project.Application.Common.Models;
using Project.Application.Exceptions;
using Project.Application.Repository;
using Project.Application.UnitOfWork;
using Project.Domain.Accounts;
using System.Linq.Expressions;
using Project.Domain.Users;
using AutoMapper;

namespace Project.Application.Services.Accounts;
public class AccountService : IAccountService
{
    private readonly IRepository<Account> _accountRepo;
    private readonly IUnitOfWork<Account> _uniUnitOfWork;
    private readonly IMapper _mapper;

    public AccountService(IRepository<Account> accountRepo, IUnitOfWork<Account> unitOfWork, IMapper mapper)
    {
        _accountRepo = accountRepo;
        _uniUnitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Account> CreateAccount(Guid userNumber, string personalN)
    {
        var account = new Account();
        account.AccountNumber = IBANGenerator.GenerateAccountNumber();
        account.UserNumber = userNumber;
        account.PersonalN = personalN;
        await _accountRepo.AddAsync(account);
        await _uniUnitOfWork.CommitAsync();
        return account;
    }

    public async Task<Account> GetAccountForTransaction(string accountNumber, User user)
    {
        Expression<Func<Account, bool>> predicate = x => x.AccountNumber == accountNumber && x.PersonalN == user.PersonalN;
        var account = await _accountRepo.FindAsync(predicate);
        return account;
    }

    public async Task<AccountModel> GetAccount(User user)
    {
        Expression<Func<Account, bool>> predicate = x => x.UserNumber == user.UserNumber && x.PersonalN == user.PersonalN;
        var account = await _accountRepo.FindAsync(predicate);
        if (account == null)
            throw new NotFoundException(nameof(Account), user.PersonalN);
        return _mapper.Map<AccountModel>(account);
    }

    public async Task UpdateAsync(Account accountToUpdate, string personalN)
    {
        Expression<Func<Account, bool>> predicate = x => x.AccountNumber == accountToUpdate.AccountNumber && x.PersonalN == personalN;
        var existingAccount = await _accountRepo.FindAsync(predicate);
        if (existingAccount == null)
        {
            throw new KeyNotFoundException("Account not found.");
        }
        existingAccount.Balance = accountToUpdate.Balance;

        await _accountRepo.UpdateAsync(existingAccount);
        await _uniUnitOfWork.CommitAsync();
    }
}
