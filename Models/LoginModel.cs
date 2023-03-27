using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoices_Manager_API.Models
{
    public class LoginModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "UserName is missing!")]
        [NotMapped]
        public string Username { get; set; } = String.Empty;

        [Required(ErrorMessage = "Password is missing!")]
        [NotMapped]
        public string Password { get; set; } = String.Empty;
        
        public string Token { get; set; } = String.Empty;
        public DateTime LoginDate { get; set; }
    }
}
