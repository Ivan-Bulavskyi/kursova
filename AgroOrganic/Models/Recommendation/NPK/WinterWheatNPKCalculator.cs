using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models.Recommendation {
  public class WinterWheatNPKCalculator : NPKCalculator {

    public override void SetFertilizers(List<FertilizerData> fertilizers) {
      base.SetFertilizers(fertilizers);
      //base.fertilizers.Add(new FertilizerData() { Name = "Солома", Value = 2.726f });
      base.fertilizers.Add(new FertilizerData() { Name = "Зелене добриво (сидерати-люпин)", Value = 30 });
    }

    public override double GetСropСapacity() => 4.3;
    public override KeyValuePair<float, NPK> GetNutrients() => new KeyValuePair<float, NPK>(220, new NPK(1.86, 0.7, 0.44));
    public override NPK GetTakeout() => new NPK(0.0207, 0.0074, 0.0049);

  }
}