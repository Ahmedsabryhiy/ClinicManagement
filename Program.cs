using Application.Contracts;
using Application.MappingProfiles;
using Application.Services;
using Infrastructure.Contracts;
using Infrastructure.DbContexts;
using Infrastructure.Repositories;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using  Serilog;

namespace ClinicManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddAutoMapper(typeof(MappingProfiles));
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ClinicManagementContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            Log.Logger = new LoggerConfiguration()
                  .WriteTo.Console()
                .WriteTo.MSSqlServer(
                    connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
                    tableName: "Log",
                    autoCreateSqlTable: true)
                .CreateLogger();

            builder.Host.UseSerilog();
            // Register repositories and services
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IPatient, PatientService>();
            builder.Services.AddScoped<IDoctor, DoctorService>();
            builder.Services.AddScoped<IAppointmentMedicalService,AppointmentMedicalService  >();

            builder.Services.AddScoped<IAppointment, AppointmentService>();
            builder.Services.AddScoped<IInvoice, InvoiceService>();
         
            builder.Services.AddScoped<IMedicalService, MedicalService>();
            builder.Services.AddScoped<IAppointmentMedicalService, AppointmentMedicalService>();


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

            #region Routing
#pragma warning disable ASP0014 // Suggest using top level route registrations
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "admin",
                pattern: "{area:exists}/{controller=Home}/{action=Index}");

                //endpoints.MapControllerRoute(
                //name: "LandingPages",
                //pattern: "{area:exists}/{controller=Home}/{action=Index}");

                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

                //endpoints.MapControllerRoute(
                //name: "ali",
                //pattern: "ali/{controller=Home}/{action=Index}/{id?}");



            }
            );
#pragma warning restore ASP0014 // Suggest using top level route registrations
            #endregion

            app.Run();
        }
    }
}
