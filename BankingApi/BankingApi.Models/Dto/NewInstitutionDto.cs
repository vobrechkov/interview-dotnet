using System;
using System.ComponentModel.DataAnnotations;

namespace BankingApi.Models.Dto
{
    public class NewInstitutionDto
    {
        [Required]
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
    }
}
