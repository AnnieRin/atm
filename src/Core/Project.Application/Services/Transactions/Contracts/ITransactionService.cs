using Project.Application.Common.Models;
using Project.Application.Services.Transactions.Models;

namespace Project.Application.Services.Transactions.Contracts;
public interface ITransactionService
{
    Task DepositAsync(TransactionModel transactionRequest, UserInfo userInfo);
    Task WithdrawAsync(TransactionModel transactionRequest, UserInfo userInfo);
    Task TransferAsync(TransactionModel transactionRequest, UserInfo userInfo);
    Task<List<TransactionModel>> GetAllAsync(UserInfo userInfo);
}
