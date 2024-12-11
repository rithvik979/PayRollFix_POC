using Microsoft.EntityFrameworkCore;
using Payrollfix_poc.Data;
using Payrollfix_poc.IRepository;
using Payrollfix_poc.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Payrollfix_poc.SignalR_Hub;
using Microsoft.AspNetCore.SignalR;

public class program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<PayRollFix_pocContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("PayRollFixContext") ?? throw new InvalidOperationException("Connection string 'PayRollFixContext' not found.")));

        //DI Services 
        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        builder.Services.AddScoped<IAdminRepository, AdminRepository>();
        builder.Services.AddScoped<IServicesRepository, ServiceRepository>();
        builder.Services.AddSingleton<IUserIdProvider, SessionUserIdProvider>();


        //Sessions 
        builder.Services.AddDistributedMemoryCache(); // Required for session state in memory
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
            options.Cookie.HttpOnly = true; // Make the session cookie HttpOnly
            options.Cookie.IsEssential = true; // Make it essential to be included
        });

        // Add services to the container.
        builder.Services.AddMvc();
        builder.Services.AddSignalR();
        // JWT Authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };

                // Custom event to retrieve token from cookie
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Cookies["JwtToken"]; // Get token from cookie
                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token; // Token validation
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        // Add role-based authorization policies
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
        });


        var app = builder.Build();

        app.MapHub<NotificationHub>("/notificationHub"); // Make sure this matches client-side URL

		// Configure the HTTP request pipeline.
		//string environment = Environment.GetEnvironmentVariable("APP_ENV");
		//if (environment == "dev")
		//{
		//	// Dev-specific logic
		//}
		//else if (environment == "prod")
		//{
		//	// Prod-specific logic
		//}
		if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}
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
