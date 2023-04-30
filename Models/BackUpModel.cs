namespace Invoices_Manager_API.Models
{
    public class BackUpModel
    {
        [Key]
        public int Id { get; set; }

        public List<InvoiceBackUpModel> Invoices { get; set; } = default!;
        [NotMapped]
        public List<NoteModel> Notebook { get; set; } = default!;
    }
}
