using APIClient;
using DataAccessLayer.DAO;
using Microsoft.AspNetCore.Authentication.Cookies;
public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddSingleton<IAdministratorDAO>(new AdministratorAPIClient("https://localhost:7179"));
        builder.Services.AddSingleton<ICompanyDAO>(new CompanyAPIClient("https://localhost:7179"));
        builder.Services.AddSingleton<IJobDAO>(new JobAPIClient("https://localhost:7179"));
        builder.Services.AddSingleton<IEmployeeDAO>(new EmployeeAPIClient("https://localhost:7179"));
        builder.Services.AddSingleton<IShiftDAO>(new ShiftAPIClient("https://localhost:7179"));
        builder.Services.AddSingleton<IShiftEmployeesDAO>(new ShiftEmployeesAPIClient("https://localhost:7179"));

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Administrator/Login";
                options.LogoutPath = "/Administrator/Logout";
                options.AccessDeniedPath = "/Administrator/AccessDenied";
            });

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
            pattern: "{controller=Administrator}/{action=Login}/{id?}");

        app.Run();
    }
}