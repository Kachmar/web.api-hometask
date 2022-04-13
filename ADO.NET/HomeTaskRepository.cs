using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Models;
using Models.Models;

namespace ADO.NET
{
    public class HomeTaskRepository : RepositoryBase, IRepository<HomeTask>
    {
        public HomeTaskRepository(string connectionString) : base(connectionString)
        {
        }

        public List<HomeTask> GetAll()
        {
            List<HomeTask> result = new List<HomeTask>();
            using (SqlConnection connection = GetConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(
                    @"
                   SELECT [Id]
                  ,[Date]
                  ,[Title]
                  ,[Description]
                  ,[Number]
                  ,[CourseId]
              FROM [dbo].[HomeTasks]", connection);

                using var reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    HomeTask homeTask = new HomeTask();
                    homeTask.Id = reader.GetInt32(0);
                    homeTask.Date = reader.GetDateTime(1);
                    homeTask.Title = reader.GetStringOrDefault(2);
                    homeTask.Description = reader.GetStringOrDefault(3);
                    homeTask.Number = reader.GetInt32(4);
                    homeTask.CourseId = reader.GetInt32(5);
                    homeTask.HomeTaskAssessments = GetHomeTaskAssessments(homeTask.Id);
                    result.Add(homeTask);
                }
            }

            return result;
        }

        private List<HomeTaskAssessment> GetHomeTaskAssessments(int homeTaskId)
        {
            List<HomeTaskAssessment> result = new List<HomeTaskAssessment>();
            using (SqlConnection connection = GetConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(
                    $@"
                   SELECT [Id]                 
                  ,[IsComplete]
                  ,[Date]                              
              FROM [dbo].[HomeTaskAssessment]             
              where HomeTaskId =  {homeTaskId}", connection);

                using var reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    HomeTaskAssessment homeTaskAssessment = new HomeTaskAssessment();
                    homeTaskAssessment.Id = reader.GetInt32(0);
                    homeTaskAssessment.IsComplete = reader.GetBoolean(1);
                    homeTaskAssessment.Date = reader.GetDateTime(2);
                    result.Add(homeTaskAssessment);
                }
            }

            return result;
        }

        public HomeTask GetById(int id)
        {
            return this.GetAll().SingleOrDefault(homeTask => homeTask.Id == id);
        }

        public HomeTask Create(HomeTask homeTask)
        {
            using (SqlConnection connection = GetConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(@"
INSERT INTO [dbo].[HomeTasks]
           ([Date]
           ,[Title]
           ,[Description]
           ,[Number]
            ,[CourseId])
     VALUES
           (@Date
           ,@Title
           ,@Description
           ,@Number
            ,@CourseId);
SELECT CAST(scope_identity() AS int)
",
                    connection);
                sqlCommand.Parameters.AddWithNullableValue("@Date", homeTask.Date);
                sqlCommand.Parameters.AddWithNullableValue("@Title", homeTask.Title);
                sqlCommand.Parameters.AddWithNullableValue("@Description", homeTask.Description);
                sqlCommand.Parameters.AddWithNullableValue("@Number", homeTask.Number);
                sqlCommand.Parameters.AddWithNullableValue("@CourseId", homeTask.CourseId);

                int identity = (int)sqlCommand.ExecuteScalar();
                if (identity == 0)
                {
                    return null;
                }

                homeTask.Id = identity;
            }

            return homeTask;
        }

        public void Update(HomeTask homeTask)
        {
            using SqlConnection connection = GetConnection();
            using SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                using SqlCommand sqlCommand = new SqlCommand(@"
                UPDATE [dbo].[HomeTasks]
                   SET [Date] = @Date
                      ,[Title] = @Title
                      ,[Description] = @Description
                      ,[Number] = @Number      
                 WHERE Id = @Id
                ",
                    connection,
                    transaction);
                sqlCommand.Parameters.AddWithNullableValue("@Date", homeTask.Date);
                sqlCommand.Parameters.AddWithNullableValue("@Id", homeTask.Id);
                sqlCommand.Parameters.AddWithNullableValue("@Description", homeTask.Description);
                sqlCommand.Parameters.AddWithNullableValue("@Title", homeTask.Title);
                sqlCommand.Parameters.AddWithNullableValue("@Number", homeTask.Number);
                sqlCommand.ExecuteNonQuery();

                SetAssessmentToHomeTask(homeTask.HomeTaskAssessments, homeTask, transaction);
                transaction.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        private void SetAssessmentToHomeTask(List<HomeTaskAssessment> homeTaskAssessments, in HomeTask homeTask, SqlTransaction transaction)
        {
            SqlCommand sqlCommand = new SqlCommand($@"DELETE FROM [dbo].[HomeTaskAssessment]
            WHERE HomeTaskId = {homeTask.Id}", transaction.Connection, transaction);
            sqlCommand.ExecuteNonQuery();
            foreach (var homeTaskAssessment in homeTaskAssessments)
            {
                sqlCommand = new SqlCommand(@"
INSERT INTO [dbo].[HomeTaskAssessment]
           ([Date]
           ,[IsComplete]
           ,[HomeTaskId]
           ,[StudentId])
     VALUES
           (@Date
           ,@IsComplete
           ,@HomeTaskId
           ,@StudentId);
SELECT CAST(scope_identity() AS int)
", transaction.Connection, transaction);
                sqlCommand.Parameters.AddWithNullableValue("@Date", homeTaskAssessment.Date);
                sqlCommand.Parameters.AddWithNullableValue("@IsComplete", homeTaskAssessment.IsComplete);
                sqlCommand.Parameters.AddWithNullableValue("@HomeTaskId", homeTask.Id);
                sqlCommand.Parameters.AddWithNullableValue("@StudentId", homeTaskAssessment.StudentId);

                int identity = (int)sqlCommand.ExecuteScalar();
                if (identity == 0)
                {
                    throw new Exception($"Failed to insert {nameof(homeTaskAssessment)} for hometaskId: {homeTask.Id}");
                }

                homeTaskAssessment.Id = identity;
            }
        }

        public void Remove(int id)
        {
            using SqlConnection connection = GetConnection();
            SqlCommand sqlCommand = new SqlCommand(
                $@"DELETE FROM [dbo].[HomeTasks]
                WHERE Id={id}", connection);
            sqlCommand.ExecuteNonQuery();
        }
    }
}
