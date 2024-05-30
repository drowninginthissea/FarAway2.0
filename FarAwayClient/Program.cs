using FarAwayClient.Models;
using FarAwayClient.Services;
using FarAwayClient.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace FarAwayClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // logger settings
            builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
            builder.Logging.AddFilter("System.Net.Http.HttpClient.Geocoder", LogLevel.Warning);

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

            // another custom services -----

            // hash service for working with user passwords
            builder.Services.AddTransient<HashService>();

            // cors settings for opening a local server on a local network
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            builder.Services.AddCors(option =>
            {
                option.AddPolicy(name: MyAllowSpecificOrigins, policy =>
                {
                    policy.WithOrigins("http://192.168.0.100:7191").AllowAnyMethod().AllowAnyHeader();
                });
            });

            // geocoder service for working with Yandex Geocoder API
            builder.Services.AddHttpClient<Geocoder>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            var cultureInfo = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            app.UseSession();


            app.Use(async (context, next) =>
            {
                context.Session.SetInt32(Literals.UserSessionKey, 1);
                await next.Invoke();
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.MapRazorPages();

            app.Run();
        }
    }
}
