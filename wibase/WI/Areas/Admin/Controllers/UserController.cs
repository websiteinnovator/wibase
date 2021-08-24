using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using WI.Models.Web;
using WI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WI.Areas.Admin.Controllers
{
    [Area(SD.RoleAdmin)]
    [Authorize(Roles = SD.RoleAdmin)]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<UsersController> _logger;
        private readonly ApplicationDbContext _context;

        public UsersController(SignInManager<ApplicationUser> signInManager,
            ILogger<UsersController> logger,
            UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
        }

        public ActionResult Index(string sve)
        {
            //List<ApplicationUser> allUsers = new List<ApplicationUser>();
            ViewBag.allUsers = _context.Users.ToList();
            ViewBag.Roles = _context.Roles.ToList();
            ViewBag.UserManager = _userManager;

            var user = User.Identity;
            ViewBag.Name = user.Name;
            return View();
        }

        #region Edit User Guid

        public async Task GetViewBag(string username)
        {
            ViewBag.Name = await _context.Roles.Select(x => new SelectListItem { Value = x.Name, Text = x.Name }).ToListAsync();
            var user = await _userManager.FindByNameAsync(username);
            ViewBag.MyRoles = await _userManager.GetRolesAsync(user);
        }

        // GET: Users/Edit/Guid
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = new ApplicationUser();
            var resultModel = await _userManager.FindByIdAsync(id);
            ApplicationUser usersdetail = new ApplicationUser();

            usersdetail.FirstName = resultModel.FirstName;
            usersdetail.Id = resultModel.Id;
            usersdetail.LastName = resultModel.LastName;
            usersdetail.Email = resultModel.Email;

            if (resultModel == null)
            {
                return NotFound();
            }
            await GetViewBag(resultModel.UserName);
            return View(usersdetail);
        }

        /// <summary>
        /// Edit user 
        /// </summary>
        /// <param name="User Roles"></param>
        /// <returns></returns>
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ApplicationUser EditUserdetail, string[] RolesList)
        {
            if (ModelState.IsValid)
            {
                var userid = EditUserdetail.Id;
                var user = await _userManager.FindByIdAsync(userid);

                // Update it with the values from the view model
                user.FirstName = EditUserdetail.FirstName;
                user.LastName = EditUserdetail.LastName;
                user.Email = EditUserdetail.Email;
                // Remove Roles
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
                // Add Role
                //await this.UserManager.AddToRoleAsync(user.Id, EditUserdetail.UserRole);

                if (RolesList != null)
                {
                    foreach (string role in RolesList)
                    {
                        await _userManager.AddToRoleAsync(user, role);
                    }
                }

                //foreach (var myRole in user.Roles)
                //{
                //    await UserManager.RemoveFromRolesAsync(EditUserdetail.Id, user.Roles);
                //}
                //await this.UserManager.RemoveFromRolesAsync(user.Id);
                //await this.UserManager.AddToRoleAsync(user.Id, EditUserdetail.UserRole);

                await _userManager.UpdateAsync(user);
                TempData["messageType"] = "up";
                return RedirectToAction("Index");
            }
            else
            {
                await GetViewBag(EditUserdetail.UserName);
                return View();
            }
        }
        #endregion

        #region Delete User
        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>

        //
        // GET: /Roles/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return NotFound();
                }
                var role = await _userManager.FindByIdAsync(id);
                var result = await _userManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First().ToString());
                    return View();
                }
                TempData["messageType"] = "del";
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        #endregion
    }
}