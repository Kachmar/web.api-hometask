namespace Models.Models
{
    using System;

    public class HomeTaskAssessment
    {
        public int Id { get; set; }

        public bool IsComplete { get; set; }

        public DateTime Date { get; set; }

        public int HomeTaskId { get; set; }

        public virtual HomeTask HomeTask { get; set; }

        public int StudentId { get; set; }

        public virtual Student Student { get; set; }
    }
}