using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.Models;
namespace EntityFrameWork
{
    public class CourseRepository : IRepository<Course>
    {
         public CourseRepository(string connectionString) 
        {
        }
        public Course Create(Course entity)
        {
            var context = new Context();
            context.Courses.Add(entity);
            context.SaveChanges();
            return entity;
        }

        public List<Course> GetAll()
        {
            using var context = new Context();
            return context.Courses.ToList();
            
        }

        public Course GetById(int id)
        {
            using var context = new Context();
            return context.Courses.Find(id);
        }

        public void Remove(int id)
        {
            using var context = new Context();
            var course = context.Courses.Find(id);
            context.Remove(course);
            context.SaveChanges();
        }

        public void Update(Course entity)
        {
            using var context = new Context();
            context.Courses.Update(entity);
            context.SaveChanges();
        }
    }
}
