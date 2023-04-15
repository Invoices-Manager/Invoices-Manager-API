namespace Invoices_Manager_API.Models
{
    public class BackUpInfoModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "BackUp is missing!")]
        public BackUpModel BackUp { get; set; } = default!;

        [Required(ErrorMessage = "BackUpName is missing!")]
        public string BackUpName { get; set; } = default!;
        
        [Required(ErrorMessage = "DateOfCreation is missing!")]
        public DateTime DateOfCreation { get; set; } = default!;

        [Required(ErrorMessage = "BackUpSize is missing!")]
        public string BackUpSize { get; set; } = default!;

        [Required(ErrorMessage = "EntityCountInvoices is missing")]
        [Range(0, int.MaxValue)]
        public int EntityCountInvoices { get; set; } = default!;

        [Required(ErrorMessage = "EntityCountNotes is missing")]
        [Range(0, int.MaxValue)]
        public int EntityCountNotes { get; set; } = default!;
    }
}
