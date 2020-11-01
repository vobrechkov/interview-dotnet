using System;

namespace BankingApi.Models.ViewModels
{
    public class BankAccountViewModel
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string InstitutionName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public decimal PostedBalance { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
