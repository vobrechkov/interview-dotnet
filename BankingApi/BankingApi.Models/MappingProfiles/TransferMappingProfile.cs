using AutoMapper;
using BankingApi.Models.Dto;
using BankingApi.Models.Entities;

namespace BankingApi.Models.MappingProfiles
{
    public class TransferMappingProfile : Profile
    {
        public TransferMappingProfile()
        {
            CreateMap<Transfer, TransferDto>()
                .ReverseMap()
                .ForMember(d => d.SourceTransaction, o => o.Ignore())
                .ForMember(d => d.DestinationTransaction, o => o.Ignore());

            CreateMap<Transfer, TransferSummaryDto>()
                .ForMember(d => d.TransferId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.TransferDate, o => o.MapFrom(s => s.CreatedAt))
                .ForMember(d => d.SourceBankAccountId, o => o.Ignore())
                .ForMember(d => d.DestinationBankAccountId, o => o.Ignore());
        }
    }
}
