using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.Models;
namespace EntityFrameWork
{
    public class HomeTaskRepository : IRepository<HomeTask>
    {
        public HomeTaskRepository(string connectionString)
        {
        }

        public HomeTask Create(HomeTask entity)
        {
            var context = new Context();
            context.HomeTasks.Add(entity);
            context.SaveChanges();
            return entity;
        }

        public List<HomeTask> GetAll()
        {
            using var context = new Context();
            var homeTasks = context.HomeTasks.ToList();
            context.SaveChanges();
            return homeTasks;
        }

        public HomeTask GetById(int id)
        {
            using var context = new Context();
            return context.HomeTasks.Find(id);
        }

        public void Remove(int id)
        {
            using var context = new Context();
            var course = context.HomeTasks.Find(id);
            context.Remove(course);
            context.SaveChanges();
        }

        public void Update(HomeTask entity)
        {
            using var context = new Context();
            context.HomeTasks.Update(entity);
            context.SaveChanges();
        }
    }
}
