using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IzmirBel.Survey.Areas.Admin.Controllers
{
    
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<IdentityUser> userManager, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "CanSetAdmins")]
        public async Task<IActionResult> SetAdmin(Guid userId)
        {
            var loggedInUser = await _userManager.GetUserAsync(HttpContext.User);
            var targetUser = await _userManager.FindByIdAsync(userId.ToString());
            await _userManager.AddToRoleAsync(targetUser, "Administrator");

            _logger.LogWarning("User {loggedInUser} has set user {targetUser} as admin.", loggedInUser, targetUser.Id);

            return null;
        }
    }
}