using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IzmirBel.Survey.Models
{
    public class SurveyQuestion
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(SurveyId))]
        public CustomerSurvey? Survey { get; set; }
        public Guid SurveyId { get; set; }
        [Required]
        public string Question { get; set; }
        public string Answer { get; set; } = string.Empty;
        public string PossibleAnswers { get; set; }

        public SurveyQuestion(Guid surveyId, Guid id, string question, string possibleAnswers = "")
        {
            SurveyId = surveyId;
            Id = id;
            Question = question;
            PossibleAnswers = possibleAnswers;
        }

    }
}