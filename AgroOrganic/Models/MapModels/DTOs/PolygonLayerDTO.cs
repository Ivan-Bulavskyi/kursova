using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models.MapModels.DTOs
{
    public class PolygonLayerDTO
    {
        public int id { get; set; }
        public string geoJSON { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }
        public string point { get; set; }
        public DateTime? date { get; set; }
        public List<ValueLayer> values;
    }
}