using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models
{
    public class SymptomDescriptionModel
    {
        public string ElementName { get; set; }
        public bool IsForDeficit { get; set; }
        public string Description { get; set; }
    }
}