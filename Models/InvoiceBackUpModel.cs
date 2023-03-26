using System.ComponentModel.DataAnnotations;

namespace Invoices_Manager_API.Models
{
    public class InvoiceBackUpModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "InvoiceModel is missing!")]
        public InvoiceModel Invoice { get; set; } = default!;

        [Required(ErrorMessage = "Base64 is missing!")]
        public string Base64 { get; set; } = default!;
    }
}
