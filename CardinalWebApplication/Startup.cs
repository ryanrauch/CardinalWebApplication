using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CardinalWebApplication.Data;
using CardinalWebApplication.Models;
using CardinalWebApplication.Services;
using CardinalWebApplication.Services.Interfaces;
using CardinalWebApplication.Models.DbContext;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CardinalWebApplication
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration[Constants.CardinalAppDbConnection]));
                //options.UseSqlServer(Configuration.GetConnectionString(Constants.CardinalAppDbConnection)));

            services.AddIdentity<ApplicationUser, IdentityRole>(/*config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
            }*/)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IHexagonal, HexagonalEquilateralScale>();
            services.AddTransient<ILocationHistoryService, LocationHistoryService>();

            services.AddSingleton<IZoneBoundaryService, ZoneBoundaryService>();
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Jwt";
                options.DefaultChallengeScheme = "Jwt";
            })
            .AddJwtBearer("Jwt", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    //ValidAudience = "the audience you want to validate",
                    ValidateIssuer = false,
                    //ValidIssuer = "the isser you want to validate",
                    ValidateIssuerSigningKey = true,
                    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.JwtSecretNeedsToBeSecured)),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration[Constants.JwtSecretKey])),
                    ValidateLifetime = true, //validate the expiration and not before values in the token
                    ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
                };
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
