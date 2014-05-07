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
    public class BoundariesController : ApiController
    {
        public IEnumerable<dynamic> Get([FromUri] BoundaryRequest data)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var b = new SqlGeographyBuilder();
                b.SetSrid(4326);
                b.BeginGeography(OpenGisGeographyType.Polygon);
                b.BeginFigure(data.NorthEast.Latitude, data.NorthEast.Longitude);
                b.AddLine(data.NorthWest.Latitude, data.NorthWest.Longitude);
                b.AddLine(data.SouthWest.Latitude, data.SouthWest.Longitude);
                b.AddLine(data.SouthEast.Latitude, data.SouthEast.Longitude);
                b.AddLine(data.NorthEast.Latitude, data.NorthEast.Longitude);
                b.EndFigure();
                b.EndGeography();
                //                var results = connection.Query<Box>(@"declare @g geography = geography::STGeomFromText('POLYGON((-110 51, -110 49, -108 49, -108 51, -108 51, -110 51))', 4326);
                //                                   select id, trm as Name, geom as Coordinates from lsds t where @g.STIntersects(t.geom) > 0");

                var results = connection.Query<Box>(@"select id, trm as Name, geom as Coordinates from lsds t where @geometry.STIntersects(t.geom) > 0", new SpatialParam("@geometry", b.ConstructedGeography));

                return results.Select(x => new { Name = x.Name, Coordinates = GetCoordinates(x.Coordinates) });

            }
        }
        private DbConnection GetConnection()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }

        private IEnumerable<Coordinate> GetCoordinates(SqlGeography geography)
        {
            var results = new List<Coordinate>();
            for (int i = 1; i <= geography.STNumPoints(); i++)
            {
                results.Add(new Coordinate { Latitude = geography.STPointN(i).Lat.Value, Longitude = geography.STPointN(i).Long.Value });
            }
            return results;
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
