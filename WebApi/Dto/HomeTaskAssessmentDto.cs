
using System;
using Models.Models;

namespace WebApi.Dto
{
    public class HomeTaskAssessmentDto
    {
        public int Id { get; set; }

        public bool IsComplete { get; set; }

        public DateTime Date { get; set; }

        public int HomeTaskId { get; set; }

        public int StudentId { get; set; }
        
        public HomeTaskAssessment ToModel()
        {
            return new HomeTaskAssessment()
            {
                Id = Id,
                IsComplete = IsComplete,
                Date = Date,
                StudentId = StudentId,
                HomeTaskId = HomeTaskId
            };
        }

        public static HomeTaskAssessmentDto FromModel(HomeTaskAssessment homeTaskAssessment)
        {
            return new HomeTaskAssessmentDto()
            {
                Date = homeTaskAssessment.Date,
                StudentId = homeTaskAssessment.StudentId,
                IsComplete = homeTaskAssessment.IsComplete,
                Id = homeTaskAssessment.Id,
                HomeTaskId = homeTaskAssessment.HomeTaskId
            };
        }
    }
}