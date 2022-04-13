using System;
using System.Data.SqlClient;

namespace ADO.NET
{
    public class RepositoryBase
    {
        private string _connectionString;

        public RepositoryBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected SqlConnection GetConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}