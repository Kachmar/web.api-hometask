using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Models.Models;
using Services.Validators;

namespace Services
{
    public class CourseService
    {
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<HomeTask> _homeTaskRepository;
        private readonly IRepository<HomeTaskAssessment> _homeTaskAssessmentRepository;

        //public CourseService()
        //{

        //}

        public CourseService(IRepository<Course> courseRepository,
            IRepository<Student> studentRepository,
            IRepository<HomeTask> homeTaskRepository,
            IRepository<HomeTaskAssessment> homeTaskAssessmentRepository)
        {
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
            _homeTaskRepository = homeTaskRepository;
            _homeTaskAssessmentRepository = homeTaskAssessmentRepository;
        }

        public virtual List<Course> GetAllCourses()
        {
            return _courseRepository.GetAll();
        }

        public virtual void DeleteCourse(int id)
        {
            var course = _courseRepository.GetById(id);
            if (course == null)
            {
                throw new ArgumentException($"Cannot find course with id '{id}'");
            }

            foreach (var homeTask in course.HomeTasks.ToArray())
            {
                foreach (var homeTaskHomeTaskAssessment in homeTask.HomeTaskAssessments.ToArray())
                {
                    _homeTaskAssessmentRepository.Remove(homeTaskHomeTaskAssessment.Id);
                }
                _homeTaskRepository.Remove(homeTask.Id);
            }

            _courseRepository.Remove(id);
        }

        public virtual Course GetCourseById(int id)
        {
            return _courseRepository.GetById(id);
        }

        public virtual ValidationResponse UpdateCourse(Course course)
        {
            ValidationResponse<Course> response = ValidateCourse(course);
            if (response.HasErrors)
            {
                return response;
            }

            _courseRepository.Update(course);
            return new ValidationResponse();
        }

        public virtual ValidationResponse<Course> CreateCourse(Course course)
        {
            ValidationResponse<Course> response = ValidateCourse(course);
            if (response.HasErrors)
            {
                return response;
            }
            var all = _courseRepository.GetAll();

            if (all.Any(p => p.Name == course.Name))
            {
                return new ValidationResponse<Course>("name", $"course with name '{course.Name}' already exists.");
            }
            var newCourse = _courseRepository.Create(course);
            return new ValidationResponse<Course>(newCourse);
        }

        public virtual void SetStudentsToCourse(int courseId, IEnumerable<int> studentIds)
        {
            var course = _courseRepository.GetById(courseId);
            if (course == null)
            {
                throw new ArgumentException($"There is no course with id '{courseId}'");
            }
            course.Students.Clear();
            foreach (var studentId in studentIds)
            {
                var student = _studentRepository.GetById(studentId);
                if (student == null)
                {
                    throw new ArgumentException($"Cannot find student with id '{studentId}'");
                }
                course.Students.Add(student);
            }
            _courseRepository.Update(course);
        }

        private ValidationResponse<Course> ValidateCourse(Course course)
        {
            if (course == null)
            {
                return new ValidationResponse<Course>("course", "Course cannot be null");
            }

            if (course.StartDate > course.EndDate)
            {
                return new ValidationResponse<Course>(nameof(course.StartDate), "Start date cannot be greater than end date!");
            }
            
            return new ValidationResponse<Course>(course);
        }
    }
}
