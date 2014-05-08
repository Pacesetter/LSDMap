using System;
using Dapper;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Map.Controllers
{
    public class SpatialParam : SqlMapper.IDynamicParameters
    {
        string name;
        object val;

        public SpatialParam(string name, object val)
        {
            this.name = name;
            this.val = val;
        }

        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            var sqlCommand = (SqlCommand)command;
            sqlCommand.Parameters.Add(new SqlParameter
            {
                UdtTypeName = "geography",
                Value = val,
                ParameterName = name
            });
        }
    }
}
