using Project.Application.Services.Accounts.Contracts;
using Project.Application.Services.Users.Contracts;
using Project.Application.Services.Accounts.Models;
using Project.Application.Services.Users.Models;
using Project.Application.Common.Models;
using Project.Application.Auth.Common;
using Project.Application.Auth.Models;
using Project.Application.Exceptions;
using Project.Application.UnitOfWork;
using Project.Application.Repository;
using System.Linq.Expressions;
using Project.Domain.Users;
using AutoMapper;
using Project.Application.Services.Transactions;

namespace Project.Application.Services.Users;
public class UserService : IUserService
{
    private readonly IRepository<User> _userRepo;
    private readonly IAccountService _accountService; 
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork<User> _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(IRepository<User> userRepository, IAccountService accountService, ITokenService tokenService, IUnitOfWork<User> unitOfWork, IMapper mapper)
    {
        _userRepo = userRepository;
        _accountService = accountService;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
    {
        var user = await _userRepo.FindAsync(x => x.PersonalN.Equals(model.PersonalN));
        if (user is null)
            throw new NotFoundException(nameof(User), model.PersonalN);
        if (!_tokenService.Verify(model.Password, user.Password))
            throw new UnauthorizedAccessException("არასწორი პაროლი");
        var systemUser = _tokenService.BuildToken(new SystemUserModel()
        {
            PersonalN = user.PersonalN,
            UserName = user.UserName
        });
        return new AuthenticateResponse(systemUser);
    }

    public async Task<AccountModel> GetAccountByPersonalN(string personalN, UserInfo userInfo)
    {
        Expression<Func<User, bool>> predicate = x => x.PersonalN == personalN && x.UserName == userInfo.UserName;
        var user = await _userRepo.FindAsync(predicate);
        if (user == null)
            throw new NotFoundException(nameof(User), personalN);
        var account = await _accountService.GetAccount(user);
        return _mapper.Map<AccountModel>(account);
    }

    public async Task UserRegistration(UserModel userRegistartion)
    {
        var validator = new UserRegistrationModelValidator();
        var result = validator.Validate(userRegistartion);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);
        var userEntity = await _userRepo.FindAsync(x => x.UserName.Equals(userRegistartion.UserName));
        if (userEntity != null)
            throw new AlreadyExistsException(nameof(User), userRegistartion.UserName);
        try
        {
            var userNumber = Guid.NewGuid();
            var account = await _accountService.CreateAccount(userNumber, userRegistartion.PersonalN);
            var user = new User()
            {
                FirstName = userRegistartion.FirstName,
                LastName = userRegistartion.LastName,
                PersonalN = userRegistartion.PersonalN,
                Age = userRegistartion.Age,
                Email = userRegistartion.Email,
                UserName = userRegistartion.UserName,
                Password = _tokenService.Hash(userRegistartion.Password),
                UserNumber = userNumber,
                Account = account,
                RegistrationDate = DateTime.Now,
            };
            await _userRepo.AddAsync(user);
            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
