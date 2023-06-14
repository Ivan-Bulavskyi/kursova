using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models.Recommendation {
  public class MaizeNPKCalculator : NPKCalculator {

    public override void SetFertilizers(List<FertilizerData> fertilizers) {
      base.SetFertilizers(fertilizers);
      //base.fertilizers.Add(new FertilizerData() { Name = "Солома", Value = 1.45f });
    }

    public override double GetСropСapacity() => 5.0;
    public override KeyValuePair<float, NPK> GetNutrients() => new KeyValuePair<float, NPK>(25, new NPK(1.37, 0.53, 0.38));
    public override NPK GetTakeout() => new NPK(0.0153, 0.0059, 0.0042);
  }
}