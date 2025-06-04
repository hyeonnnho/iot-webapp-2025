using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPortfolioWebApp.Models;
using System.Diagnostics;

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}