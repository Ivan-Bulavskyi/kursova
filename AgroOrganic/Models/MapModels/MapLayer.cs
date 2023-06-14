using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AgroOrganic.Models.MapModels
{
    public class MapLayer
    {
        public int Id { get; set; }

        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Display(Name = "В чому вимірюється")]
        public string Measure { get; set; }
        [Display(Name = "Мін. значення")]
        public int MinValue { get; set; }
        [Display(Name = "Макс. значення")]
        public int MaxValue { get; set; }
    }
}