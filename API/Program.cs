using DataAccessLayer.DAO;
using DataAccessLayer.SQL;
using Microsoft.AspNetCore.Authentication.Cookies;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddScoped<ICompanyDAO>((_) => new CompanySQL("Data Source=hildur.ucn.dk;Initial Catalog=dma-csd-v221_10009362;User Id=dma-csd-v221_10009362;Password=Password1!;"));
        builder.Services.AddScoped<IShiftDAO>((_) => new ShiftSQL("Data Source=hildur.ucn.dk;Initial Catalog=dma-csd-v221_10009362;User Id=dma-csd-v221_10009362;Password=Password1!;"));
        builder.Services.AddScoped<IShiftEmployeesDAO>((_) => new ShiftEmployeesSQL("Data Source=hildur.ucn.dk;Initial Catalog=dma-csd-v221_10009362;User Id=dma-csd-v221_10009362;Password=Password1!;"));
        builder.Services.AddScoped<IEmployeeDAO>((_) => new EmployeeSQL("Data Source=hildur.ucn.dk;Initial Catalog=dma-csd-v221_10009362;User Id=dma-csd-v221_10009362;Password=Password1!;"));
        builder.Services.AddScoped<IAdministratorDAO>((_) => new AdministratorSQL("Data Source=hildur.ucn.dk;Initial Catalog=dma-csd-v221_10009362;User Id=dma-csd-v221_10009362;Password=Password1!;"));
        builder.Services.AddScoped<IJobDAO>((_) => new JobSQL("Data Source=hildur.ucn.dk;Initial Catalog=dma-csd-v221_10009362;User Id=dma-csd-v221_10009362;Password=Password1!;"));

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Administrator/Login";
            options.LogoutPath = "/Administrator/Logout";
            options.AccessDeniedPath = "/Administrator/AccessDenied";
        });



        builder.Services.AddControllers();
        builder.Services.AddLogging();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.UseAuthentication();

        app.Run();
    }
}