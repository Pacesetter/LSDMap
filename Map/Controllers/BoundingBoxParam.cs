using System;
using Dapper;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.SqlServer.Types;

namespace Map.Controllers
{

    public class BoundingBoxParam : SqlMapper.IDynamicParameters
    {

        private Dictionary<string, SqlGeography> _geographyTypes;
        private Dictionary<String, string> _stringTypes;

        public BoundingBoxParam()
        {
            _geographyTypes = new Dictionary<string, SqlGeography>();
            _stringTypes = new Dictionary<string, string>();
        }

        public void AddGeography(string name, SqlGeography value)
        {
            _geographyTypes.Add(name, value);
        }

        public void AddString(string name, string value)
        {
            _stringTypes.Add(name, value);
        }

        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            var sqlCommand = (SqlCommand)command;

            foreach (var geography in _geographyTypes)
            {
                sqlCommand.Parameters.Add(new SqlParameter
                {
                    UdtTypeName = "geography",
                    Value = geography.Value,
                    ParameterName = geography.Key
                });
            }

            foreach (var stringValue in _stringTypes)
            {
                sqlCommand.Parameters.Add(new SqlParameter
                {
                    Value = stringValue.Value,
                    ParameterName = stringValue.Key
                });
            }
        }
    }
}
