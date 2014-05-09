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
                SqlGeographyBuilder searchArea = GetSearchArea(data);

                if (data.ZoomLevel >= 10 && data.ZoomLevel < 13)
                    return GetTownships(connection, searchArea);
                if (data.ZoomLevel >= 13)
                    return GetSections(connection, searchArea);
                return new List<dynamic>();

            }
        }
        private IEnumerable<dynamic> GetTownships(DbConnection connection, SqlGeographyBuilder searchArea)
        {
            var boundingBoxParameters = new BoundingBoxParam();
            boundingBoxParameters.AddGeography("@geometry", searchArea.ConstructedGeography);
            var results = connection.Query<Box>(@"select id, trm as Name, geom as Coordinates from townships t where @geometry.STIntersects(t.geom) > 0",
                                                                boundingBoxParameters);
            return results.Select(x => new { Name = x.Name, Coordinates = GetCoordinates(x.Coordinates), CenterCoordinates = GetLabelCoordinates(x.Coordinates) });
        }

        private IEnumerable<dynamic> GetSections(DbConnection connection, SqlGeographyBuilder searchArea)
        {
            var boundingBoxParameters = new BoundingBoxParam();
            boundingBoxParameters.AddGeography("@geometry", searchArea.ConstructedGeography);
            var townships = connection.Query<Township>(@"select *, geom as Coordinates from townships t where @geometry.STIntersects(t.geom) > 0", boundingBoxParameters);

            List<dynamic> results = new List<dynamic>();
            foreach (var township in townships)
            {
                var sectionsParameters = new BoundingBoxParam();
                sectionsParameters.AddGeography("@geometry", searchArea.ConstructedGeography);
                sectionsParameters.AddString("@township", township.TWP);
                sectionsParameters.AddString("@range", township.RGE);
                sectionsParameters.AddString("@meridian", township.MER);
                var sections = connection.Query<Box>(@"select id, 
                                                             ptwp +'-'+ prge +'-' + pmer + '-' + SECT as Name, 
                                                             geom as Coordinates from sections s 
                                                       where ptwp=@township 
                                                         and prge = @range 
                                                         and pmer = @meridian 
                                                         and @geometry.STIntersects(s.geom) > 0",
                                                                    sectionsParameters);
                results.AddRange(sections.Select(x => new { Name = x.Name, Coordinates = GetCoordinates(x.Coordinates), CenterCoordinates = GetLabelCoordinates(x.Coordinates) }));
            }
            return results;
        }
        private static SqlGeographyBuilder GetSearchArea(BoundaryRequest data)
        {
            var searchArea = new SqlGeographyBuilder();
            searchArea.SetSrid(4326);
            searchArea.BeginGeography(OpenGisGeographyType.Polygon);
            searchArea.BeginFigure(data.NorthEast.Latitude, data.NorthEast.Longitude);
            searchArea.AddLine(data.NorthWest.Latitude, data.NorthWest.Longitude);
            searchArea.AddLine(data.SouthWest.Latitude, data.SouthWest.Longitude);
            searchArea.AddLine(data.SouthEast.Latitude, data.SouthEast.Longitude);
            searchArea.AddLine(data.NorthEast.Latitude, data.NorthEast.Longitude);
            searchArea.EndFigure();
            searchArea.EndGeography();
            return searchArea;
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

        private Coordinate GetLabelCoordinates(SqlGeography geography)
        {
            double maxLatitude = double.MinValue;
            double maxLongitude = double.MinValue;
            for (int i = 1; i <= geography.STNumPoints(); i++)
            {
                if (geography.STPointN(i).Lat.Value > maxLatitude)
                    maxLatitude = geography.STPointN(i).Lat.Value;
                if (geography.STPointN(i).Long.Value > maxLongitude)
                    maxLongitude = geography.STPointN(i).Long.Value;
            }
            return new Coordinate { Latitude = maxLatitude, Longitude = maxLongitude };
        }
    }

}
