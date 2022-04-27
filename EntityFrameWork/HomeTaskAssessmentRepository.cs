using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.Models;
namespace EntityFrameWork
{
    public class HomeTaskAssessmentRepository : IRepository<HomeTaskAssessment>
    {
        public HomeTaskAssessmentRepository(string connectionString)
        {
        }

        public HomeTaskAssessment Create(HomeTaskAssessment entity)
        {
            var context = new Context();
            context.HomeTaskAssessment.Add(entity);
            context.SaveChanges();
            return entity;
        }

        public List<HomeTaskAssessment> GetAll()
        {
           using var context = new Context();
            var hometasksassessment = context.HomeTaskAssessment.ToList();
            context.SaveChanges();
            return hometasksassessment;
            
        }

        public HomeTaskAssessment GetById(int id)
        {
            using var context = new Context();
            return context.HomeTaskAssessment.Find(id);
        }

        public void Remove(int id)
        {
            using var context = new Context();
            var course = context.HomeTaskAssessment.Find(id);
            context.Remove(course);
            context.SaveChanges();
        }

        public void Update(HomeTaskAssessment entity)
        {
            using var context = new Context();
            context.HomeTaskAssessment.Update(entity);
            context.SaveChanges();
        }
    }
}
