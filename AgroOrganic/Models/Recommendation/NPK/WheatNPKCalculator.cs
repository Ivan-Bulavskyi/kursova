using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models.Recommendation {
  public class WheatNPKCalculator : NPKCalculator {

    public override void SetFertilizers(List<FertilizerData> fertilizers) {
      base.SetFertilizers(fertilizers);
      //base.fertilizers.Add(new FertilizerData() { Name = "Солома", Value = 4.773f });
      base.fertilizers.Add(new FertilizerData() { Name = "Зелене добриво (сидерати-люпин)", Value = 30 });
    }

    public override double GetСropСapacity() => 2.7;
    public override KeyValuePair<float, NPK> GetNutrients() => new KeyValuePair<float, NPK>(200, new NPK(1.65, 0.66, 0.52));
    public override NPK GetTakeout() => new NPK(0.0174, 0.0075, 0.0054);
  }
}