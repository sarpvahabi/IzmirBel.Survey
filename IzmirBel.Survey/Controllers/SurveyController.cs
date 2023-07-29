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


        [HttpPost]
        public async Task<ActionResult> CompleteSurvey(Guid id, SurveyAnswer[] questions)
        {
            CustomerSurvey? customerSurvey = _surveyDbContext.CustomerSurveys
                .Include(x => x.Questions).FirstOrDefault(x => x.Id == id);

            CustomerSurveyResponse customerSurveyResponse = new CustomerSurveyResponse(id, questions.ToList());
            foreach (SurveyAnswer answer in questions)
            {
                answer.SurveyResponse = customerSurveyResponse;
            }

            _surveyDbContext.CustomerSurveysResponses.Add(new CustomerSurveyResponse(id, questions.ToList()));

            await _surveyDbContext.SaveChangesAsync();

            return View(viewName: "SurveyComplete", model: customerSurvey);
        }
    }
}
