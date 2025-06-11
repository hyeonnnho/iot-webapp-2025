namespace WebFrontApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args); // ASP.NET Core ���� ���� ��ü ����
            // ���� ��ü�� ��� : MVC���� ����, DB����, ���� ����, API���� �� ������ ��ü ���� ���
            var app = builder.Build();
            // ������ ���� app��ü ����
            app.UseStaticFiles(); // ���� ���� ��� ���� (wwwroot ������ ���ϵ�)

            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
