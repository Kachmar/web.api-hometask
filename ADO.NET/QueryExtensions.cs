using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET
{
    public static class QueryExtensions
    {
        public static string GetStringOrDefault(this SqlDataReader reader, int index)
        {
            return reader.IsDBNull(index) ? null : reader.GetString(index);
        }

        public static SqlParameter AddWithNullableValue(this SqlParameterCollection target, string parameterName, object value)
        {
            if (value == null)
            {
                return target.AddWithValue(parameterName, DBNull.Value);
            }

            return target.AddWithValue(parameterName, value);
        }
    }
}
