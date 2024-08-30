using Microsoft.EntityFrameworkCore;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Data.Context;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.IServices;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Services;

namespace PruebaFacturacionCalculosDinamicsJQueryMvcNet8
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("ConnFacturaDB"));
            });

            builder.Services.AddScoped<IFacturaService, FacturaService>();
            builder.Services.AddScoped<IClienteService, ClienteService>();
            builder.Services.AddScoped<IOrdenProductService, OrdenProductoService>();
            builder.Services.AddScoped<IProductoService, ProductoService>();

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
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

// C:\Users\MARLO\source\repos\PruebaFacturacionCalculosDinamicsJQueryMvcNet8\PruebaFacturacionCalculosDinamicsJQueryMvcNet8
// C:\Users\MARLO\source\repos\PruebaFacturacionCalculosDinamics\JQueryMvcNet8\PruebaFacturacionCalculosDinamics\JQueryMvcNet8\PruebaFacturacionCalculosDinamics\JQueryMvcNet8.csproj\Program.cs
