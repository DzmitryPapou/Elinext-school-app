using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.UI.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace School.UI
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSession();

           
            services.AddDbContext<SchoolContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<SchoolContext>();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseSession();
            app.UseMvc();
           

            CreateUserAndRoles(serviceProvider).Wait();
        }



        public async Task CreateUserAndRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var adminCheck = await roleManager.RoleExistsAsync("Administrator");
            if (!adminCheck)
            {
                await roleManager.CreateAsync(new IdentityRole("Administrator"));

                var admin = await userManager.FindByEmailAsync("admin@gmail.com");
                if (admin is null)
                {
                    var user = new IdentityUser()
                    {
                        UserName = "admin@gmail.com",
                        Email = "admin@gmail.com"
                    };

                    await userManager.CreateAsync(user, "Admin_12345");
                    await userManager.AddToRoleAsync(user, "Administrator");

                }


                var teacherCheck = await roleManager.RoleExistsAsync("Teacher");
                if (!teacherCheck)
                {
                    await roleManager.CreateAsync(new IdentityRole("Teacher"));

                    var teacher = await userManager.FindByEmailAsync("teacher@gmail.com");
                    if (teacher is null)
                    {
                        var user = new IdentityUser()
                        {
                            UserName = "teacher@gmail.com",
                            Email = "teacher@gmail.com"
                        };

                        await userManager.CreateAsync(user, "Teacher_12345");
                        await userManager.AddToRoleAsync(user, "Teacher");

                    }

                }
            }
        }
    }
}






