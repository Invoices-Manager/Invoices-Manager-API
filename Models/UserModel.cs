namespace Invoices_Manager_API.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is missing!")]
        [MaxLength(20, ErrorMessage = "Username must be at most 20 characters long!")]
        [RegularExpression(@"^^[a-zA-Z0-9_]+$", ErrorMessage = "Username must be alphanumeric! (allowed are underscore characters '_')")]
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

        [NotMapped]
        public bool IsBlocked => IncorrectLoginAttempts >= 3;

        public int IncorrectLoginAttempts { get; set; } = 0;


        public List<NoteModel> Notebook { get; set; } = new List<NoteModel>();
        public List<InvoiceModel> Invoices { get; set; } = new List<InvoiceModel>();
        public List<LoginModel> Logins { get; set; } = new List<LoginModel>();
    }
}
