using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models.Recommendation {
  public class BuckwheatNPKCalculator : NPKCalculator {

    public override void SetFertilizers(List<FertilizerData> fertilizers) {
      base.SetFertilizers(fertilizers);
      //base.fertilizers.Add(new FertilizerData() { Name = "Солома", Value = 0.97f });
    }

    public override double GetСropСapacity() => 1.0;
    public override KeyValuePair<float, NPK> GetNutrients() => new KeyValuePair<float, NPK>(80, new NPK(1.81, 0.88, 0.6));
    public override NPK GetTakeout() => new NPK(0.0177, 0.0059, 0.0042);

  }
}