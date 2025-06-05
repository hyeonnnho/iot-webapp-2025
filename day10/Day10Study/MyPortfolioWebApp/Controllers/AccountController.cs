using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyPortfolioWebApp.Models;
using System.Xml;

namespace MyPortfolioWebApp.Controllers
{
    public class AccountController : Controller
    {
        // ASP.NET Core Identity 필요한 변수
        private readonly UserManager<CustomUser> userManager;
        private readonly SignInManager<CustomUser> signInManager;

        // 생성자
        public AccountController(UserManager<CustomUser> userManager, SignInManager<CustomUser> signInManager)
        {
            // userManager나 signInManager가 null값이 들어오면 안됨
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        // NewsController GET Create(), Post Create()와 비슷한 역할을 함
        [HttpGet] // [HttpGet] 가 디폴트. 생략가능
        public IActionResult Register()
        {
            // 회원가입 페이지를 보여줌
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            // 모델이 유효하지 않으면 다시 회원가입 페이지를 보여줌
            if (ModelState.IsValid)
            {
                // Id를 이메일로 사용하겠다
                // 나중에 추가
                var user = new CustomUser { 
                    UserName = model.Email,
                    Email = model.Email, 
                    City = model.City,
                    Mobile = model.Mobile,
                    Hobby = model.Hobby
                };
                // 회원가입 시도
                var result = await userManager.CreateAsync(user, model.Password); // 자동으로 DB에 저장

                // 회원가입 성공 시 로그인 처리
                if (result.Succeeded)
                {
                    // 위의 저장한 유저로 로그인, isPersistent는 로그인 상태 유지 여부. false로 설정하면 20~30분 동안 사용안하면 로그아웃
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                // 회원가입 실패 시 에러 메시지를 추가하고 다시 회원가입 페이지를 보여줌
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model); // 회원가입 페이지를 다시 보여줌

        }

        [HttpGet]
        public IActionResult Login()
        {
            // 로그인 페이지를 보여줌
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, false);

                if (result.Succeeded)
                {
                    // 로그인 성공 시 홈 페이지로 리다이렉트
                    return RedirectToAction(controllerName: "Home", actionName: "Index");
                }
                // 로그인 실패 시 에러 메시지를 추가하고 다시 로그인 페이지를 보여줌
                ModelState.AddModelError(string.Empty, "로그인 실패!!");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // 로그아웃 처리
            await signInManager.SignOutAsync();
            // 로그아웃 후 홈 페이지로 리다이렉트
            return RedirectToAction("Index", "Home");
        }
    }
}

