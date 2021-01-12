using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Iep.Models.Database;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Iep.Factories;
using Iep.Hubs;

namespace Iep
{
    public class Startup
    {
        public Startup(IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services )
        {
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddDbContext<AuctionContext> ( 

                options => options.UseSqlServer ( this.Configuration.GetConnectionString ( "AukcijaDB" ) )
            );
            services.AddControllersWithViews();
                services.AddIdentity<User, IdentityRole>(
                options => {
                    
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 8;
                }


            ).AddEntityFrameworkStores<AuctionContext>();

             services.AddAutoMapper(typeof(Startup));

            services.ConfigureApplicationCookie(
                options => {
                    options.LoginPath = "/User/LogIn";
                    options.AccessDeniedPath = "/Home/Error";
                    
                }
            );

            services.AddSignalR();

            services.AddScoped<IUserClaimsPrincipalFactory<User>, ClaimFactory>();
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

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<AuctionHub> ("/update");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
