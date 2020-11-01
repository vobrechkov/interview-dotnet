using AutoMapper;
using BankingApi.Models.Dto;
using BankingApi.Models.Entities;
using BankingApi.Models.ViewModels;

namespace BankingApi.Models.MappingProfiles
{
    public class BankAccountMappingProfile : Profile
    {
        public BankAccountMappingProfile()
        {
            CreateMap<BankAccount, NewBankAccountDto>()
                .ReverseMap()
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore());

            CreateMap<BankAccount, BankAccountDto>()
                .IncludeBase<BankAccount, NewBankAccountDto>()
                .ReverseMap();

            CreateMap<BankAccount, BankAccountViewModel>()
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Type.ToString()))
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer != null ? $"{s.Customer.FirstName} {s.Customer.LastName}" : null))
                .ForMember(d => d.InstitutionName, o => o.MapFrom(s => s.Customer.Institution.Name));
        }
    }
}
