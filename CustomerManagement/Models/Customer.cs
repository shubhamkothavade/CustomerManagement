using System.ComponentModel.DataAnnotations;

namespace CustomerManagement.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public int MasterCustomerId { get; set; }

        [Required]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        [Display(Name = "Phone No")]
        public string PhoneNo { get; set; }

        [Display(Name = "Mobile No")]
        public string MobileNo { get; set; }

        [Display(Name = "Address 1")]
        public string Address1 { get; set; }

        [Display(Name = "Address 2")]
        public string Address2 { get; set; }

        [Display(Name = "Service")]
        public string Service { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [Display(Name = "PanCard No")]
        public string PanCardNo { get; set; }

        [Display(Name = "GST No")]
        public string GSTNO { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        

    }
}
