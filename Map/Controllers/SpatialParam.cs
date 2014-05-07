using Map.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.Mvc;
using Microsoft.SqlServer.Types;
using System.Data;

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
