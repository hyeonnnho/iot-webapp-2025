using Microsoft.AspNetCore.Identity;

namespace MyPortfolioWebApp.Models
{
    // IdentityUser는 AspNetCore.Identity 네임스페이스에 있는 클래스
    // 회원가입 시 추가로 받고싶은 정보를 구성
    public class CustomUser : IdentityUser
    {
        public string? Mobile { get; set; } // 휴대폰 번호
        public string? City { get; set; } // 도시
        public string? Hobby { get; set; } // 취미
    }
}
