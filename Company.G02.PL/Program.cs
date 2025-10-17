using Company.G02.BLL;
using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Data.Contexts;
using Company.G02.DAL.Models;
using Company.G02.PL.Helpers;
using Company.G02.PL.Mapping;
using Company.G02.PL.Setting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Company.G02.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();//Register Build-In Mvc Servises

            builder.Services.AddScoped<IDepartmentRepository , DepartmentRepository>(); //Register DI (Dependency Injection) for DepartmentRepository
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>(); //Register DI (Dependency Injection) for DepartmentRepository
            
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); //Register DI (Dependency Injection) for UnitOfWork
            
            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }); //Register DI (Dependency Injection) for CompanyDbContext

            builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile())); //Register DI (Dependency Injection) for AutoMapper
            builder.Services.AddAutoMapper(M => M.AddProfile(new DepartmentProfile())); //Register DI (Dependency Injection) for AutoMapper
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                            .AddEntityFrameworkStores<CompanyDbContext>()
                            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie( config =>
            {
                config.LoginPath = "/Account/SignIn";
                config.LogoutPath = "/Account/SignOut";
                config.AccessDeniedPath = "/Account/AccessDenied";
                config.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            });
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
            builder.Services.AddScoped<IMailService, MailService>();
            //Life Time .
            //builder.Services.AddScoped(); //Create Object Life Time per Request - Unreachable object after Request 
            //builder.Services.AddTransient(); //Create Object Life Time per Operation   
            //builder.Services.AddSingleton();//Create Object Life Time per Application - Object will be alive until Application is running

            var app = builder.Build();




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

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
