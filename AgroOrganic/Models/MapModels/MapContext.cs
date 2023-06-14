using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using AgroOrganic.Models.MapModels;

namespace AgroOrganic.Models.MapModels
{
    public class MapContext : DbContext
    {
        public MapContext() : base("DefaultConnection")
        {
            //Database.SetInitializer<MapContext>(new MapDBInitializer());
        }
        public DbSet<MapLayer> MapLayers { get; set; }
        public DbSet<PolygonLayer> PolygonLayers { get; set; }
        public DbSet<ValueLayer> ValueLayers { get; set; }
    }
}