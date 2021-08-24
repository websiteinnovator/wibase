using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using WI.Data;
using WI.Data.Initializer;
using WI.Models.Web;

namespace WI
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            CurrentEnvironment = env;

        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment CurrentEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddDefaultTokenProviders()
                .AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            //services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true);



            //services.Configure<IdentityOptions>(options =>
            //{
            //    // Password settings.
            //    options.Password.RequireDigit = true;
            //    options.Password.RequireLowercase = true;
            //    options.Password.RequireNonAlphanumeric = true;
            //    options.Password.RequireUppercase = true;
            //    options.Password.RequiredLength = 6;
            //    options.Password.RequiredUniqueChars = 1;

            //    // Lockout settings.
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //    options.Lockout.MaxFailedAccessAttempts = 5;
            //    options.Lockout.AllowedForNewUsers = true;

            //    // User settings.
            //    options.User.AllowedUserNameCharacters =
            //    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            //    options.User.RequireUniqueEmail = false;
            //});
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<WIContext>(options =>
                options.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("WI.Models")));

            services.AddRazorPages()
                .AddSessionStateTempDataProvider();
            IMvcBuilder builder = services.AddControllersWithViews()
                .AddSessionStateTempDataProvider();


            if (CurrentEnvironment.IsDevelopment())
            {
                builder.AddRazorRuntimeCompilation();
            }

            if (!CurrentEnvironment.IsDevelopment())
            {
                services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            }

            services.AddScoped<IDbInitializer, DbInitializer>();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddAuthentication().AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => { options.ExpireTimeSpan = System.TimeSpan.FromHours(24); });
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            var keysFolder = Path.Combine(CurrentEnvironment.ContentRootPath, "Keys");

            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
                .SetApplicationName("MyWebsite")
                .SetDefaultKeyLifetime(TimeSpan.FromDays(90));

        }

        private async Task DeveloperLogin(HttpContext httpContext)
        {

            var UserManager = httpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
            var signInManager = httpContext.RequestServices.GetRequiredService<SignInManager<ApplicationUser>>();

            var _user = await UserManager.FindByEmailAsync("info@websiteinnovator.com");

            await signInManager.SignInAsync(_user, isPersistent: false);

            //// Save in Sessions
            //List<string> myList = new List<string>();
            //    myList.Add("51442cc0-2780-4dce-a3f9-fe94172eaa77");
            //httpContext.Session.SetObjectAsJson(SD.SessionSiteIdListText, myList);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Microsoft.Extensions.Logging.ILoggerFactory loggerFactory, IDbInitializer dbInit)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

                //app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            dbInit.Initialize();

            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();
            //if (env.IsDevelopment())
            //{
            //    app.Use(async (context, next) =>
            //    {
            //        var user = context.User.Identity.Name;
            //        DeveloperLogin(context).Wait();
            //        await next.Invoke();
            //    });
            //}
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "myArea",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            loggerFactory.AddLog4Net();

        }
    }
}
