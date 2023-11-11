using FHIR_MIS_web.Interfaces;
using FHIR_MIS_web.Repositories;
using FHIR_MIS_web.Data;
using Serilog;

namespace FHIR_MIS_web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IServerPatientRepository, ServerPatientRepository>();
            builder.Services.Configure<FhirServerSettings>(builder.Configuration
                .GetSection("FhirServerSettings")
                .GetSection("MyServer"));

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("logs/json/log-.json", rollingInterval: RollingInterval.Day)
                .WriteTo.File("logs/txt/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

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
                pattern: "{controller=Patient}/{action=Index}/{id?}");

            app.Run();
        }
    }
}