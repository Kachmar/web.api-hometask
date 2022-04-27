using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.Models;
namespace EntityFrameWork
{
    public class StudentRepository : IRepository<Student>
    {
        public StudentRepository(string connectionString)
        {
        }

        public Student Create(Student entity)
        {
            var context = new Context();
            context.Students.Add(entity);
            context.SaveChanges();
            return entity;
        }

        public List<Student> GetAll()
        {
            using var context = new Context();
            var student = context.Students.ToList();
            context.SaveChanges();
            return student;
        }

        public Student GetById(int id)
        {
            var context = new Context();
            return context.Students.Find(id);
        }

        public void Remove(int id)
        {
            using var context = new Context();
            var course = context.Students.Find(id);
            context.Remove(course);
            context.SaveChanges();
        }

        public void Update(Student entity)
        {
            using var context = new Context();
            context.Students.Update(entity);
            context.SaveChanges();
        }
    }
}
