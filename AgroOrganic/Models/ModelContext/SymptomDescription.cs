namespace AgroOrganic.Models.ModelContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SymptomDescription")]
    public partial class SymptomDescription
    {
        [Key]
        [Column("SymptomDescription")]
        public int SymptomDescription1 { get; set; }

        public int MapLayerId { get; set; }

        public bool IsForDeficit { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }
    }
}
