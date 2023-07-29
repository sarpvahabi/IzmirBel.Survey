using System.ComponentModel.DataAnnotations;

namespace IzmirBel.Survey.Models
{
    public class CustomerSurvey
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string SurveyCompleteMessage { get; set; } = string.Empty;
        public List<SurveyQuestion> Questions { get; set; } = null!;

        public CustomerSurvey()
        {
                
        }

        public CustomerSurvey(Guid id, string title, string surveyCompleteMessage)
        {
            Id = id;
            Title = title;
            SurveyCompleteMessage = surveyCompleteMessage;
        }
    }
}
