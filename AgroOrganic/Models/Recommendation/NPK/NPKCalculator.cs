using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models.Recommendation {
  public abstract class NPKCalculator {

    protected List<FertilizerData> fertilizers = new List<FertilizerData>();

    public virtual void SetFertilizers(List<FertilizerData> fertilizers) {
      this.fertilizers = fertilizers;
    }

    public abstract double GetСropСapacity();
    public abstract KeyValuePair<float, NPK> GetNutrients();
    public abstract NPK GetTakeout();
    public NPK GetSeedNutrients() {
      var nutrient = GetNutrients();
      double N = (nutrient.Key / 10 * nutrient.Value.N) / 1000;
      double P = (nutrient.Key / 10 * nutrient.Value.P) / 1000;
      double K = (nutrient.Key / 10 * nutrient.Value.K) / 1000;
      return new NPK(N, P, K);
    }

    //income
    public virtual NPK GetOrganicNPK(FertilizerData data) {
      var result = NPK.Default;
      switch (data.Name) {
        case "Біогумус":
          result = new NPK(0.84, 1.5, 0.9) / 100;
          break;
        case "Екосойл":
        case "Екоплант":
          result = new NPK(0, 0.05, 0.3);
          break;
        case "Фосфоритне борошно":
          result = new NPK(0, 0.23, 0.022);
          break;
        case "Сапропель":
          result = new NPK(0.42, 0.07, 0) / 100;
          break;
        case "Зелене добриво (сидерати-люпин)":
          result = new NPK(4.1 / 1000, 1.1 / 1000, 2.3 / 1000);
          break;
        case "Зелене добриво (сидерати-редька олійна)":
          result = new NPK(3.7 / 1000, 0.12 / 1000, 4 / 1000);
          break;
      }
      return result * data.Value;
    }

    public virtual NPK GetOtherIncomeNPK(FertilizerData data) {
      var result = NPK.Default;
      switch (data.Name) {
        case "Біогумус":
          var nutrient = GetSeedNutrients();
          double N_Precipitation = 8.7 / 1000;
          double K_Precipitation = 2 / 1000;
          result = new NPK(N_Precipitation + nutrient.N, nutrient.P, K_Precipitation + nutrient.K);
          break;
        case "Зелене добриво (сидерати-люпин)":
        case "Зелене добриво (сидерати-редька олійна)":
          var grassesNPKCalc = new PerennialGrassesNPKCalculator();
          var nutrient2 = grassesNPKCalc.GetSeedNutrients();
          double azotfix = data.Value * 9.4 / 1000;
          result = new NPK(azotfix + nutrient2.N, nutrient2.P, nutrient2.K);
          break;
      }
      return result;
    }

    public virtual NPK GetTotalIncome() {
      List<NPK> organicNPKs = new List<NPK>();
      List<NPK> otherNPKs = new List<NPK>();

      fertilizers.ForEach(f => {
        organicNPKs.Add(GetOrganicNPK(f));
        otherNPKs.Add(GetOtherIncomeNPK(f));
      });

      double N = organicNPKs.Sum(o => o.N) + otherNPKs.Sum(o => o.N);
      double P = organicNPKs.Sum(o => o.P) + otherNPKs.Sum(o => o.P);
      double K = organicNPKs.Sum(o => o.K) + otherNPKs.Sum(o => o.K);

      return new NPK(N, P, K);
    }


    //takeout
    public virtual NPK GetHarvestNPK(FertilizerData data) {
      var result = NPK.Default;
      switch (data.Name) {
        case "Біогумус":
          result = GetTakeout() * GetСropСapacity();
          break;
        case "Солома":
          result = new NPK(10 / 1000, 0, 0) * data.Value;
          break;
        case "Зелене добриво (сидерати-люпин)":
        case "Зелене добриво (сидерати-редька олійна)":
          var grassesNPKCalc = new PerennialGrassesNPKCalculator();
          result = grassesNPKCalc.GetTakeout() * data.Value;
          break;
      }
      return result;
    }

    public virtual NPK GetAdditionalExpenses(FertilizerData data) {
      var result = NPK.Default;
      switch (data.Name) {
        case "Біогумус":
          result = GetHarvestNPK(data) * 0.22;
          break;
        case "Зелене добриво (сидерати-люпин)":
        case "Зелене добриво (сидерати-редька олійна)":
          result = GetTakeout() * data.Value;
          break;
      }
      return result;
    }

    public virtual NPK GetTotalTakeout() {
      List<NPK> harvestNPKs = new List<NPK>();
      List<NPK> additionalNPKs = new List<NPK>();

      fertilizers.ForEach(f => {
        harvestNPKs.Add(GetHarvestNPK(f));
        additionalNPKs.Add(GetAdditionalExpenses(f));
      });

      double N = harvestNPKs.Sum(o => o.N) + additionalNPKs.Sum(o => o.N);
      double P = harvestNPKs.Sum(o => o.P) + additionalNPKs.Sum(o => o.P);
      double K = harvestNPKs.Sum(o => o.K) + additionalNPKs.Sum(o => o.K);

      return new NPK(N, P, K);
    }

    public virtual NPK GetBalance() {
      return GetTotalIncome() - GetTotalTakeout();
    }


  }
}