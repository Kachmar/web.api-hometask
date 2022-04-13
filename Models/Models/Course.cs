namespace Models.Models
{
    using System;
    using System.Collections.Generic;

    public class Course
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int PassCredits { get; set; }

        public virtual List<HomeTask> HomeTasks { get; set; } = new List<HomeTask>();

        public virtual List<Student> Students { get; set; } = new List<Student>();
    }
}