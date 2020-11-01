using AutoMapper;
using BankingApi.Models.Dto;
using BankingApi.Models.Entities;

namespace BankingApi.Models.MappingProfiles
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<Customer, NewCustomerDto>()
                .ReverseMap()
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore());

            CreateMap<Customer, CustomerDto>()
                .IncludeBase<Customer, NewCustomerDto>()
                .ReverseMap();
        }
    }
}
