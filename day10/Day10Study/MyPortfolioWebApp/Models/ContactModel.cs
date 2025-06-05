using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MyPortfolioWebApp.Models
{
    public class ContactModel
    {
        [Required(ErrorMessage ="성함은 필수입니다")]
        public string Name { get; set; }
        [Required(ErrorMessage ="이메일은 필수입니다")]
        public string Email { get; set; }
        [Required(ErrorMessage ="제목은 필수입니다")]
        public string Subject { get; set; }
        [Required(ErrorMessage ="메시지은 필수입니다")]
        public string Message { get; set; }
    }
}
