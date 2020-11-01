using System;

namespace BankingApi.Models.Dto
{
    public class InstitutionDto : NewInstitutionDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
