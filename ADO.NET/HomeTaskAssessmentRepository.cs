using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Models;
using Models.Models;

namespace ADO.NET
{
    public class HomeTaskAssessmentRepository : RepositoryBase, IRepository<HomeTaskAssessment>
    {
        public HomeTaskAssessmentRepository(string connectionString) : base(connectionString)
        {
        }

        public HomeTaskAssessment GetById(int id)
        {
            return this.GetAll().SingleOrDefault(h => h.Id == id);
        }

        public HomeTaskAssessment Create(HomeTaskAssessment entity)
        {
            using (SqlConnection connection = GetConnection())
            {
                var sqlCommand = new SqlCommand(@"
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
", connection);
                sqlCommand.Parameters.AddWithNullableValue("@Date", entity.Date);
                sqlCommand.Parameters.AddWithNullableValue("@IsComplete", entity.IsComplete);
                sqlCommand.Parameters.AddWithNullableValue("@HomeTaskId", entity.HomeTaskId);
                sqlCommand.Parameters.AddWithNullableValue("@StudentId", entity.StudentId);

                int identity = (int)sqlCommand.ExecuteScalar();
                if (identity == 0)
                {
                    return null;
                }

                entity.Id = identity;
            }

            return entity;
        }

        public void Update(HomeTaskAssessment entity)
        {
            using SqlConnection connection = GetConnection();
            try
            {
                using SqlCommand sqlCommand = new SqlCommand(@"
                UPDATE [dbo].[HomeTaskAssessment]
                   SET [Date] = @Date
                      ,[IsComplete] = @IsComplete
                      ,[HomeTaskId] = @HomeTaskId
                      ,[StudentId] = @StudentId      
                 WHERE Id = @Id
                ",
                    connection);
                sqlCommand.Parameters.AddWithNullableValue("@Date", entity.Date);
                sqlCommand.Parameters.AddWithNullableValue("@Id", entity.Id);
                sqlCommand.Parameters.AddWithNullableValue("@IsComplete", entity.IsComplete);
                sqlCommand.Parameters.AddWithNullableValue("@HomeTaskId", entity.HomeTaskId);
                sqlCommand.Parameters.AddWithNullableValue("@StudentId", entity.StudentId);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void Remove(int id)
        {
            using SqlConnection connection = GetConnection();
            SqlCommand sqlCommand = new SqlCommand(
                $@"DELETE FROM [dbo].[HomeTaskAssessment]
                WHERE Id={id}", connection);
            sqlCommand.ExecuteNonQuery();
        }


        public List<HomeTaskAssessment> GetAll()
        {
            List<HomeTaskAssessment> result = new List<HomeTaskAssessment>();
            using (SqlConnection connection = GetConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(
                    @"
                   SELECT [Id]                 
                  ,[IsComplete]
                  ,[Date]
                  ,[HomeTaskId]
                  ,[StudentId] 
              FROM [dbo].[HomeTaskAssessment]", connection);

                using var reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    HomeTaskAssessment homeTaskAssessment = new HomeTaskAssessment();
                    homeTaskAssessment.Id = reader.GetInt32(0);
                    homeTaskAssessment.IsComplete = reader.GetBoolean(1);
                    homeTaskAssessment.Date = reader.GetDateTime(2);
                    homeTaskAssessment.HomeTaskId = reader.GetInt32(3);
                    homeTaskAssessment.StudentId = reader.GetInt32(4);

                    result.Add(homeTaskAssessment);
                }
            }

            return result;
        }
    }
}
