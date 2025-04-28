namespace CustomerManagement.Models
{
    public class FeeTrackingListItemViewModel
    {
        public int FeeRecordId { get; set; }
        public int MasterCustomerId { get; set; }
        public string FinancialYear { get; set; }
        public decimal TotalFees { get; set; }
    }
}
