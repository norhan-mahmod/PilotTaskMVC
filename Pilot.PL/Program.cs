using Microsoft.EntityFrameworkCore;
using Pilot.BLL.IssueTypeRepo;
using Pilot.BLL.TicketRepo;
using Pilot.DAL.Context;
using Pilot.PL.Helper;

namespace Pilot.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<TicketDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<ITicketRepository, TicketRepository>();
            builder.Services.AddScoped<IIssueTypeRepository , IssueTypeRepository>();

            var app = builder.Build();

            //Initialize database using script file
            DatabaseInitializer.EnsureDatabaseExists(builder.Configuration.GetConnectionString("master"));
            var scop = app.Services.CreateScope();
            var services = scop.ServiceProvider;
            var context = services.GetRequiredService<TicketDbContext>();
            DatabaseInitializer.Initialize(context);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
