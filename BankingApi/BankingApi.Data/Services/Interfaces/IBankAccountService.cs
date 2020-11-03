using BankingApi.Models.Dto;
using BankingApi.Models.Enumerations;
using BankingApi.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingApi.Data.Services
{
    public interface IBankAccountService
    {
        public IEnumerable<BankAccountViewModel> GetAll();
        public IEnumerable<BankAccountViewModel> GetAllByCustomerId(Guid customerId);
        public Task<BankAccountDto> GetByIdAsync(string number);
        public Task<bool> ExistsAsync(Guid customerId, string displayName);
        public Task<BankAccountDto> CreateAsync(NewBankAccountDto bankAccount);
        public Task<int> UpdateAsync(BankAccountDto bankAccount);
        public Task<bool> DeleteAsync(string number);
        public Task<ResultDto<TransactionSummaryDto>> CreateTransactionAsync(string bankAccountNumber, TransactionType transactionType, decimal amount);
        public Task<ResultDto<TransferSummaryDto>> TranserAsync(string sourceAccountNumber, string destinationAccountNumber, decimal amount);
    }
}
