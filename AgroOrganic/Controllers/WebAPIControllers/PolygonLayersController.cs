using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using AgroOrganic.Models;
using AgroOrganic.Models.MapModels;
using AgroOrganic.Models.MapModels.DTOs;
using System.Web;

namespace AgroOrganic.Controllers.MapControllers
{
    public class PolygonLayersController : ApiController
    {
        MapContext db = new MapContext();
        [HttpGet]
        public async Task<Polygons.GeoJSON> GetPolygonsAsync(
           decimal NELat,
           decimal NELng,
           decimal SWLat,
           decimal SWLng
       )
        {
            return await Task.Run(() =>
            {

                var geoJSONAsync = Polygons.GetPolygonsInRectangle(NELat, NELng, SWLat, SWLng);

                Polygons.GeoJSON result = new Polygons.GeoJSON()
                {
                    type = "FeatureCollection",
                    features = geoJSONAsync
                    .GroupBy(g=>g.id)
                   .Select(x => Polygons.selectGeoJSON(x))
                   .ToList()
                };
                
                return result;
            }
            );
        }
        [Authorize]
        [HttpPut]
        public void EditPoly([FromBody] PolygonLayerDTO dto)
        {
            PolygonLayer poly = db.PolygonLayers.Find(dto.id);
            poly.date = dto.date ?? DateTime.Now;
            poly.creator = User.Identity.Name;
            db.Entry(poly).State = EntityState.Modified;
            for (int i = 0; i < dto.values.Count; i++)
            {
                int idMapLayer = dto.values[i].IdMapLayer;
                ValueLayer v = db.ValueLayers.FirstOrDefault(val => val.IdPolygon == poly.id && val.IdMapLayer == idMapLayer);
                if (v != null)
                {
                    v.Value = dto.values[i].Value;
                    db.Entry(v).State = EntityState.Modified;
                }
                else
                {
                    dto.values[i].IdPolygon = poly.id;
                    db.ValueLayers.Add(dto.values[i]);
                }
            }
            db.SaveChanges();
        }
        [Authorize]
        [HttpDelete]
        public void DeletePoly(int id)
        {
            PolygonLayer poly= db.PolygonLayers.Find(id);
            if (poly!= null)
            {
                db.PolygonLayers.Remove(poly);
                db.SaveChanges();
            }
        }
        [Authorize]
        [HttpPost]
        public void PostPoly([FromBody] PolygonLayerDTO dto)
        {
            PolygonLayer poly = new PolygonLayer()
            {
                geoJSON = dto.geoJSON,
                latitude = dto.latitude,
                longitude = dto.longitude,
                point = System.Data.Entity.Spatial.DbGeography.FromText(dto.point),
                creator = User.Identity.Name,
                date = dto.date ?? DateTime.Now
            };
            db.PolygonLayers.Add(poly);
            db.SaveChanges();
            dto.values.ToList().ForEach(v => v.IdPolygon = poly.id);
            db.ValueLayers.AddRange(dto.values);
            db.SaveChanges();
        }
    }
}
