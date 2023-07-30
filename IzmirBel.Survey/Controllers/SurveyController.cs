using IzmirBel.Survey.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
                .Include(x => x.Questions)
                .FirstOrDefault(x => x.Id == id);

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

        public ActionResult SurveyCompleteMessageAdo(string id)
        {
            string message = string.Empty;

            using (var command = _surveyDbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SELECT SurveyCompleteMessage FROM CustomerSurveys WHERE Id = @id";
                command.Parameters.Add(new SqlParameter("@id", id));
                _surveyDbContext.Database.OpenConnection();
                try
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            message += reader.GetString(0) + "<br/>";
                        }
                    }
                }
                finally
                {
                    _surveyDbContext.Database.CloseConnection();
                }
            }

            return View("SurveyCompleteMessage", message);
        }
    }
}
