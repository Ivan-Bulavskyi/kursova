using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models.Recommendation {
  public class SoyNPKCalculator : NPKCalculator {
    public override void SetFertilizers(List<FertilizerData> fertilizers) {
      base.SetFertilizers(fertilizers);
      //base.fertilizers.Add(new FertilizerData() { Name = "Солома", Value = 4.773f });
    }

    public override double GetСropСapacity() => 1.3;
    public override KeyValuePair<float, NPK> GetNutrients() => new KeyValuePair<float, NPK>(55, new NPK(5.37, 1.18, 2.07));
    public override NPK GetTakeout() => new NPK(0.065, 0.0104, 0.0084);
  }
}