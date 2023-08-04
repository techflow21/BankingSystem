using AutoMapper;
using BankingSystem.Core.Entities;
using BankingSystem.Core.Enums;
using BankingSystem.Infrastructure.GenericRepository;
using BankingSystem.Service.ServiceInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BankingSystem.Service.Implementations
{
    public class ReportService : IReportService
    {
        //private readonly IRepository<Account> _accountRepository;
        private readonly IRepository<Transaction> _transactionRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<ReportService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public ReportService(IMapper mapper, ILogger<ReportService> logger, IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _transactionRepository = _unitOfWork.GetRepository<Transaction>();
            _userManager = userManager;
        }

        public Task<int> GetTotalUsersAsync()
        {
            var count = _userManager.Users.Count();
            return Task.FromResult(count);
        }

        public async Task<decimal> GetTotalDepositsAsync()
        {
            var totalDepositedAmount = await _transactionRepository.Where(t => t.TransactionType == TransactionType.Deposit);
            var totalDeposited = totalDepositedAmount.Sum(t => t.Amount);
            return totalDeposited;
        }

        public async Task<decimal> GetTotalWithdrawalsAsync()
        {
            var totalWithdrawnAmount = await _transactionRepository.GetByAsync(t => t.TransactionType == TransactionType.Withdraw);
            var totalWithdrawn = totalWithdrawnAmount.Sum(t => t.Amount);
            return totalWithdrawn;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            var transactions = await _transactionRepository.GetAllAsync();
            var allTransactions = _mapper.Map<IEnumerable<Transaction>>(transactions);
            return allTransactions;
        }

        public async Task<IEnumerable<Transaction>> GetDepositTransactionsAsync()
        {
            var depositTransactions = await _transactionRepository.Where(t => t.TransactionType == TransactionType.Deposit);
            var deposits = _mapper.Map<IEnumerable<Transaction>>(depositTransactions);
            return deposits;
        }

        public async Task<IEnumerable<Transaction>> GetWithdrawalTransactionsAsync()
        {
            var withdrawalTransactions = await _transactionRepository.Where(t => t.TransactionType == TransactionType.Withdraw);
            var withdrawals = _mapper.Map<IEnumerable<Transaction>>(withdrawalTransactions);
            return withdrawals;
        }
    }
}
