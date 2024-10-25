using Microsoft.EntityFrameworkCore;
using Payrollfix_poc.Data;
using Payrollfix_poc.Filters;
using Payrollfix_poc.IRepository;
using Payrollfix_poc.Services;
public class program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<PayRollFix_pocContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("PayRollFixContext") ?? throw new InvalidOperationException("Connection string 'PayRollFixContext' not found.")));

        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        builder.Services.AddScoped<IAdminRepository, AdminRepository>();
        builder.Services.AddScoped<IServicesRepository,ServiceRepository>();

        builder.Services.AddDistributedMemoryCache(); // Required for session state in memory
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
            options.Cookie.HttpOnly = true; // Make the session cookie HttpOnly
            options.Cookie.IsEssential = true; // Make it essential to be included
        });

        // Add services to the container.
        builder.Services.AddMvc(options =>
        {
            options.Filters.Add<CustomAuthorizeAttribute>(); // Register globally if needed
        });
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.  
        }
        app.UseHsts();
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseSession();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=AboutUs}/{id?}");

        app.Run();
    }
}
