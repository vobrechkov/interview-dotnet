using AutoMapper;
using BankingApi.Models.Dto;
using BankingApi.Models.Entities;
using BankingApi.Models.Enumerations;
using BankingApi.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApi.Data.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly BankingContext _ctx;
        private readonly IMapper _mapper;

        public BankAccountService(BankingContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns a list of all bank accounts
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BankAccountViewModel> GetAll() => GetBankAccounts();

        /// <summary>
        /// Returns a list of all bank accounts that belong to a customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IEnumerable<BankAccountViewModel> GetAllByCustomerId(Guid customerId) => GetBankAccounts(customerId);

        /// <summary>
        /// Gets a bank account by account number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public async Task<BankAccountDto> GetByIdAsync(string number)
        {
            var bankAccountEntity = await _ctx.BankAccounts.FindAsync(number);

            if (bankAccountEntity is null)
            {
                return null;
            }

            return _mapper.Map<BankAccountDto>(bankAccountEntity);
        }

        /// <summary>
        /// Checks if a customer has an account with a given displayName
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(Guid customerId, string displayName)
            => await _ctx.BankAccounts.AnyAsync(ba => ba.CustomerId == customerId &&
                ba.DisplayName.ToLower() == displayName.ToLower());

        /// <summary>
        /// Creates a bank account
        /// </summary>
        /// <param name="bankAccount"></param>
        /// <returns></returns>
        public async Task<BankAccountDto> CreateAsync(NewBankAccountDto bankAccount)
        {
            var bankAccountEntity = _mapper.Map<BankAccount>(bankAccount);

            await using var dbTransaction = await _ctx.Database.BeginTransactionAsync();

            var bankAccountNumber = await _ctx.BankAccountNumbers.FirstOrDefaultAsync();

            if (bankAccountNumber == null)
            {
                _ctx.BankAccountNumbers.Add(new BankAccountNumber() { LastNumber = 1 });
            }

            bankAccountNumber.LastNumber++;
            bankAccountNumber.GeneratedAt = DateTime.UtcNow;

            bankAccountEntity.Number = $"{bankAccountNumber.LastNumber:0000000000}";
            bankAccountEntity.CreatedAt = DateTime.UtcNow;

            _ctx.BankAccounts.Add(bankAccountEntity);

            if (await _ctx.SaveChangesAsync() > 0)
            {
                await dbTransaction.CommitAsync();
                return _mapper.Map<BankAccountDto>(bankAccountEntity);
            }

            return null;
        }

        /// <summary>
        /// Updates a bank account
        /// </summary>
        /// <param name="bankAccount"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(BankAccountDto bankAccount)
        {
            var bankAccountEntity = await _ctx.BankAccounts.FindAsync(bankAccount.Number);

            if (bankAccountEntity == null)
            {
                return 0;
            }

            _mapper.Map(bankAccount, bankAccountEntity);
            bankAccountEntity.UpdatedAt = DateTime.UtcNow;

            return await _ctx.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a bank account
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string number)
        {
            var bankAccountEntity = await _ctx.BankAccounts.FindAsync(number);

            if (bankAccountEntity is null)
            {
                return false;
            }

            _ctx.BankAccounts.Remove(bankAccountEntity);

            return await _ctx.SaveChangesAsync() == 1;
        }

        /// <summary>
        /// Creates a bank account transaction (depoist/withdrawal)
        /// </summary>
        /// <param name="bankAccountNumber"></param>
        /// <param name="transactionType"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task<ResultDto<TransactionSummaryDto>> CreateTransactionAsync(string bankAccountNumber, TransactionType transactionType, decimal amount)
        {
            var bankAccount = await _ctx.BankAccounts.FindAsync(bankAccountNumber);
            var result = new ResultDto<TransactionSummaryDto>();

            if (bankAccount == null)
            {
                result.Errors.Add("Invalid bank account");
            }

            if (amount <= 0)
            {
                result.Errors.Add("Invalid transaction amount");
            }

            if (transactionType == TransactionType.Withdrawal)
            {
                if (bankAccount.PostedBalance <= amount)
                {
                    result.Errors.Add("Insufficient funds");
                }
            }

            if (result.Errors.Any())
            {
                return result;
            }

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                BankAccountNumber = bankAccountNumber,
                Type = transactionType,
                Amount = amount,
                CreatedAt = DateTime.UtcNow
            };

            bankAccount.PostedBalance = transactionType == TransactionType.Deposit ?
                bankAccount.PostedBalance + amount :
                bankAccount.PostedBalance - amount;

            _ctx.Transactions.Add(transaction);

            if (await _ctx.SaveChangesAsync() == 2)
            {
                var transactionSummary = _mapper.Map<TransactionSummaryDto>(transaction);
                transactionSummary.CustomerId = bankAccount.CustomerId;
                transactionSummary.CurrentBalance = bankAccount.PostedBalance;

                result.Data = transactionSummary;
            }
            else
            {
                result.Errors.Add($"Failed to complete {transactionType}");
            }

            return result;
        }

        /// <summary>
        /// Creates a transfer between two accounts
        /// </summary>
        /// <param name="sourceAccountNumber"></param>
        /// <param name="destinationAccountNumber"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task<ResultDto<TransferSummaryDto>> TranserAsync(string sourceAccountNumber, string destinationAccountNumber, decimal amount)
        {
            BankAccount sourceAccount;
            BankAccount destinatonAccount;

            var result = new ResultDto<TransferSummaryDto>();
            var bankAccounts = _ctx.BankAccounts.Where(ba => ba.Number == sourceAccountNumber || ba.Number == destinationAccountNumber).ToList();

            if ((sourceAccount = bankAccounts.FirstOrDefault(ba => ba.Number == sourceAccountNumber)) == null)
            {
                result.Errors.Add("Invalid source account");
            }

            if ((destinatonAccount = bankAccounts.FirstOrDefault(ba => ba.Number == destinationAccountNumber)) == null)
            {
                result.Errors.Add("Invalid destination account");
            }

            if (sourceAccount?.PostedBalance < amount)
            {
                result.Errors.Add("Insufficient funds");
            }

            if (result.Errors.Any())
            {
                return result;
            }

            await using var dbTransaction = await _ctx.Database.BeginTransactionAsync();

            var withdrawalResult = await CreateTransactionAsync(sourceAccountNumber, TransactionType.Withdrawal, amount);

            if (withdrawalResult?.Errors?.Any() == true)
            {
                await dbTransaction.RollbackAsync();
                result.Errors.AddRange(withdrawalResult.Errors);
                return result;
            }

            var depositResult = await CreateTransactionAsync(destinationAccountNumber, TransactionType.Deposit, amount);

            if (depositResult?.Errors?.Any() == true)
            {
                await dbTransaction.RollbackAsync();
                result.Errors.AddRange(depositResult.Errors);
                return result;
            }

            var transfer = new Transfer
            {
                Id = Guid.NewGuid(),
                Amount = amount,
                SourceTransactionId = withdrawalResult.Data.TransactionId,
                DestinationTransactionId = depositResult.Data.TransactionId,
                CreatedAt = DateTime.UtcNow
            };

            _ctx.Transfers.Add(transfer);

            if (await _ctx.SaveChangesAsync() == 1)
            {
                await dbTransaction.CommitAsync();

                var transferSummary = _mapper.Map<TransferSummaryDto>(transfer);
                transferSummary.SourceAccountNumber = sourceAccountNumber;
                transferSummary.SourceAccountBalance = sourceAccount.PostedBalance;
                transferSummary.DestinationAccountNumber = destinationAccountNumber;
                transferSummary.DestinationAccountBalance = destinatonAccount.PostedBalance;

                result.Data = transferSummary;
            }
            else
            {
                result.Errors.Add("Failed to complete transfer");
            }

            if (result.Errors.Any())
            {
                await dbTransaction.RollbackAsync();
            }

            return result;
        }

        /// <summary>
        /// Gets bank accounts filtered by customer or returns all accounts if no customerId specified
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        private IEnumerable<BankAccountViewModel> GetBankAccounts(Guid? customerId = null)
        {
            IQueryable<BankAccount> query = _ctx.BankAccounts.Include(i => i.Customer).Include(i => i.Customer.Institution);

            if (customerId.HasValue)
            {
                query = query.Where(x => x.CustomerId == (Guid)customerId);
            }

            return _mapper.ProjectTo<BankAccountViewModel>(
                query.OrderBy(o => o.DisplayName));
        }
    }
}
