using AutoMapper;
using BankingSystem.Core.DTOs.Requests;
using BankingSystem.Core.Entities;
using BankingSystem.Core.Enums;
using BankingSystem.Infrastructure.GenericRepository;
using BankingSystem.Service.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BankingSystem.Service.Implementations
{
    public class BankOperationService : IBankOperationService
    {
        private readonly IRepository<Account> _accountRepository;
        private readonly IRepository<Transaction> _transactionRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<BankOperationService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        public BankOperationService(UserManager<User> userManager, IMapper mapper, ILogger<BankOperationService> logger, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            //_httpContextAccessor = httpContextAccessor;
            _accountRepository = _unitOfWork.GetRepository<Account>();
            _transactionRepository = _unitOfWork.GetRepository<Transaction>();
        }

        public async Task<string> DepositAsync(DepositRequest request)
        {
            var account = await _accountRepository.GetByNameAsync(request.AccountNumber);
            if (account == null)
            {
                return "Account not found.";
            }
            if (request.Amount < 10 || request.Amount > 100_000)
            {
                return "Attempting to deposit invalid amount, try again";
            }
            account.Balance += request.Amount;
            await _accountRepository.UpdateAsync(account);

            //var transaction = _mapper.Map<Transaction>(request);
            var transaction = new Transaction()
            {
                SenderAccount = request.AccountNumber,
                ReceiverAccount = request.AccountNumber,
                Amount = request.Amount,
                TransactionType = TransactionType.Deposit,
                TransactionDate = DateTime.UtcNow
            };

            await _transactionRepository.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();
            return "Money deposited successfully.";
        }

        public async Task<string> TransferAsync(TransferRequest request)
        {
            var fromAccount = await _accountRepository.GetByNameAsync(request.FromAccountNumber);
            var toAccount = await _accountRepository.GetByNameAsync(request.ToAccountNumber);
            if (toAccount == null)
            {
                return "Customer account not found";
            }
            if (request.Amount < 10 || request.Amount > 100_000)
            {
                return "Attempting to transfer invalid amount, try again";
            }
            if (fromAccount.Balance < request.Amount)
            {
                return "Insufficient balance";
            }

            fromAccount.Balance -= request.Amount;
            toAccount.Balance += request.Amount;

            await _accountRepository.UpdateAsync(fromAccount);
            await _accountRepository.UpdateAsync(toAccount);

            //var transaction = _mapper.Map<Transaction>(request);
            var transaction = new Transaction()
            {
                SenderAccount = request.FromAccountNumber,
                ReceiverAccount = request.ToAccountNumber,
                Amount = request.Amount,
                TransactionType = TransactionType.Transfer,
                TransactionDate = DateTime.UtcNow
            };

            await _transactionRepository.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();
            return $"${request.Amount} transferred successfully.";
        }

        public async Task<string> CheckBalanceAsync(string accountNumber)
        {
            var account = await _accountRepository.GetByNameAsync(accountNumber);
            if (account == null)
            {
                return "Account not found";
            }
            var accountBalance = account.Balance;
            return $"The remaining account balance is ${accountBalance}.";
        }

        public async Task<string> WithdrawAsync(WithdrawRequest request)
        {
            var account = await _accountRepository.GetByIdAsync(request.AccountNumber);
            if (account == null)
            {
                return "Account not found.";
            }
            if (request.Amount < 10 || request.Amount > 100_000)
            {
                return "Attempting to withdraw invalid amount, try again";
            }
            if (account.Balance < request.Amount)
            {
                return "Insufficient balance.";
            }

            account.Balance -= request.Amount;
            await _accountRepository.UpdateAsync(account);

            var transaction = new Transaction()
            {
                SenderAccount = request.AccountNumber,
                ReceiverAccount = request.AccountNumber,
                Amount = request.Amount,
                TransactionType = TransactionType.Withdraw,
                TransactionDate = DateTime.UtcNow
            };

            await _transactionRepository.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();
            return $"${request.Amount} withdrawn successfully!";
        }
    }
}
