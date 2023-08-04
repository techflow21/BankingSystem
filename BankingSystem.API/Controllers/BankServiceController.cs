using BankingSystem.Core.DTOs.Requests;
using BankingSystem.Service.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankServiceController : ControllerBase
    {
        private readonly IBankOperationService _bankService;
        public BankServiceController(IBankOperationService bankService)
        {
            _bankService = bankService;
        }


        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] DepositRequest request)
        {
            var deposit = await _bankService.DepositAsync(request);
            if (deposit == null)
            {
                return BadRequest(); 
            }
            return Ok(deposit);
        }


        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferRequest request)
        {
            var transfer = await _bankService.TransferAsync(request);
            if (transfer == null)
            {
                return BadRequest();
            }
            return Ok(transfer);
        }


        [HttpGet("check-balance")]
        public async Task<IActionResult> CheckBalance(string accountNumber)
        {
            var balance = await _bankService.CheckBalanceAsync(accountNumber);
            if (balance == null)
            {
                return BadRequest();
            }
            return Ok(balance);
        }


        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawRequest request)
        {
            var withdraw = await _bankService.WithdrawAsync(request);
            if (withdraw == null)
            {
                return BadRequest();
            }
            return Ok(withdraw);
        }
    }
}
