using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyPortfolioWebApp.Models;

namespace MyPortfolioWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            // DB연결 초기화
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(
                builder.Configuration.GetConnectionString("SmartHomeConnection"),
                ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("SmartHomeConnection"))
            ));

            // ASP.NET Core Identity 설정
            // 원본은 IdentityUser -> CustomUser로 변경
            builder.Services.AddIdentity<CustomUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // 패스워드 정책
            // 변경 전. 최소 6자리 이상, 특수문자, 대문자, 소문자 포함
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 4; // 최소 길이
                options.Password.RequireNonAlphanumeric = false; // 특수문자 사용안함
                options.Password.RequireUppercase = false; // 대문자 사용안함
                options.Password.RequireLowercase = false; // 소문자 사용안함
                options.Password.RequireDigit = false; // 숫자 필수 여부
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication(); //  ASP.NET Core Identity 계정
            app.UseAuthorization(); // 권한

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}