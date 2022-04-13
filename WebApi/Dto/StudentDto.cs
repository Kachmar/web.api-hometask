using System;
using System.Collections.Generic;
using System.Linq;
using Models.Models;

namespace WebApi.Dto
{
    public class StudentDto
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public DateTime BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string GitHubLink { get; set; }

        public string Notes { get; set; }

        public virtual List<HomeTaskAssessmentDto> HomeTaskAssessments { get; set; } = new List<HomeTaskAssessmentDto>();

        public virtual List<CourseDto> Courses { get; set; } = new List<CourseDto>();

        public Student ToModel()
        {
            return new Student()
            {
                Name = Name,
                Id = Id,
                BirthDate = BirthDate,
                PhoneNumber = PhoneNumber,
                Notes = Notes,
                Email = Email,
                GitHubLink = GitHubLink,
                Courses = Courses.Select(p => new Course
                {
                    Id = p.Id,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Name = p.Name,
                    PassCredits = p.PassCredits
                }).ToList(),
                HomeTaskAssessments = HomeTaskAssessments.Select(p => new HomeTaskAssessment()
                {
                    Id = p.Id,
                    HomeTaskId = p.HomeTaskId,
                    StudentId = p.StudentId,
                    Date = p.Date,
                    IsComplete = p.IsComplete
                }).ToList()
            };
        }

        public static StudentDto FromModel(Student student)
        {
            return new StudentDto()
            {
                Id = student.Id,
                BirthDate = student.BirthDate,
                Email = student.Email,
                GitHubLink = student.GitHubLink,
                PhoneNumber = student.PhoneNumber,
                Notes = student.Notes,
                Name = student.Name,
                Courses = student.Courses.Select(p=>new CourseDto()
                {
                    Id = p.Id,
                    StartDate = p.StartDate,
                    PassCredits = p.PassCredits,
                    EndDate = p.EndDate,
                    Name = p.Name
                }).ToList(),
                HomeTaskAssessments = student.HomeTaskAssessments.Select(p=>new HomeTaskAssessmentDto()
                {
                    Date = p.Date,
                    Id = p.Id,
                    StudentId = p.StudentId,
                    IsComplete = p.IsComplete,
                    HomeTaskId = p.HomeTaskId
                }).ToList()
            };
        }
    }

}