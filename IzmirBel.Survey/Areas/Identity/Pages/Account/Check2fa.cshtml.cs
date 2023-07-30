using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace IzmirBel.Survey.Areas.Identity.Pages.Account
{
    public class Check2faModel : PageModel
    {
        private const string TokenProvider = "Authenticator"; //for VertifyTwoFactorTokenAsync method's argument
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<Check2faModel> _logger;

        public Check2faModel(UserManager<IdentityUser> userManager, ILogger<Check2faModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty] //model olarak baðlamak için
        public InputModel Input { get; set; } = new();
        public string ReturnUrl { get; set; } = string.Empty;
        public class InputModel
        {
            [Required]
            [StringLength(6, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Authenticator code")]
            public string TwoFactorCode { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl)
        {
            //user already autenticated
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                throw new InvalidOperationException($"Unable to load two-factor authentication user");

            if (!user.TwoFactorEnabled)
                return RedirectToPage("/Identity/Account/Manage/EnableAuthenticator");

            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl)
        {
            if (!ModelState.IsValid) //in case of user disabled javascript
                return Page();

            // returnUrl = returnUrl ?? Url.Content("~/");

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
                throw new InvalidOperationException($"Unable to load two-factor authentication user");

            if (!user.TwoFactorEnabled)
                return RedirectToPage("/Identity/Account/Manage/EnableAuthenticator");

            var result = await _userManager.VerifyTwoFactorTokenAsync(user, TokenProvider, Input.TwoFactorCode);

            if (result)
            {
                _logger.LogInformation($"User with Id '{user.Id}' passed the 2fa check.");
                var email = await _userManager.GetUserNameAsync(user);
                HttpContext.Session.SetString(email + EnforeStepUpAttribute.StepUpAllowPathName, returnUrl.ToLower()); //review
                return LocalRedirect(returnUrl);
            }
            else
            {
                _logger.LogWarning($"Invalid authenticator code entered for user with Id '{user.Id}'.");
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return Page();
            }
        }
    }
}
