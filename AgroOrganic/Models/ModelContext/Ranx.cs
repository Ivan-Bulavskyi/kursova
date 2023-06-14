namespace AgroOrganic.Models.ModelContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ranx
    {
        [Key]
        public int RangeId { get; set; }

        public int MapLayerId { get; set; }

        [Required]
        [StringLength(50)]
        public string RangeName { get; set; }

        public float StartRange { get; set; }

        public int EndRange { get; set; }
    }
}
