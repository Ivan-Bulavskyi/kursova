using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AgroOrganic.Models;
using AgroOrganic.Models.MapModels;
using AgroOrganic.Models.MapModels.DTOs;

namespace AgroOrganic.Controllers.MapControllers
{
    public class MapLayersController : ApiController
    {
        MapContext db = new MapContext();
        [HttpGet]
        public IEnumerable<MapLayer> GetValues()
        {
            return db.MapLayers;
        }

    }
}
