using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WI.Models;
using log4net;
using WI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using WI.Models.Web;

namespace WI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginModel(SignInManager<ApplicationUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager, ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public void SetCookie(string key, string value)
        {
            CookieOptions option = new CookieOptions();

            option.Expires = DateTime.Now.AddDays(1);

            //if (expireTime.HasValue)
            //    option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            //else
            //    option.Expires = DateTime.Now.AddMilliseconds(10);

            Response.Cookies.Append(key, value, option);
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            ILog log = LogManager.GetLogger("mylog");
            log.Info("Login-In");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                log.Info("Login: " + result.ToString());

                //// Get SiteId for User
                //var mySiteModel = await _context.SiteUserModel.Where(s => s.Email == Input.Email && s.SiteModel.d_expire >= DateTime.Today).Select(s=>s.SiteModelId).ToListAsync();
                //if (mySiteModel == null || mySiteModel.Count() == 0)
                //{
                //    ModelState.AddModelError(string.Empty, "Invalid login attempt (no site).");
                //    return Page();
                //}
                //// Save in Cookies
                //SetCookie(SD.SessionSiteIdListText, mySiteModel.SiteModelId.ToString());


                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");

                    //if (mySiteModel.Count() == 1) // 1 site => go to site
                    //{
                    //    // Save in Sessions
                    //    HttpContext.Session.SetObjectAsJson(SD.SessionSiteIdListText, mySiteModel);
                    //    return LocalRedirect(returnUrl);
                    //}
                    //// Multiple sites : Check cookie and if not, redirect to cookie page
                    ////string cookieValueFromContext = _httpContextAccessor.HttpContext.Request.Cookies[SD.SessionSiteIdListText];
                    ////read cookie from Request object  
                    //if (Request.Cookies[SD.SessionSiteIdListText] == null)
                    //{
                    //    // Save in Sessions
                    //    HttpContext.Session.SetObjectAsJson(SD.SessionSiteIdListText, mySiteModel);
                    //    return Redirect("/home/sites");
                    //}
                    //else
                    //{
                    //    string cookieValueFromReq = Request.Cookies[SD.SessionSiteIdListText];
                    //    List<string> myList = new List<string>();
                    //    if (cookieValueFromReq.Contains(","))
                    //    {
                    //        foreach (var mySiteId in cookieValueFromReq.Split(','))
                    //        {
                    //            myList.Add(mySiteId);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        myList.Add(cookieValueFromReq);
                    //    }
                    //    HttpContext.Session.SetObjectAsJson(SD.SessionSiteIdListText, myList);
                    //}

                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            log.Info("Login-Out");
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
