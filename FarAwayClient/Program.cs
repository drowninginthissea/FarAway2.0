using FarAwayClient.Models;
using FarAwayClient.Services;
using FarAwayClient.Tools;
using Microsoft.EntityFrameworkCore;

namespace FarAwayClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // razor pages services
            builder.Services.AddRazorPages();

            // session services
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromHours(12);
                options.Cookie.Name = ".FarAway2.0.Session";
                options.Cookie.IsEssential = true;
            });

            // database services and settings
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<Db>(
                options => options.UseSqlServer(connectionString).UseLazyLoadingProxies());

            // another custom services
            builder.Services.AddTransient<HashService>();
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            builder.Services.AddCors(option =>
            {
                option.AddPolicy(name: MyAllowSpecificOrigins, policy =>
                {
                    policy.WithOrigins("http://192.168.0.10:7191").AllowAnyMethod().AllowAnyHeader();
                });
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseSession();


            //app.Use(async (context, next) =>
            //{
            //    context.Session.SetInt32(Literals.UserSessionKey, 1);
            //    await next.Invoke();
            //});

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.MapRazorPages();

            app.Run();
        }
    }
}
