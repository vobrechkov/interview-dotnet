using AutoMapper;
using BankingApi.Models.Dto;
using BankingApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApi.Data.Services
{
    public class InstitutionService
    {
        private readonly BankingContext _ctx;
        private readonly IMapper _mapper;

        public InstitutionService(BankingContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public IEnumerable<InstitutionDto> GetAll()
            => _mapper.ProjectTo<InstitutionDto>(_ctx.Institutions.OrderBy(o => o.Name));

        public async Task<InstitutionDto> GetByIdAsync(Guid id)
        {
            var institutionEntity = await _ctx.Institutions.FindAsync(id);

            if (institutionEntity == null)
            {
                return null;
            }

            return _mapper.Map<InstitutionDto>(institutionEntity);
        }


        public async Task<InstitutionDto> CreateAsync(NewInstitutionDto institution)
        {
            var institutionEntity = _mapper.Map<Institution>(institution);
            institutionEntity.CreatedAt = DateTime.UtcNow;

            _ctx.Institutions.Add(institutionEntity);

            if (await _ctx.SaveChangesAsync() == 1)
            {
                return _mapper.Map<InstitutionDto>(institutionEntity);
            }

            return null;
        }

        public async Task<int> UpdateAsync(InstitutionDto institution)
        {
            var institutionEntity = await _ctx.Institutions.FindAsync(institution.Id);

            if (institutionEntity == null)
            {
                return 0;
            }

            _mapper.Map(institution, institutionEntity);
            institutionEntity.UpdatedAt = DateTime.UtcNow;

            return await _ctx.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var institutionEntity = await _ctx.Institutions.FindAsync(id);

            if (institutionEntity == null)
            {
                return false;
            }

            _ctx.Institutions.Remove(institutionEntity);

            return await _ctx.SaveChangesAsync() == 1;
        }
    }
}
