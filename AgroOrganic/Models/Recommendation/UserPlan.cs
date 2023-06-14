using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models.Recommendation
{
    public class UserPlan
    {
        public string Culture { get; set; }
        public double AddedFertilizer { get; set; }
        public string FertilizerType { get; set; }
        public double Productivity { get; set; }
        public double ProductivityAfter { get; set; }
    }
}