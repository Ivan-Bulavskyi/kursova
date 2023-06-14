using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace AgroOrganic.Models.MapModels
{
    public class PolygonLayer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public string geoJSON { get; set; }

        public decimal latitude { get; set; }

        public decimal longitude { get; set; }

        public DbGeography point { get; set; }
        public string creator { get; set; }
        public DateTime date { get; set; }
    }
}