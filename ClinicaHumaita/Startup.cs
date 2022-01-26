using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ClinicaHumaita.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using ClinicaHumaita.Data.Context;
using ClinicaHumaita.Business.Interfaces;
using ClinicaHumaita.Data.Interfaces;
using ClinicaHumaita.Data.Repository;

namespace ClinicaHumaita
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddSession();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
            });

            string urlLocal = string.Empty;
            string urlProd = string.Empty;
            urlLocal = "192.168.1.4";
            urlProd = "192.168.1.4";

            string connectionString = Configuration.GetConnectionString("Clinica").Replace("LocalURL", urlLocal).Replace("ProdURl", urlProd);

            Console.WriteLine(connectionString);

            //Adiociona context para o db
            services.AddDbContext<ClinicaContext>(options => options.UseSqlServer(connectionString));

            //Ligação entre a internface e a classe implementadora.
            services.AddScoped<IPersonServices, PersonService>();
            services.AddScoped<IUserServices, UserServices>();

            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            //Referencia ao autenticador, redirecionando pro login 
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                   .AddCookie(options =>
                   {
                       options.LoginPath = "/Login/";
                   });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
