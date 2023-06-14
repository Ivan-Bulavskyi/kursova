using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models.Recommendation {
  public class SideratesNPKCalculator : NPKCalculator {
    public override void SetFertilizers(List<FertilizerData> fertilizers) {
      base.SetFertilizers(fertilizers);
      base.fertilizers.Add(new FertilizerData() { Name = "Зелене добриво (сидерати-люпин)", Value = 30 });
      base.fertilizers.Add(new FertilizerData() { Name = "Зелене добриво (сидерати-редька олійна)", Value = 24 });
    }

    public override NPK GetHarvestNPK(FertilizerData data) {
      var result = base.GetHarvestNPK(data);
      //if (data.Name == "Біогумус")
      //  result = new NPK(0.012, 0.0016, 0.0048) * GetСropСapacity();
      return result;
    }

    public override double GetСropСapacity() => 0.0;
    public override KeyValuePair<float, NPK> GetNutrients() => new KeyValuePair<float, NPK>(0, new NPK(0, 0, 0));
    public override NPK GetTakeout() => new NPK(0, 0, 0);

  }
}