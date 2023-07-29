using IzmirBel.Survey.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IzmirBel.Survey.Controllers
{
    public class SurveyController : Controller
    {
        private readonly SurveyDbContext _surveyDbContext;

        public SurveyController(SurveyDbContext surveyDbContext)
        {
            _surveyDbContext = surveyDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TakeSurvey(Guid id)
        {
            var customerSurvey = _surveyDbContext.CustomerSurveys
                .Include(x=> x.Questions)
                .FirstOrDefault(x=> x.Id == id);

            return View(customerSurvey); //ViewModel eklemedik
        }

        public async Task<ActionResult> CompleteSurvey(Guid id, SurveyAnswer[] questions)
        {
            var customerSurvey = _surveyDbContext.CustomerSurveys
                .Include(x => x.Questions)
                .FirstOrDefault(x => x.Id == id);

            var customerSurveyResponse = new CustomerSurveyResponse(id, questions.ToList());

            foreach (var answer in questions)
                answer.SurveyResponse = customerSurveyResponse;

            _surveyDbContext.CustomerSurveysResponses.Add(new CustomerSurveyResponse(id, questions.ToList()));

            await _surveyDbContext.SaveChangesAsync();

            return View("SurveyComplete", customerSurvey);

        }
    }
}
