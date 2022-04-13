using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Models;
using Models.Models;

namespace ADO.NET
{
    public class CourseRepository : RepositoryBase, IRepository<Course>
    {
        public CourseRepository(string connectionString) : base(connectionString)
        {
        }

        internal List<Course> GetAll(bool loadChildEntities)
        {
            List<Course> result = new List<Course>();
            using (SqlConnection connection = GetConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(
                    $@"
                   SELECT [Id]
                  ,[Name]
                  ,[StartDate]
                  ,[EndDate]
                  ,[PassCredits]
              FROM [dbo].[Courses]", connection);

                using var reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    Course course = new Course();
                    course.Id = reader.GetInt32(0);
                    course.Name = reader.GetStringOrDefault(1);
                    course.StartDate = reader.GetDateTime(2);
                    course.EndDate = reader.GetDateTime(3);
                    course.PassCredits = reader.GetInt32(4);
                    if (loadChildEntities)
                    {
                        course.HomeTasks = GetHomeTasks(course.Id);
                        course.Students = GetCourseStudents(course.Id);
                    }

                    result.Add(course);
                }
            }

            return result;
        }

        private List<HomeTask> GetHomeTasks(in int courseId)
        {
            List<HomeTask> result = new List<HomeTask>();
            using (SqlConnection connection = GetConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(
                    $@"select Id, Date, Title, Description, Number from HomeTasks                    
              where CourseId =  {courseId}", connection);

                using var reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    HomeTask homeTask = new HomeTask();
                    homeTask.Id = reader.GetInt32(0);
                    homeTask.Date = reader.GetDateTime(1);
                    homeTask.Title = reader.GetStringOrDefault(2);
                    homeTask.Description = reader.GetStringOrDefault(3);
                    homeTask.Number = reader.GetInt32(4);
                    result.Add(homeTask);
                }
            }

            return result;
        }

        private List<Student> GetCourseStudents(in int courseId)
        {
            List<Student> result = new List<Student>();
            using (SqlConnection connection = GetConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(
                    $@"select Id, Name, BirthDate, PhoneNumber, Email, GitHubLink, Notes from Students
                    as s
              join CourseStudent as cs on cs.StudentsId=s.Id
              where cs.CoursesId =  {courseId}", connection);

                using var reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    Student student = new Student();
                    student.Id = reader.GetInt32(0);
                    student.Name = reader.GetStringOrDefault(1);
                    student.BirthDate = reader.GetDateTime(2);
                    student.PhoneNumber = reader.GetStringOrDefault(3);
                    student.Email = reader.GetStringOrDefault(4);
                    student.GitHubLink = reader.GetStringOrDefault(5);
                    student.Notes = reader.GetStringOrDefault(6);
                    result.Add(student);
                }
            }

            return result;
        }


        public List<Course> GetAll()
        {
            return this.GetAll(true);
        }

        public Course GetById(int id)
        {
            return this.GetAll().SingleOrDefault(course => course.Id == id);
        }

        public Course Create(Course course)
        {
            using (SqlConnection connection = GetConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(@"
INSERT INTO [dbo].[Courses]
           ([Name]
           ,[StartDate]
           ,[EndDate]
           ,[PassCredits])
     VALUES
           (@Name
           ,@StartDate
           ,@EndDate
           ,@PassCredits);
SELECT CAST(scope_identity() AS int)
",
                    connection);
                sqlCommand.Parameters.AddWithNullableValue("@Name", course.Name);
                sqlCommand.Parameters.AddWithNullableValue("@StartDate", course.StartDate);
                sqlCommand.Parameters.AddWithNullableValue("@EndDate", course.EndDate);
                sqlCommand.Parameters.AddWithNullableValue("@PassCredits", course.PassCredits);

                int identity = (int)sqlCommand.ExecuteScalar();
                if (identity == 0)
                {
                    return null;
                }

                course.Id = identity;
            }

            return course;
        }

        public void Update(Course course)
        {
            using SqlConnection connection = GetConnection();
            using SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                using SqlCommand sqlCommand = new SqlCommand(@"
                UPDATE [dbo].[Courses]
                   SET [Name] = @Name
                      ,[StartDate] = @StartDate
                      ,[EndDate] = @EndDate
                      ,[PassCredits] = @PassCredits      
                 WHERE Id = @Id
                ",
                 connection,
                 transaction);
                sqlCommand.Parameters.AddWithNullableValue("@Name", course.Name);
                sqlCommand.Parameters.AddWithNullableValue("@Id", course.Id);
                sqlCommand.Parameters.AddWithNullableValue("@StartDate", course.StartDate);
                sqlCommand.Parameters.AddWithNullableValue("@EndDate", course.EndDate);
                sqlCommand.Parameters.AddWithNullableValue("@PassCredits", course.PassCredits);
                sqlCommand.ExecuteNonQuery();

                SetStudentToCourse(course.Students.Select(p => p.Id), course.Id, transaction);
                transaction.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void SetStudentToCourse(IEnumerable<int> studentIds, in int courseId, SqlTransaction transaction)
        {
            SqlCommand sqlCommand = new SqlCommand($@"DELETE FROM [dbo].[CourseStudent]
            WHERE CoursesId = {courseId}", transaction.Connection, transaction);
            sqlCommand.ExecuteNonQuery();
            foreach (var studentId in studentIds)
            {
                sqlCommand = new SqlCommand(
                    $@"INSERT INTO [dbo].[CourseStudent]
           ([CoursesId]
           ,[StudentsId])
            VALUES
           ({courseId},{studentId})",
                    transaction.Connection,
                    transaction);

                sqlCommand.ExecuteNonQuery();
            }
        }

        public void Remove(int id)
        {
            using SqlConnection connection = GetConnection();
            SqlCommand sqlCommand = new SqlCommand(
                $@"DELETE FROM [dbo].[Courses]
                WHERE Id={id}", connection);
            sqlCommand.ExecuteNonQuery();
        }
    }
}
