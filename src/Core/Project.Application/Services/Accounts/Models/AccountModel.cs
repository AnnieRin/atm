

using Project.Application.Services.Transactions.Models;
using Project.Domain.ATMTransactions;
using System.Text.Json.Serialization;

namespace Project.Application.Services.Accounts.Models;
public class AccountModel
{
    public string AccountNumber { get; set; }
    public decimal Balance { get; set; }
}
