using Microsoft.EntityFrameworkCore;
using Payrollfix_poc.Data;
using Payrollfix_poc.Models;

public class program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        WebApplicationOptions

        builder.Services.AddDbContext<PayRollFix_pocContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("PayRollFixContext") ?? throw new InvalidOperationException("Connection string 'PayRollFixContext' not found.")));

        // Add services to the container.
        builder.Services.AddMvc();

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

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Employee}/{action=Attandence}/{id?}");

        app.Run();
    }
}
