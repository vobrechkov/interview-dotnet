using AutoMapper;
using BankingApi.Models.Dto;
using BankingApi.Models.Entities;

namespace BankingApi.Models.MappingProfiles
{
    public class TransactionMappingProfile : Profile
    {
        public TransactionMappingProfile()
        {
            CreateMap<Transaction, TransactionDto>()
                .ReverseMap()
                .ForMember(d => d.BankAccount, o => o.Ignore());

            CreateMap<Transaction, TransactionSummaryDto>()
                .ForMember(d => d.TransactionDate, o => o.MapFrom(s => s.CreatedAt))
                .ForMember(d => d.TransactionId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.TransactionType, o => o.MapFrom(s => s.Type.ToString()))
                .ForAllOtherMembers(d => d.Ignore());
        }
    }
}
