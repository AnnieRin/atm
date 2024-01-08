using AutoMapper;
using Project.Domain.Accounts;
using Project.Domain.Users;
using Project.Domain.ATMTransactions;
using Project.Application.Services.Users.Models;
using Project.Application.Services.Accounts.Models;
using Project.Application.Services.Transactions.Models;

namespace Project.Application.Common.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserModel>().ReverseMap();
        CreateMap<Account, AccountModel>().ReverseMap();
        CreateMap<ATMTransaction, TransactionModel>().ReverseMap();
    }
}
