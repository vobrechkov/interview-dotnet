using BankingApi.Models.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingApi.Data.Services
{
    public interface IInstitutionService
    {
        public IEnumerable<InstitutionDto> GetAll();
        public Task<InstitutionDto> GetByIdAsync(Guid id);
        public Task<InstitutionDto> CreateAsync(NewInstitutionDto institution);
        public Task<int> UpdateAsync(InstitutionDto institution);
        public Task<bool> DeleteAsync(Guid id);
    }
}
