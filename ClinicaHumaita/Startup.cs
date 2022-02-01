using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ClinicaHumaita.Services;
using ClinicaHumaita.Data.Context;
using ClinicaHumaita.Business.Interfaces;
using ClinicaHumaita.Data.Interfaces;
using ClinicaHumaita.Data.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ClinicaHumaita.Business.Configuration;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Linq;

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

            string urlLocal = "192.168.1.4";
            string urlProd = "192.168.0.110";

            string connectionString = Configuration.GetConnectionString("Clinica").Replace("LocalURL", urlLocal).Replace("ProdURl", urlProd);

            Console.WriteLine(connectionString);

            //Adiociona context para o db
            services.AddDbContext<ClinicaContext>(options => options.UseSqlServer(connectionString));

            services.AddSwaggerGen(c =>
            {
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Insira o token JWT desta maneira: Bearer {seu token}",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            
            //Ligação entre a internface e a classe implementadora.
            services.AddTransient<IPersonService, PersonService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRabbitMQService, RabbitMQService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<ILogService, LogService>();
            services.AddTransient<IRabbitMQRecieverService, RabbitMQRecieverService>();

            services.AddTransient<IPersonRepository, PersonRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ILogRepository, LogRepository>();

            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });

                services.AddHostedService<RabbitMQRecieverService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ClinicaContext clinicaContext)
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Clinica Humaita API v1"); 
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            clinicaContext.Database.EnsureCreated();
            clinicaContext.Database.Migrate();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
