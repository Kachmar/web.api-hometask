using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Models;
using Models.Models;

namespace ADO.NET
{
    class Program
    {
        private static string _connectionString;
        static void Main(string[] args)
        {
            SetConnectionString();

            IRepository<Course> courseRepository = GetCourseRepository();
            Course newCourse = new Course() { StartDate = DateTime.Now, EndDate = DateTime.Now, PassCredits = 500 };

            var course = courseRepository.Create(newCourse);
            course.PassCredits = 1000;

            courseRepository.Update(course);

            IRepository<Student> studentRepository = GetStudentRepository();
            Student newStudent = new Student()
            {
                BirthDate = DateTime.Now,
                Email = "Test",
                GitHubLink = "Test",
                Name = "test",
                PhoneNumber = "000"
            };
            var insertedStudent = studentRepository.Create(newStudent);

            //Update

            insertedStudent.Notes += "; Is employed";
            insertedStudent.Courses.Add(course);

            studentRepository.Update(insertedStudent);

            var homeTaskRepository = GetHomeTaskRepository();
            var homeTaskAssessmentRepository = GetHomeTaskAssessmentRepository();



            HomeTask homeTask = new HomeTask()
            {
                Course = course,
                CourseId = course.Id,
                Date = DateTime.Now,
                Title = "First By Program"
            };
            homeTask = homeTaskRepository.Create(homeTask);
            HomeTaskAssessment homeTaskAssessment = new HomeTaskAssessment()
            { Date = DateTime.Now, StudentId = insertedStudent.Id, HomeTaskId = homeTask.Id };
            homeTaskAssessment = homeTaskAssessmentRepository.Create(homeTaskAssessment);

            homeTask.Title += " Updated";
            homeTask.HomeTaskAssessments.Add(homeTaskAssessment);
            homeTaskAssessment.IsComplete = true;
            homeTaskAssessmentRepository.Update(homeTaskAssessment);
            homeTaskAssessment.Date = new DateTime(2020, 1, 1);

            homeTaskRepository.Update(homeTask);

            var result = homeTaskAssessmentRepository.GetById(homeTaskAssessment.Id);
            var homeTaskResult = homeTaskRepository.GetById(homeTask.Id);

            homeTaskAssessmentRepository.Remove(homeTaskAssessment.Id);
            homeTaskRepository.Remove(homeTask.Id);
            studentRepository.Remove(insertedStudent.Id);
            courseRepository.Remove(course.Id);
        }

        private static IRepository<Course> GetCourseRepository()
        {
            return new CourseRepository(_connectionString);
        }

        private static IRepository<Student> GetStudentRepository()
        {
            return new StudentRepository(_connectionString);
        }
        private static IRepository<HomeTask> GetHomeTaskRepository()
        {
            return new HomeTaskRepository(_connectionString);
        }
        private static IRepository<HomeTaskAssessment> GetHomeTaskAssessmentRepository()
        {
            return new HomeTaskAssessmentRepository(_connectionString);
        }

        private static void SetConnectionString()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile($"appsettings.json", true, true);
            var configuration = builder.Build();
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
    }
}