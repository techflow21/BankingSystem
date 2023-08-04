using BankingSystem.Service.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ILogger<ReportController> _logger;
        public ReportController(IReportService reportService, ILogger<ReportController> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }


        [HttpGet("get-transactions")]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _reportService.GetAllTransactionsAsync();
            if (transactions == null)
            {
                _logger.LogError("Error fetching all transactions");
                return BadRequest();
            }
            _logger.LogInformation("Getting all transactions");
            return Ok(transactions);
        }


        [HttpGet("deposit-transactions")]
        public async Task<IActionResult> GetDepositTransactions()
        {
            var deposits = await _reportService.GetDepositTransactionsAsync();
            if (deposits == null)
            {
                _logger.LogError("Unable to fetch deposit transactions");
                return BadRequest();
            }
            _logger.LogInformation("Getting all deposit transactions only");
            return Ok(deposits);
        }


        [HttpGet("withdraw-transactions")]
        public async Task<IActionResult> GetWithdrawalTransactions()
        {
            var withdrawals = await _reportService.GetWithdrawalTransactionsAsync();
            if (withdrawals == null)
            {
                _logger.LogError("Unable to fetch withdrawal transactions");
                return BadRequest();
            }
            _logger.LogInformation("Getting all withdrawal transactions only");
            return Ok(withdrawals);
        }

        
        [HttpGet("total-deposited")]
        public async Task<IActionResult> GetTotalDeposit()
        {
            var totalDeposit = await _reportService.GetTotalDepositsAsync();
            if (totalDeposit < 0 )
            {
                _logger.LogError("Unable to fetch total deposit");
                return BadRequest();
            }
            _logger.LogInformation("fetching total deposit");
            return Ok(totalDeposit);
        }


        [HttpGet("total-withdrawn")]
        public async Task<IActionResult> GetTotalWithdrawal()
        {
            var totalWithdraw = await _reportService.GetTotalWithdrawalsAsync();
            if (totalWithdraw < 0)
            {
                _logger.LogError("Unable to fetch total withdrawals");
                return BadRequest();
            }
            _logger.LogInformation("fetching total withdrawals");
            return Ok(totalWithdraw);
        }
    }
}
