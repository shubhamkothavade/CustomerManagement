namespace CustomerManagement.Models
{
    public class FeeSummaryViewModel
    {
        public int FeeRecordId { get; set; }
        public int CustomerId { get; set; }
        public int MasterCustomerId { get; set; }
        public string CustomerName { get; set; }
        public string FinancialYear { get; set; }
        public decimal TotalFees { get; set; }
        public decimal FeesPaid { get; set; }
        public decimal Balance => TotalFees - FeesPaid;

    }
}
