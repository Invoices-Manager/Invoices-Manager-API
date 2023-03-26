using System.ComponentModel.DataAnnotations;

namespace Invoices_Manager_API.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public NotebookModel Notebook { get; set; } = default!;

        public List<BackUpModel> BackUps { get; set; } = default!;

        public List<InvoiceModel> Invoices { get; set; } = default!;
    }
}
