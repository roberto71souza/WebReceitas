using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Repository;
using Microsoft.Extensions.DependencyInjection;
using Dominio;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using ApiReceitas.Profiles;

namespace ApiReceitas
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
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            //Mapeamento
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProfileMapper());
            });

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);

            services.AddDbContext<ReceitasContext>(x => x.UseSqlServer(Configuration.GetConnectionString("ConnectionSQL")));

            services.AddScoped<IReceitasRepository, ReceitasApp>();

            services.AddDefaultIdentity<Usuario>(opt =>
            {
                opt.SignIn.RequireConfirmedEmail = true;

                opt.Password.RequireDigit = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequiredLength = 5;

                opt.Lockout.MaxFailedAccessAttempts = 7;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                opt.Lockout.AllowedForNewUsers = true;
            })
                .AddEntityFrameworkStores<ReceitasContext>()
                .AddSignInManager<SignInManager<Usuario>>()
                .AddDefaultTokenProviders();

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyMethod());

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
