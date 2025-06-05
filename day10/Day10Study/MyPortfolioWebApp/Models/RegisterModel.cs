using System.ComponentModel.DataAnnotations;

namespace MyPortfolioWebApp.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        // 추가로 PhoneNumber 받으려면 string?(Nullable)로 작성
        //public string? PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? Mobile { get; set; }
        public string? Hobby { get; set; }
    }
}
