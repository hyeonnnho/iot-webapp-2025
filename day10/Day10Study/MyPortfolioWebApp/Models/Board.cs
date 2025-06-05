using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MyPortfolioWebApp.Models;

public partial class Board
{
    [Key]
    [DisplayName("번호")]
    public int Id { get; set; }
    [Required]
    [BindNever] // 폼에서의 입력 무시. 서버에서 설정
    public string? Email { get; set; }
    [DisplayName("작성자")]
    [BindNever] // 폼에서의 입력 무시. 서버에서 설정
    public string? Writer { get; set; }
    [DisplayName("제목")]
    [Required(ErrorMessage = "{0}은 필수입니다.")]
    public string Title { get; set; }
    [DisplayName("내용")]
    [Required(ErrorMessage = "{0}은 필수입니다.")]
    public string Contents { get; set; }
    [DisplayName("작성일")]
    [DisplayFormat(DataFormatString = "{0:yyyy년 MM월 dd일}", ApplyFormatInEditMode = true)]
    [BindNever] // 폼에서의 입력 무시. 서버에서 설정
    public DateTime? PostDate { get; set; }
    [DisplayName("조회수")]
    [BindNever] // 폼에서의 입력 무시. 서버에서 설정
    public int? ReadCount { get; set; }
}
