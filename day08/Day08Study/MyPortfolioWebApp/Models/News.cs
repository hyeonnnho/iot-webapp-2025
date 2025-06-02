using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MyPortfolioWebApp.Models
{
    // 실제 DB이 News 테이블로 만들어짐
    public class News
    {
        [Key] // Id == Primary Key
        [DisplayName("번호")]
        public int Id { get; set; }
        [DisplayName("작성자")]
        [BindNever] // 폼에서의 입력 무시. 서버에서 설정
        public string? Writer { get; set; } // Nullable로 변경하면 Validation에서 제외
        [DisplayName("뉴스제목")]
        [Required(ErrorMessage ="{0}은 필수입니다.")]
        public string Title { get; set; }
        [DisplayName("뉴스내용")]
        [Required(ErrorMessage = "{0}은 필수입니다.")]
        public string Description { get; set; }
        [DisplayName("작성일")]
        [DisplayFormat(DataFormatString = "{0:yyyy년 MM월 dd일}", ApplyFormatInEditMode = true)]
        [BindNever] // 폼에서의 입력 무시. 서버에서 설정
        public DateTime PostDate { get; set; }
        [DisplayName("조회수")]
        [BindNever] // 폼에서의 입력 무시. 서버에서 설정
        public int ReadCount { get; set; }
    }
}
