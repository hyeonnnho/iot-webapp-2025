namespace WebFrontApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args); // ASP.NET Core 시작 빌더 객체 생성
            // 빌더 객체의 기능 : MVC패턴 설정, DB설정, 권한 설정, API설정 등 웹앱의 전체 설정 담당
            var app = builder.Build();
            // 빌더로 만든 app객체 설정
            app.UseStaticFiles(); // 정적 파일 사용 설정 (wwwroot 폴더의 파일들)

            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
