using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WI.Models.Web;

namespace WI.Data.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            //string TempAdminEmail = "test@gmail.com";

            //_userManager.CreateAsync(new ApplicationUser
            //{
            //    UserName = TempAdminEmail,
            //    Email = TempAdminEmail,
            //    EmailConfirmed = true,

            //}, "1").GetAwaiter().GetResult();
            //ApplicationUser testuser = _db.ApplicationUsers.Where(u => u.Email == TempAdminEmail).FirstOrDefault();
            //_userManager.AddToRoleAsync(testuser, SD.Admin).GetAwaiter().GetResult();


            //// Admin User
            //string localAdminEmail = "web@kimengineering.com";
            //if (_db.ApplicationUsers.Any(r => r.Email== localAdminEmail)) return;
            //_userManager.CreateAsync(new ApplicationUser
            //{
            //    UserName = localAdminEmail,
            //    Email = localAdminEmail,
            //    EmailConfirmed = true,

            //}, "@Web2021").GetAwaiter().GetResult();

            //ApplicationUser user = _db.ApplicationUsers.Where(u => u.Email == localAdminEmail).FirstOrDefault();
            //_userManager.AddToRoleAsync(user, SD.Admin).GetAwaiter().GetResult();

            //// Task
            //if (_db.FunctionModel.Count() == 0)
            //{
            //    SaveFunction(new FunctionModel() { Id = SD.FuncTask, Title = "Task" });
            //    SaveFunction(new FunctionModel() { Id = SD.FuncCode, Title = "Code" });
            //    SaveFunction(new FunctionModel() { Id = SD.FuncCustomer, Title = "Customer" });
            //    SaveFunction(new FunctionModel() { Id = SD.FuncBlog, Title = "Blog" });
            //    SaveFunction(new FunctionModel() { Id = SD.FuncTimesheet, Title = "Timesheet" });
            //    SaveFunction(new FunctionModel() { Id = SD.FuncBible, Title = "Bible" });
            //}

            //string AdminEmail = "info@websiteinnovator.com";

            //if (_db.Roles.Any(r => r.Name == SD.AdminSuper)) return;
            //_roleManager.CreateAsync(new IdentityRole(SD.AdminSuper)).GetAwaiter().GetResult();
            //ApplicationUser user3 = _db.ApplicationUsers.Where(u => u.Email == AdminEmail).FirstOrDefault();
            //_userManager.AddToRoleAsync(user3, SD.AdminSuper).GetAwaiter().GetResult();

            //if (_db.Roles.Any(r => r.Name == SD.Admin)) return;

            //_roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();


            //_userManager.CreateAsync(new ApplicationUser
            //{
            //    UserName = AdminEmail,
            //    Email = AdminEmail,
            //    EmailConfirmed = true,

            //}, "Test!1234").GetAwaiter().GetResult();

            //ApplicationUser user2 = _db.ApplicationUsers.Where(u => u.Email == AdminEmail).FirstOrDefault();
            //_userManager.AddToRoleAsync(user2, SD.Admin).GetAwaiter().GetResult();

        }
    }
}
