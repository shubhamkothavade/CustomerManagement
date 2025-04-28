using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerManagement.Models
{
    public class FeeHistory
    {
        public int Id { get; set; }

        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        // ✅ Add this field
        [Required]
        public int MasterCustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        [Display(Name = "Financial Year")]
        public string FinancialYear { get; set; }

        [Display(Name = "Fees Paid")]
        [DataType(DataType.Currency)]
        public decimal FeesPaid { get; set; }

        [Display(Name = "Updated On")]
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }

}
