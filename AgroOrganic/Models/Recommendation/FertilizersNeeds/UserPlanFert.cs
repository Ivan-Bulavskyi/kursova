using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models.Recommendation {
  public class UserPlanFert {
    public string Culture { get; set; }
    public double AddedFertilizer { get; set; }
    public double PlannedHarvest { get; set; }
  }
}