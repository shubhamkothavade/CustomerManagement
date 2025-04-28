using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerManagement.Models
{
    public class FeeRecord
    {
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int MasterCustomerId { get; set; }

        [Required]
        public string FinancialYear { get; set; }

        public decimal TotalFees { get; set; }

        public DateTime UpdatedOn { get; set; } = DateTime.Now;

        public Customer Customer { get; set; }
    }

}
