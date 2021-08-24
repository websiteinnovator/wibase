using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using WI.Models.Web;
using WI.Data;
using Microsoft.AspNetCore.Authorization;

namespace WI.Areas.Admin.Controllers
{
    [Area(SD.RoleAdmin)]
    [Authorize(Roles = SD.RoleAdmin)]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private RoleManager<IdentityRole> roleManager;
        private UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(RoleManager<IdentityRole> roleMgr, UserManager<ApplicationUser> userMrg, ILogger<AdminController> logger, ApplicationDbContext context)
        {
            roleManager = roleMgr;
            userManager = userMrg;
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Users()
        {
            return View(userManager.Users.ToList());
        }

    }
}
