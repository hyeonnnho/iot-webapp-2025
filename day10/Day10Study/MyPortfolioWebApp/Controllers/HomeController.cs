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
        private readonly ApplicationDbContext _context; // DB ����

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
            // ���� HTML�� DB �����ͷ� ���� ó��
            // DB���� �����͸� �ҷ��� �� About, Skill ���� ��ü�� ��Ƽ� ��� ����
            var skillCount = _context.Skill.Count(); // ��� ����
            var skill = await _context.Skill.ToListAsync();

            var about = await _context.About.FirstOrDefaultAsync(); // FirstAsync�� �����Ͱ� ������ ���� �߻�. FirstDefaultAsync �����Ͱ� ������ �ΰ�

            ViewBag.SkillCount = skillCount; // ex) 6�� �Ѿ
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
            if (ModelState.IsValid) // Model�� �� 4���� ���� ����� ������
            {
                try
                {
                    // �̸��� ������
                    var smtpClient = new SmtpClient("smtp.gmail.com")   // Gmail�� ����ϸ�
                    {
                        Port = 465, // ���� ��Ʈ��ȣ
                        Credentials = new NetworkCredential("tiger1330@naver.com", "��й�ȣ"), // ���̹� ������ ��й�ȣ
                        EnableSsl = true, // SSL ���

                    };

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(model.Email),    // �����ϱ⿡ �ۼ��� �����ּ�
                        Subject = model.Subject ?? "[�������]",
                        Body = $"���� ���: {model.Name} ({model.Email})\n\n�޽��� : {model.Message}",
                        IsBodyHtml = false, // HTML ������ �ƴϹǷ� false
                    };

                    mailMessage.To.Add("tiger1330@naver.com"); // ���� ���� �ּ�

                    await smtpClient.SendMailAsync(mailMessage); // �񵿱� ���� ����
                    ViewBag.Success = true; // ���� �޽���

                }
                catch (Exception ex)
                {
                    ViewBag.Success = false;
                    // ���� �޽���
                    ViewBag.Error = $"���ۿ� �����߽��ϴ�. {ex.Message}";
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