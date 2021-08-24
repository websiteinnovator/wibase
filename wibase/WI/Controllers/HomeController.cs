using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WI.Data;
using Microsoft.Extensions.Options;
using WI.Models.Web;

namespace WI.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private AppSettings _setting { get; set; }

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, IOptions<AppSettings> settings)
        {
            _context = context;
            _logger = logger;
            _setting = settings.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(VaryByHeader = "User-Agent", Duration = 3600)]
        public string UserInitial(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return "";
            }

            var user = _context.ApplicationUsers.Where(a=>a.Email == id).FirstOrDefault();
            if (user == null)
            {
                return "";
            }
            return $"{user.Initial}";
            //return $"{user.Initial} {user.GetImage("img-profile rounded-circle")}";
        }

        [ResponseCache(VaryByHeader = "User-Agent", Duration = 3600)]
        public string UserInitialOnly(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return "";
            }

            var user = _context.ApplicationUsers.Where(a => a.Email == id).FirstOrDefault();
            if (user == null)
            {
                return "";
            }
            return $"{user.Initial}";
        }

    }
}
