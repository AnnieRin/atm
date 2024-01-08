using Project.Application.Services.Transactions.Contracts;
using Project.Application.Services.Transactions.Models;
using Project.Application.Services.Accounts.Contracts;
using Project.Application.Common.Models;
using Project.Application.Exceptions;
using Project.Application.Repository;
using Project.Domain.ATMTransactions;
using Project.Application.UnitOfWork;
using System.Linq.Expressions;
using Project.Domain.Accounts;
using Project.Domain.Users;
using Project.Domain.Enum;
using AutoMapper;
using Project.Application.Services.Logs.Contracts;
using System.Security.Principal;

namespace Project.Application.Services.Transactions;
public class TransactionService : ITransactionService
{
    private readonly IRepository<ATMTransaction> _transactionRepo;
    private readonly IUnitOfWork<ATMTransaction> _uniUnitOfWork;
    private readonly IAuditLogService _auditLogService;
    private readonly IAccountService _accountService;
    private readonly IRepository<User> _userRepo;
    private readonly IMapper _mapper;

    public TransactionService(IRepository<ATMTransaction> transactionRepository, 
        IUnitOfWork<ATMTransaction> unitOfWork, 
        IAuditLogService auditLogService,
        IAccountService accountService, 
        IRepository<User> usserRepo, 
        IMapper mapper)
        
    {
        _transactionRepo = transactionRepository;
        _auditLogService = auditLogService;
        _accountService = accountService;
        _uniUnitOfWork = unitOfWork;
        _userRepo = usserRepo;
        _mapper = mapper;
    }

    public async Task<List<TransactionModel>> GetAllAsync(UserInfo userInfo)
    {
        Expression<Func<User, bool>> predicate = x => x.PersonalN == userInfo.PersonalN;
        var user = await _userRepo.FindAsync(predicate);
        if (user == null)
        {
            throw new NotFoundException(nameof(User), userInfo.PersonalN);
        }
        try
        {
            var account = await _accountService.GetAccount(user);
            if (account == null)
            {
                throw new NotFoundException(nameof(Account), userInfo.PersonalN);
            }
            Expression<Func<ATMTransaction, bool>> predicate1 = x => x.UserIBAN == account.AccountNumber || x.RecipientIBAN == account.AccountNumber;
            var transactions = await _transactionRepo.FindListAsync(predicate1);
            return _mapper.Map<List<TransactionModel>>(transactions);
        }
        catch (Exception ex)
        {
            await _auditLogService.LogExceptionAsync(ex, user, nameof(TransactionService));
            throw;
        }
    }

    public async Task DepositAsync(TransactionModel transactionRequest, UserInfo userInfo)
    {
        await ProcessTransactionAsync(transactionRequest, userInfo, TransactionType.Deposit);
    }

    public async Task WithdrawAsync(TransactionModel transactionRequest, UserInfo userInfo)
    {
        await ProcessTransactionAsync(transactionRequest, userInfo, TransactionType.Withdrawal);
    }

    public async Task TransferAsync(TransactionModel transactionRequest, UserInfo userInfo)
    {
        await ProcessTransactionAsync(transactionRequest, userInfo, TransactionType.Transfer);
    }

    private async Task ProcessTransactionAsync(TransactionModel transactionRequest, UserInfo userInfo, TransactionType transactionType)
    {
        Expression<Func<User, bool>> userPredicate = x => x.Account.AccountNumber == transactionRequest.UserIBAN && x.PersonalN == userInfo.PersonalN;
        Expression<Func<User, bool>> recipientPredicate = x => x.Account.AccountNumber == transactionRequest.RecipientIBAN;
        var user = await _userRepo.FindAsync(userPredicate);
        var recipient = await _userRepo.FindAsync(recipientPredicate);
        try
        {
            await _uniUnitOfWork.BeginTransactionAsync();
            var account = await _accountService.GetAccountForTransaction(transactionRequest.UserIBAN, user);
            if (account == null)
            {
                throw new NotFoundException(nameof(Account), transactionRequest.UserIBAN);
            }

            switch (transactionType)
            {
                case TransactionType.Deposit:
                    account.Balance += transactionRequest.Amount;
                    break;
                case TransactionType.Withdrawal:
                    if (account.Balance < transactionRequest.Amount)
                    {
                        throw new BadRequestException("ანგარიშზე არ არის საკმარისი თანხა");
                    }
                    account.Balance -= transactionRequest.Amount;
                    break;
                case TransactionType.Transfer:
                    if(recipient != null)
                    {
                        var toAccount = await _accountService.GetAccountForTransaction(transactionRequest.RecipientIBAN, recipient);

                        if (toAccount == null)
                        {
                            throw new NotFoundException(nameof(Account), transactionRequest.RecipientIBAN);
                        }
                        if (account.Balance < transactionRequest.Amount)
                        {
                            throw new BadRequestException("ანგარიშზე არ არის საკმარისი თანხა");
                        }
                        account.Balance -= transactionRequest.Amount;
                        toAccount.Balance += transactionRequest.Amount;
                        await _accountService.UpdateAsync(toAccount, recipient.PersonalN);
                    }
                    break;
            }

            await _accountService.UpdateAsync(account, user.PersonalN);
            var transaction = GenerateTransaction(transactionRequest, account, transactionType);
            await _transactionRepo.AddAsync(transaction);
            await _uniUnitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex, user);
        }
    }
    private ATMTransaction GenerateTransaction(TransactionModel transactionRequest, Account account, TransactionType transactionType)
    {
        return new ATMTransaction
        {
            Amount = (transactionType == TransactionType.Withdrawal || transactionType == TransactionType.Transfer) ? -transactionRequest.Amount : transactionRequest.Amount,
            Date = DateTime.Now,
            Type = transactionType.ToString(),
            UserIBAN = transactionRequest.UserIBAN,
            RecipientIBAN = transactionRequest.RecipientIBAN ?? string.Empty,
            AccountId = account.Id
        };
    }

    private async Task<Exception> HandleExceptionAsync(Exception ex, User user)
    {
        await _uniUnitOfWork.RollbackAsync();
        await _auditLogService.LogExceptionAsync(ex, user, nameof(TransactionService));
        throw ex;
    }
}
