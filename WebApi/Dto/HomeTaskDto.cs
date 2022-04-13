
using System;
using System.Collections.Generic;
using System.Linq;
using Models.Models;

namespace WebApi.Dto
{
    public class HomeTaskDto
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Number { get; set; }

        public int CourseId { get; set; }

        public virtual List<HomeTaskAssessmentDto> HomeTaskAssessments { get; set; } = new List<HomeTaskAssessmentDto>();

        public HomeTask ToModel()
        {
            return new HomeTask()
            {
                Id = Id,
                Title = Title,
                Date = Date,
                Description = Description,
                Number = Number,
                CourseId = CourseId,
                HomeTaskAssessments = HomeTaskAssessments.Select(p => new HomeTaskAssessment
                {
                    Date = p.Date,
                    HomeTaskId = p.HomeTaskId,
                    Id = p.Id,
                    IsComplete = p.IsComplete,
                    StudentId = p.StudentId
                }).ToList()
            };
        }

        public static HomeTaskDto FromModel(HomeTask homeTask)
        {
            return new HomeTaskDto()
            {
                Title = homeTask.Title,
                Id = homeTask.Id,
                Date = homeTask.Date,
                Description = homeTask.Description,
                Number = homeTask.Number,
                CourseId = homeTask.CourseId,
                HomeTaskAssessments = homeTask.HomeTaskAssessments.Select(p => new HomeTaskAssessmentDto()
                {
                    Id = p.Id,
                    Date = p.Date,
                    StudentId = p.StudentId,
                    IsComplete = p.IsComplete,
                    HomeTaskId = p.HomeTaskId
                }).ToList()
            };
        }
    }
}