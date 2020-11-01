using System;
using System.ComponentModel.DataAnnotations;

namespace BankingApi.Models.Dto
{
    public class NewCustomerDto
    {
        [Required]
        public Guid InstitutionId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
