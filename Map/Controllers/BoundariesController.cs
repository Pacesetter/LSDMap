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

namespace Map.Controllers
{
    public class BoundariesController : ApiController
    {
        public IEnumerable<Box> Get([FromUri] BoundaryRequest data)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var results = connection.Query(@"declare @g geography = geography::STGeomFromText('POLYGON((-110 51, -110 49, -108 49, -108 51, -108 51, -110 51))', 4326);
                                   select trm, geom from lsds t where @g.STIntersects(t.geom) > 0");
                return null;
            }
        }
        private DbConnection GetConnection()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }
    }
    public class BoundaryRequest
    {
        public int ZoomLevel { get; set; }
        public Coordinate NorthWest { get; set; }
        public Coordinate NorthEast { get; set; }
        public Coordinate SouthWest { get; set; }
        public Coordinate SouthEast { get; set; }
    }


}
