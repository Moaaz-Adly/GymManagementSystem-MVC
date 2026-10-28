using GymManagementBLL;
using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Classses;
using GymManagementBLL.Services.Interfaces;
using GymManagementDAL.Data.Context;
using GymManagementDAL.Data.DataSeed;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagementPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GYMDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            //builder.Services.AddScoped(typeof(IGenaricRepository<>) , typeof(GenaricRepository<>));
            //builder.Services.AddScoped(typeof(IPlanRepository) , typeof(PlanRepository));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddAutoMapper(x => x.AddProfile(new MappingProfiles()));
            builder.Services.AddScoped<IAnaliticService, AnaliticService>();
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<IPlanService, PlanService>();














            var app = builder.Build();




            #region Migrate DataBase & Seed Data

            var Scope = app.Services.CreateScope();
            var dbcontext = Scope.ServiceProvider.GetRequiredService<GYMDbContext>();

            //var UserManager = Scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            //var RoleManager = Scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var PendingMigrations = dbcontext.Database.GetPendingMigrations();
            if (PendingMigrations?.Any() ?? false)
                dbcontext.Database.Migrate();

            GymDbContextSeed.SeedData(dbcontext);
            //IdentityDbContextSeeding.SeedData(RoleManager, UserManager);

            #endregion

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
