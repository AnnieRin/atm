using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Services.Transactions.Contracts;
using Project.Application.Services.Transactions.Models;

namespace Project.Api.Controllers.ATMTransaction;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ATMTransactionController : ApiControllerBase
{
    public readonly ITransactionService _transactionService;

    public ATMTransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpPost("Deposit")]
    public async Task<IActionResult> Deposit([FromBody] TransactionModel request)
    {
        await _transactionService.DepositAsync(request, UserInfo);
        return Ok();
    }

    [HttpPost("Withdraw")]
    public async Task<IActionResult> Withdraw([FromBody] TransactionModel request)
    {
        await _transactionService.WithdrawAsync(request, UserInfo);
        return Ok();
    }

    [HttpPost("Transfer")]
    public async Task<IActionResult> Transfer([FromBody] TransactionModel request)
    {
        await _transactionService.TransferAsync(request, UserInfo);
        return Ok();
    }

    [HttpGet("GetTransfers")]
    public async Task<ActionResult<List<TransactionModel>>> GetTransfers()
    {
        var transfers = await _transactionService.GetAllAsync(UserInfo);
        return Ok(transfers);
    }
}
