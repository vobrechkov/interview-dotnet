using AutoMapper;
using BankingApi.Models.Dto;
using BankingApi.Models.Entities;
using BankingApi.Models.Enumerations;
using BankingApi.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApi.Data.Services
{
    public class BankAccountService
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

        public async Task<BankAccountDto> GetByIdAsync(Guid id)
        {
            var bankAccountEntity = await _ctx.BankAccounts.FindAsync(id);

            if (bankAccountEntity is null)
            {
                return null;
            }

            return _mapper.Map<BankAccountDto>(bankAccountEntity);
        }

        public async Task<bool> ExistsAsync(Guid customerId, string displayName)
            => await _ctx.BankAccounts.AnyAsync(ba => ba.CustomerId == customerId &&
                ba.DisplayName.ToLower() == displayName.ToLower());

        public async Task<BankAccountDto> CreateAsync(NewBankAccountDto bankAccount)
        {
            var bankAccountEntity = _mapper.Map<BankAccount>(bankAccount);

            bankAccountEntity.Id = Guid.NewGuid();
            bankAccountEntity.CreatedAt = DateTime.UtcNow;

            _ctx.BankAccounts.Add(bankAccountEntity);

            if (await _ctx.SaveChangesAsync() == 1)
            {
                return _mapper.Map<BankAccountDto>(bankAccountEntity);
            }

            return null;
        }

        public async Task<int> UpdateAsync(BankAccountDto bankAccount)
        {
            var bankAccountEntity = await _ctx.BankAccounts.FindAsync(bankAccount.Id);

            if (bankAccountEntity is null)
            {
                return 0;
            }

            _mapper.Map(bankAccount, bankAccountEntity);
            bankAccountEntity.UpdatedAt = DateTime.UtcNow;

            return await _ctx.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var bankAccountEntity = await _ctx.BankAccounts.FindAsync(id);

            if (bankAccountEntity is null)
            {
                return false;
            }

            _ctx.BankAccounts.Remove(bankAccountEntity);

            return await _ctx.SaveChangesAsync() == 1;
        }

        public async Task<ResultDto<TransactionSummaryDto>> CreateTransactionAsync(Guid bankAccountId, TransactionType transactionType, decimal amount)
        {
            var bankAccount = await _ctx.BankAccounts.FindAsync(bankAccountId);
            var result = new ResultDto<TransactionSummaryDto>();

            if (bankAccount is null)
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
                BankAccountId = bankAccountId,
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

        public async Task<ResultDto<TransferSummaryDto>> TranserAsync(Guid sourceAccountId, Guid destinationAccountId, decimal amount)
        {            
            BankAccount sourceAccount;
            BankAccount destinatonAccount;

            var result = new ResultDto<TransferSummaryDto>();
            var bankAccounts = _ctx.BankAccounts.Where(ba => ba.Id == sourceAccountId || ba.Id == destinationAccountId).ToList();            

            if ((sourceAccount = bankAccounts.FirstOrDefault(ba => ba.Id == sourceAccountId)) == null)
            {
                result.Errors.Add("Invalid source account");
            }

            if ((destinatonAccount = bankAccounts.FirstOrDefault(ba => ba.Id == destinationAccountId)) == null)
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

            var withdrawalResult = await CreateTransactionAsync(sourceAccountId, TransactionType.Withdrawal, amount);

            if (withdrawalResult?.Errors?.Any() == true)
            {
                await dbTransaction.RollbackAsync();
                result.Errors.AddRange(withdrawalResult.Errors);
                return result;
            }    

            var depositResult = await CreateTransactionAsync(destinationAccountId, TransactionType.Deposit, amount);

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
                transferSummary.SourceBankAccountId = sourceAccountId;
                transferSummary.DestinationBankAccountId = destinationAccountId;

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
    }
}
