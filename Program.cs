using Project01.Models;

namespace Project01
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions {
                Args = args,
                WebRootPath = Directory.GetCurrentDirectory() + @"\node_modules"
            });

            var configuration = builder.Configuration;

            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<IRepository, DbRepository>(
                provider => new DbRepository(configuration["Data:ConnectionString"])
            );

            var app = builder.Build();

            app.UseStaticFiles();
            app.UseStatusCodePages();
            app.UseDeveloperExceptionPage();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Create}/{id?}"
            );

            app.Run();
        }
    }
}
