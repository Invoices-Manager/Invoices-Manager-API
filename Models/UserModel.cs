using System.ComponentModel.DataAnnotations;

namespace Invoices_Manager_API.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is missing!")]
        public string Username { get; set; } = default!;

        [Required(ErrorMessage = "Password is missing!")]
        public string Password { get; set; } = default!;

        public string Salt { get; set; } = String.Empty;

        [Required(ErrorMessage = "FirstName is missing!")]
        public string FirstName { get; set; } = default!;

        [Required(ErrorMessage = "LastName is missing!")]
        public string LastName { get; set; } = default!;

        [Required(ErrorMessage = "Email is missing!")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = default!;

        public NotebookModel Notebook { get; set; } = new NotebookModel();

        public List<BackUpModel> BackUps { get; set; } = new List<BackUpModel>();

        public List<InvoiceModel> Invoices { get; set; } = new List<InvoiceModel>();
    }
}
