namespace Invoices_Manager_API.Models
{
    public class LoginModel
    {
        [Key]
        public int Id { get; set; }

        [NotMapped]
        public int UserId { get; set; }

        [Required(ErrorMessage = "UserName is missing!")]
        [NotMapped]
        public string Username { get; set; } = String.Empty;

        [Required(ErrorMessage = "Password is missing!")]
        [NotMapped]
        public string Password { get; set; } = String.Empty;

        public string Token { get; set; } = String.Empty;
        public DateTime CreationDate { get; set; }
    }
}
