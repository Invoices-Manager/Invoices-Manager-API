using InvoicesManager.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoices_Manager_API.Models
{
    public class InvoiceModel
    {
        [Key]
        public int Id { get; set; }

        public string FileID { get; set; } = String.Empty;

        public DateTime CaptureDate { get; set; } = default!;

        [Required(ErrorMessage = "ExhibitionDate is missing!")]
        public DateTime ExhibitionDate { get; set; } = default!;
        public string? Reference { get; set; }
        public string? DocumentType { get; set; } 
        public string? Organization { get; set; } 
        public string? InvoiceNumber { get; set; }

        [NotMapped]
        public string[] Tags { get; set; } = Array.Empty<string>();

        [Column("Tags")]
        public string TagsAsString
        {
            get => string.Join(";", Tags);
            set => Tags = value.Split(";", StringSplitOptions.RemoveEmptyEntries);
        }

        [Required(ErrorMessage = "ImportanceState is missing!")]
        public ImportanceStateEnum ImportanceState { get; set; } = default!;  // { VeryImportant, Important, Neutral, Unimportant }

        [Required(ErrorMessage = "MoneyState is missing!")]
        public MoneyStateEnum MoneyState { get; set; } = default!; // { Paid , Received,  NoInvoice }

        [Required(ErrorMessage = "PaidState is missing!")]
        public PaidStateEnum PaidState { get; set; } = default!;  // { Paid , Unpaid,  NoInvoice }
        public double MoneyTotal { get; set; }
    }
}
