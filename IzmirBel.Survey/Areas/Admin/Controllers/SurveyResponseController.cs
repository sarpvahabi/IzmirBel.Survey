using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmirBel.Survey.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [EnforeStepUp]
    public class SurveyResponseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [EnforeStepUp]
        public IActionResult XXX()
        {
            return null;
        }
    }
}
