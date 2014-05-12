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
                    return GetTownships(connection, searchArea, data.ZoomLevel);
                if (data.ZoomLevel >= 13 && data.ZoomLevel < 15)
                    return GetSections(connection, searchArea, data.ZoomLevel);
                if (data.ZoomLevel >= 15)
                    return GetLSDs(connection, searchArea, data.ZoomLevel);
                return new List<dynamic>();

            }
        }
        private IEnumerable<dynamic> GetTownships(DbConnection connection, SqlGeographyBuilder searchArea, int zoomLevel)
        {
            return FindContainedTownships(connection, searchArea).Select(x => new { Name = x.Name, Coordinates = GetCoordinates(x.Coordinates, zoomLevel), CenterCoordinates = GetLabelCoordinates(x.Coordinates) });
        }

        private static IEnumerable<Township> FindContainedTownships(DbConnection connection, SqlGeographyBuilder searchArea)
        {
            var boundingBoxParameters = new BoundingBoxParam();
            boundingBoxParameters.AddGeography("@geometry", searchArea.ConstructedGeography);
            var townships = connection.Query<Township>(@"select *, geom as Coordinates from townships t where @geometry.STIntersects(t.geom) > 0", boundingBoxParameters);
            return townships;
        }
        private IEnumerable<dynamic> GetSections(DbConnection connection, SqlGeographyBuilder searchArea, int zoomLevel)
        {
            List<Section> results = FindContainedSections(connection, searchArea);
            return results.Select(x => new { Name = x.Name, Coordinates = GetCoordinates(x.Coordinates, zoomLevel), CenterCoordinates = GetLabelCoordinates(x.Coordinates) });
        }
        private static List<Section> FindContainedSections(DbConnection connection, SqlGeographyBuilder searchArea)
        {
            IEnumerable<Township> townships = FindContainedTownships(connection, searchArea);

            List<Section> results = new List<Section>();
            foreach (var township in townships)
            {
                var sectionsParameters = new BoundingBoxParam();
                sectionsParameters.AddGeography("@geometry", searchArea.ConstructedGeography);
                sectionsParameters.AddString("@township", township.TWP);
                sectionsParameters.AddString("@range", township.RGE);
                sectionsParameters.AddString("@meridian", township.MER);
                var sections = connection.Query<Section>(@"select *,
                                                                             geom as Coordinates from sections s
                                                                       where ptwp=@township
                                                                         and prge = @range
                                                                         and pmer = @meridian
                                                                         and @geometry.STIntersects(s.geom) > 0",
                                                                    sectionsParameters);

                results.AddRange(sections);
            }
            return results;
        }
        private IEnumerable<dynamic> GetLSDs(DbConnection connection, SqlGeographyBuilder searchArea, int zoomLevel)
        {
            List<LSD> results = new List<LSD>();
            foreach (var section in FindContainedSections(connection, searchArea))
            {
                var lsdsParameters = new BoundingBoxParam();
                lsdsParameters.AddGeography("@geometry", searchArea.ConstructedGeography);
                lsdsParameters.AddString("@township", section.PTWP);
                lsdsParameters.AddString("@range", section.PRGE);
                lsdsParameters.AddString("@meridian", section.PMER);
                lsdsParameters.AddString("@section", section.SECT);
                var lsds = connection.Query<LSD>(@"select *,
                                                          LSD as LSDName,
                                                          geom as Coordinates from lsds l
                                                  where ptwp=@township
                                                    and prge = @range
                                                    and pmer = @meridian
                                                    and psect = @section
                                                    and @geometry.STIntersects(l.geom) > 0",
                                                lsdsParameters);

                results.AddRange(lsds);
            }
            return results.Select(x => new { Name = x.Name, Coordinates = GetCoordinates(x.Coordinates, zoomLevel), CenterCoordinates = GetLabelCoordinates(x.Coordinates) });
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

        private IEnumerable<Coordinate> GetCoordinates(SqlGeography geography, int zoomLevel)
        {
            var results = new List<Coordinate>();
            for (int i = 1; i <= geography.STNumPoints(); i++)
            {
                results.Add(new Coordinate
                {
                    Latitude = Math.Round(geography.STPointN(i).Lat.Value, GetDecimalsFromZoomLevel(zoomLevel)),
                    Longitude = Math.Round(geography.STPointN(i).Long.Value, GetDecimalsFromZoomLevel(zoomLevel))
                });
            }
            return results;
        }

        private int GetDecimalsFromZoomLevel(int zoomLevel)
        {
            if (zoomLevel < 13)
                return 2;
            if (zoomLevel < 15)
                return 3;
            return 4;
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
