using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data.SqlClient;
using System.Web.Helpers;

namespace AgroOrganic.Models.MapModels
{
    public class Polygons
    {
        public static GeoFeature selectGeoJSON(IGrouping<int,PolygonCombined> groupZip)
        {
            List<GeoProperties> properties = new List<GeoProperties>();
            foreach (PolygonCombined zip in groupZip)
            {
                GeoProperties gp = new GeoProperties()
                {
                    idM = zip.idMapLayer,
                    value = zip.value,
                    name = zip.name,
                    measure = zip.measure,
                    minValue = zip.minValue,
                    maxValue = zip.maxValue
                };
                properties.Add(gp);
               
            }
            var json = groupZip.Select(i => i).First().geoJSON;
            var jsonObj = Json.Decode(json);
            var newGeo = new GeoFeature()
            {
                type = "Feature",
                id = groupZip.Select(i => i.id).First(),
                creator = groupZip.Select(i => i.creator).First(),
                date = groupZip.Select(i => i.date).First(),
                properties = properties,
                geometry = new GeoGeometry()
                {
                    type = jsonObj.geometry.type,
                    coordinates = jsonObj.geometry.coordinates
                }
            };
            return newGeo;
        }
        public static List<PolygonCombined> GetPolygonsInRectangle(
           decimal NELat,
           decimal NELng,
           decimal SWLat,
           decimal SWLng
       )
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            string dbConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            var result = new List<PolygonCombined>();
            using (var con = new SqlConnection(dbConnectString)) {
              try {
                con.Open();
                var quarryText = @"DECLARE @area geography = GEOGRAPHY :: STGeomFromText('polygon(({3} {0}, {3} {2}, {1} {2}, {1} {0}, {3} {0}))', 4326) SELECT gj.id, gj.latitude, gj.longitude, gj.geoJSON, gj.creator, gj.date, v.Value, ml.Id as idMapLayer,ml.Name, ml.Measure, ml.MinValue, ml.MaxValue  FROM PolygonLayers gj LEFT JOIN ValueLayers v on v.IdPolygon=gj.id LEFT JOIN MapLayers ml on ml.Id=v.IdMapLayer  WHERE @area.STIntersects(point) = 1";
                quarryText = string.Format(quarryText, NELat, NELng, SWLat, SWLng);

                var quarry = con.Query<PolygonCombined>(quarryText,
                     new { NELat = NELat, NELng = NELng, SWLat = SWLat, SWLng = SWLng }
                );

                result.AddRange(quarry);
              }
              catch (Exception e) {
                return result;
              }
            }
            return result;
        }
        public class GeoJSON
        {
            public string type { get; set; }
            public List<GeoFeature> features { get; set; }
        }
        public class PolygonCombined
        {
            public int id { get; set; }
            public decimal latitude { get; set; }
            public decimal longitude { get; set; }
            public string geoJSON { get; set; }
            public string creator { get; set; }
            public DateTime date { get; set; }
            public decimal value { get; set; }
            public int idMapLayer { get; set; }
            public string name { get; set; }
            public string measure { get; set; }
            public int minValue { get; set; }
            public int maxValue { get; set; }
        }
        public class GeoFeature
        {
            public string type { get; set; }
            public int id { get; set; }
            public string creator { get; set; }
            public DateTime date { get; set; }
            public List<GeoProperties> properties { get; set; }
            public GeoGeometry geometry { get; set; }
        }
        public class GeoProperties
        {
            public int idM { get; set; }
            public decimal value { get; set; }
            public string name { get; set; }
            public string measure { get; set; }
            public int minValue { get; set; }
            public int maxValue { get; set; }
        }
        public class GeoGeometry
        {
            public string type { get; set; }
            public dynamic coordinates { get; set; }
        }
    }
}