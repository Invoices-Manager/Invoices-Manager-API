using InvoicesManager.Enums;
using System.ComponentModel.DataAnnotations;

namespace Invoices_Manager_API.Models
{
    public class InvoiceModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "FileID is missing!")]
        public string FileID { get; set; } = default!;

        [Required(ErrorMessage = "CaptureDate is missing!")]
        public DateTime CaptureDate { get; set; } = default!;

        [Required(ErrorMessage = "ExhibitionDate is missing!")]
        public DateTime ExhibitionDate { get; set; } = default!;
        public string? Reference { get; set; }
        public string? DocumentType { get; set; } 
        public string? Organization { get; set; } 
        public string? InvoiceNumber { get; set; }
        public string[]? Tags { get; set; }

        [Required(ErrorMessage = "ImportanceState is missing!")]
        public ImportanceStateEnum ImportanceState { get; set; } = default!;  // { VeryImportant, Important, Neutral, Unimportant }

        [Required(ErrorMessage = "MoneyState is missing!")]
        public MoneyStateEnum MoneyState { get; set; } = default!; // { Paid , Received,  NoInvoice }

        [Required(ErrorMessage = "PaidState is missing!")]
        public PaidStateEnum PaidState { get; set; } = default!;  // { Paid , Unpaid,  NoInvoice }
        public double MoneyTotal { get; set; }
    }
}
