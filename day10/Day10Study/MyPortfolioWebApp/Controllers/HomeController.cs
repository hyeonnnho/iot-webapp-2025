using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPortfolioWebApp.Models;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace MyPortfolioWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context; // DB 연동

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {
            // 정적 HTML을 DB 데이터로 동적 처리
            // DB에서 데이터를 불러온 뒤 About, Skill 모델을 객체에 담아서 뷰로 전달
            var skillCount = _context.Skill.Count(); // 기술 개수
            var skill = await _context.Skill.ToListAsync();

            var about = await _context.About.FirstOrDefaultAsync(); // FirstAsync는 데이터가 없으면 예외 발생. FirstDefaultAsync 데이터가 없으면 널값

            ViewBag.SkillCount = skillCount; // ex) 6이 넘어감
            ViewBag.ColNum = (skillCount / 2) + (skillCount % 2); // ex) 3(6/2) + 0(6%2) = 3
            ViewBag.MaxNum = skillCount;

            var model = new AboutModel();
            model.About = about;
            model.Skill = skill;

            return View(model);
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Contact(ContactModel model)
        {
            if (ModelState.IsValid) // Model에 들어간 4개의 값이 제대로 들어갔으면
            {
                try
                {
                    // 이메일 보내기
                    var smtpClient = new SmtpClient("smtp.gmail.com")   // Gmail을 사용하면
                    {
                        Port = 465, // 메일 포트번호
                        Credentials = new NetworkCredential("tiger1330@naver.com", "비밀번호"), // 네이버 계정과 비밀번호
                        EnableSsl = true, // SSL 사용

                    };

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(model.Email),    // 문의하기에 작성한 메일주소
                        Subject = model.Subject ?? "[제목없음]",
                        Body = $"보낸 사람: {model.Name} ({model.Email})\n\n메시지 : {model.Message}",
                        IsBodyHtml = false, // HTML 형식이 아니므로 false
                    };

                    mailMessage.To.Add("tiger1330@naver.com"); // 받을 메일 주소

                    await smtpClient.SendMailAsync(mailMessage); // 비동기 메일 전송
                    ViewBag.Success = true; // 성공 메시지

                }
                catch (Exception ex)
                {
                    ViewBag.Success = false;
                    // 실패 메시지
                    ViewBag.Error = $"전송에 실패했습니다. {ex.Message}";
                }
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}