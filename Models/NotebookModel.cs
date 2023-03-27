using System.ComponentModel.DataAnnotations;

namespace Invoices_Manager_API.Models
{
    public class NotebookModel
    {
        [Key]
        public int Id { get; set; }

        public List<NoteModel> Notebook { get; set; } = new List<NoteModel>();
    }
}
    