namespace Invoices_Manager_API.Models
{
    public class NoteModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is missing!")]
        public string Name { get; set; } = default!;
        public string? Value { get; set; }

        [Required(ErrorMessage = "CreationDate is missing!")]
        public DateTime CreationDate { get; set; } = default!;

        [Required(ErrorMessage = "LastEditDate is missing!")]
        public DateTime LastEditDate { get; set; } = default!;
    }
}
    