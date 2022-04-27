using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Models;
namespace EntityFrameWork
{
    public class Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine);
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-E3LF3J7\SQLEXPRESS;Initial Catalog=University;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<Course> Courses { get; set; } 
        public DbSet<HomeTask> HomeTasks { get; set; }
        public DbSet<HomeTaskAssessment> HomeTaskAssessment { get; set; }
        public DbSet<Student> Students { get; set; }
    }
}
