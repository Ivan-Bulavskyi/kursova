using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models.Recommendation.FertilizersNeeds {
  public class FertilizersProfitCalculator {

    private List<UserPlanFert> plan;
    private string mechanical;
    private string soil;
    private NPK NPK;

    private Dictionary<string, FertilizersNeedsCalculator> _calculators = new Dictionary<string, FertilizersNeedsCalculator>() {
          {
            CulturesConstants.Buckwheat, // гречка
            new FertilizersNeedsCalculator(
              new NPK(0.031, 0.014, 0.038),
              new NPK(0.2, 0.15, 0.4),
              new NPK(0.6, 0.25, 0.7),
              new NPK(0.25, 0.35, 0.55),
              new NPK(0.15, 0.15, 0.25),
              21000)
          },
          {
            CulturesConstants.WinterWheatPure, // озима пшениця з післяжнивними сидератами
            new FertilizersNeedsCalculator(
              new NPK(0.029, 0.012, 0.022),
              new NPK(0.3, 0.1, 0.2),
              new NPK(0.7, 0.25, 0.5),
              new NPK(0.25, 0.35, 0.55),
              new NPK(0.15, 0.15, 0.25),
              7200)
          },
          {
            CulturesConstants.WinterWheat, // озима пшениця
            new FertilizersNeedsCalculator(
              new NPK(0.029, 0.012, 0.022),
              new NPK(0.3, 0.1, 0.2),
              new NPK(0.7, 0.25, 0.5),
              new NPK(0.25, 0.35, 0.55),
              new NPK(0.15, 0.15, 0.25),
              7200)
          },
          {
            CulturesConstants.Oat, // овес
            new FertilizersNeedsCalculator(
              new NPK(0.03, 0.014, 0.025),
              new NPK(0.2, 0.15, 0.4),
              new NPK(0.6, 0.25, 0.7),
              new NPK(0.25, 0.35, 0.55),
              new NPK(0.15, 0.15, 0.25),
              7200)
          },
          {
            CulturesConstants.Soy, // соя
            new FertilizersNeedsCalculator(
              new NPK(0.067, 0.015, 0.02),
              new NPK(0.3, 0.1, 0.2),
              new NPK(0.7, 0.25, 0.5),
              new NPK(0.25, 0.35, 0.55),
              new NPK(0.15, 0.15, 0.25),
              12500)
          },
          {
            CulturesConstants.PerennialGrasses, // багаторічна трава, конюшина
            new FertilizersNeedsCalculator(
              new NPK(0.012, 0.0016, 0.0048),
              new NPK(0, 0.1, 0.15),
              new NPK(0, 0.2, 0.7),
              new NPK(0.5, 0, 0),
              new NPK(0.2, 0, 0), 
              0)
          },
          {
            CulturesConstants.Siderates, // люпин
            new FertilizersNeedsCalculator(
              new NPK(0.012, 0.0016, 0.0048),
              new NPK(0, 0.1, 0.15),
              new NPK(0, 0.2, 0.7),
              new NPK(0.5, 0, 0),
              new NPK(0.2, 0, 0), 
              0, true)
          },
          {
            CulturesConstants.Wheat, // жито
            new FertilizersNeedsCalculator(
              new NPK(0.026, 0.01, 0.025),
              new NPK(0.3, 0.1, 0.2),
              new NPK(0.7, 0.25, 0.5),
              new NPK(0.25, 0.35, 0.55),
              new NPK(0.15, 0.15, 0.25),
              7200)
          },
          {
            CulturesConstants.WheatAndSiderates, // жито з післяжнивними сидератами
            new FertilizersNeedsCalculator(
              new NPK(0.026, 0.01, 0.025),
              new NPK(0.3, 0.1, 0.2),
              new NPK(0.7, 0.25, 0.5),
              new NPK(0.25, 0.35, 0.55),
              new NPK(0.15, 0.15, 0.25),
              7200)
          },
          {
            CulturesConstants.Maize, // кукурудза
            new FertilizersNeedsCalculator(
              new NPK(0.025, 0.01, 0.032),
              new NPK(0.15, 0.1, 0.3),
              new NPK(0.5, 0.2, 0.6),
              new NPK(0.25, 0.35, 0.55),
              new NPK(0.15, 0.15, 0.25),
              12500)
          }
        };

    public FertilizersProfitCalculator(string mechanical, string soil, NPK NPK, List<UserPlanFert> plan) {
      this.mechanical = mechanical;
      this.soil = soil;
      this.plan = plan;
      this.NPK = NPK; //new NPK(100, 150, 100);//
    }

    public List<RecomendationFert> GetRecommendations() {
      var recommendations = new List<RecomendationFert>();

      FertilizersNeedsCalculator calc;
      FertilizersNeedsCalculator prevCalc = null;
      for (int i = 0; i < plan.Count; i++) {
        var x = plan[i];

        calc = _calculators[x.Culture];

        calc.SetData(mechanical, x.PlannedHarvest, x.AddedFertilizer, NPK, prevCalc);
        double neededFertilizer = calc.GetNeededFertilizer();
        double exepencesOnFertilizer = calc.GetExpenses();
        double income = calc.GetIncome();

        recommendations.Add(
            new RecomendationFert {
              Culture = x.Culture,
              Year = i + 1,
              NPKBalance = calc.GetBalance().ToString(),
              AddedFertilizer = neededFertilizer,
              ExepencesOnFertilizer = exepencesOnFertilizer,
              Income = income,
              Payback = income - exepencesOnFertilizer,
            }
        );

        prevCalc = new FertilizersNeedsCalculator(calc);
      }

      return recommendations;
    }

  }

  public class RecomendationFert {
    public string Culture { get; set; }
    public string Product { get; set; }
    public int Year { get; set; }
    public string NPKBalance { get; set; }
    public double AddedFertilizer { get; set; }
    public double ExepencesOnFertilizer { get; set; }
    public double Income { get; set; }
    public double Payback { get; set; }
  }

}