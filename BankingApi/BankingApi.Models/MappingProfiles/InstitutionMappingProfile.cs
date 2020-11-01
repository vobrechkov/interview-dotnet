using AutoMapper;
using BankingApi.Models.Dto;
using BankingApi.Models.Entities;

namespace BankingApi.Models.MappingProfiles
{
    public class InstitutionMappingProfile : Profile
    {
        public InstitutionMappingProfile()
        {
            CreateMap<Institution, NewInstitutionDto>()
                .ReverseMap()
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore());

            CreateMap<Institution, InstitutionDto>()
                .IncludeBase<Institution, NewInstitutionDto>()
                .ReverseMap();
        }
    }
}
