using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models.Recommendation
{
    public class Recomendation
    {
        public string County { get; set; }
        public string Culture { get; set; }
        public string Product { get; set; }
        public int Year { get; set; }
        public double HumusContent { get; set; }
        public double HumusBalance { get; set; }
        public double ExpectedLevelOfHumus { get; set; }
        public double AddedFertilizer { get; set; }
        public double AdditionalExepencesOnHumus { get; set; }
        public double AdditionalExepencesOnCertification { get; set; }
        public double SummaryProductCost { get; set; }
        public double ProductCost { get; set; }
        public double Productivity { get; set; }
        public double Income { get; set; }        
    }
}