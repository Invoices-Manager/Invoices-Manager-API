using System.ComponentModel.DataAnnotations;

namespace Invoices_Manager_API.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is missing!")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is missing!")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "FirstName is missing!")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "LastName is missing!")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is missing!")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;

        public NotebookModel Notebook { get; set; } = default!;

        public List<BackUpModel> BackUps { get; set; } = default!;

        public List<InvoiceModel> Invoices { get; set; } = default!;
    }
}
