using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models.Recommendation {
  public class OatNPKCalculator : NPKCalculator {

    public override void SetFertilizers(List<FertilizerData> fertilizers) {
      base.SetFertilizers(fertilizers);
      //base.fertilizers.Add(new FertilizerData() { Name = "Солома", Value = 15.26f });
    }

    public override double GetСropСapacity() => 2.4;
    public override KeyValuePair<float, NPK> GetNutrients() => new KeyValuePair<float, NPK>(180, new NPK(1.74, 0.67, 0.51));
    public override NPK GetTakeout() => new NPK(0.0189, 0.0083, 0.0051);

  }
}