using System;
using Models.Models;
using System.Collections.Generic;
using System.Linq;
using Models;
using Services.Validators;

namespace Services
{
    public class StudentService
    {
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<HomeTaskAssessment> _homeTaskAssessmentRepository;

        public StudentService()
        {

        }

        public StudentService(IRepository<Student> studentRepository,
            IRepository<HomeTaskAssessment> homeTaskAssessmentRepository)
        {
            _studentRepository = studentRepository;
            _homeTaskAssessmentRepository = homeTaskAssessmentRepository;
        }

        public virtual List<Student> GetAllStudents()
        {
            return _studentRepository.GetAll();
        }

        public virtual Student GetStudentById(int studentId)
        {
            return _studentRepository.GetById(studentId);
        }

        public virtual ValidationResponse UpdateStudent(Student student)
        {
            ValidationResponse<Student> response = ValidateStudent(student);
            if (response.HasErrors)
            {
                return response;
            }

            _studentRepository.Update(student);
            return new ValidationResponse();
        }

        private ValidationResponse<Student> ValidateStudent(Student student)
        {
            if (student == null)
            {
                return new ValidationResponse<Student>("student", "Student cannot be null");
            }

            return new ValidationResponse<Student>(student);
        }

        public virtual void DeleteStudent(int id)
        {
            var student = _studentRepository.GetById(id);
            if (student == null)
            {
                throw new ArgumentException($"Cannot find student with id '{id}'");
            }

            foreach (var studentHomeTaskAssessment in student.HomeTaskAssessments.ToArray())
            {
                _homeTaskAssessmentRepository.Remove(studentHomeTaskAssessment.Id);
            }

            _studentRepository.Remove(id);
        }

        public virtual ValidationResponse<Student> CreateStudent(Student student)
        {
            ValidationResponse<Student> response = ValidateStudent(student);
            if (response.HasErrors)
            {
                return response;
            }
            var all = _studentRepository.GetAll();

            if (all.Any(p => p.Email == student.Email))
            {
                return new ValidationResponse<Student>("email", $"Student with email '{student.Email}' already exists.");
            }
            var newStudent = _studentRepository.Create(student);
            return new ValidationResponse<Student>(newStudent);
        }
    }
}
