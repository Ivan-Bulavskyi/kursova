using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace AgroOrganic.Models.MapModels
{
    public class ValueLayer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int IdMapLayer { get; set; }
        public int IdPolygon { get; set; }
        public decimal Value { get; set; }
    }
}