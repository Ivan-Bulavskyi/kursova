using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models.Recommendation {
  public class PerennialGrassesNPKCalculator : NPKCalculator {

    public override void SetFertilizers(List<FertilizerData> fertilizers) {
      base.SetFertilizers(fertilizers);
      //base.fertilizers.Add(new FertilizerData() { Name = "Солома", Value = 4.773f });
    }

    public override double GetСropСapacity() => 1.3;
    public override KeyValuePair<float, NPK> GetNutrients() => new KeyValuePair<float, NPK>(10, new NPK(2.15, 0.6, 0.9));
    public override NPK GetTakeout() => new NPK(0.012, 0.0016, 0.0048);
  }
}