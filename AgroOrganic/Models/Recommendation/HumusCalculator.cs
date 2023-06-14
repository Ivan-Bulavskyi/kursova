using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models.Recommendation {
  public abstract class HumusCalculator {
    protected double productivityOfBasicProductionMax;
    protected double? productivityAfter;
    protected double addedOrganicFertilizers;
    protected double? expectedLevelOfHumusPrev;
    protected string region;
    protected string mechanical;
    public string soil;
    public bool calculateMechanical = true;

    public HumusCalculator(string region, string mechanical, double addedOrganicFertilizers = 0, double? expectedLevelOfHumusPrev = null, double? productivityAfter = null) {
      this.region = region;
      this.mechanical = mechanical;
      this.expectedLevelOfHumusPrev = expectedLevelOfHumusPrev;
      this.productivityAfter = productivityAfter;
      this.addedOrganicFertilizers = addedOrganicFertilizers;
    }

    public double CoefficientAccumulatedCornResidue { get; set; } = 0.20;

    public abstract double GetProductivityOfBasicProduction();

    public virtual double GetProductivityOfBasicProductionAfter() =>
        GetProductivityOfBasicProduction_F((double)productivityAfter);

    public virtual double GetCoefGmuffVegetableMass() =>
        throw new NotImplementedException();

    public abstract double GetMineralizationOfHumusInSoil();

    public virtual double GetRootStocks() =>
        throw new NotImplementedException();

    public virtual double WillReceiveHumusAtTheExpenseOfRootStocks() =>
        throw new NotImplementedException();

    public virtual double GetSurfaceRemnants() =>
        throw new NotImplementedException();

    public virtual double WillReceiveHumusAtTheExpenseOfSurfaceResidues() =>
        throw new NotImplementedException();

    public abstract double WillShowHumusForAshPlantMass();

    public abstract double WillGetHumusForAshOrganicFertilizers();

    public abstract double GetTotalNewlyFormedHumus();

    public abstract double GetHumusBalance();

    public abstract string GetProduct();

    protected double GetProductivityOfBasicProduction_F(double productivity) =>
        ProductivityOfBasicProduction.Get(region, productivity, (double)expectedLevelOfHumusPrev);

    protected double coefGmuffOrgGood() {
      switch (region) {
        case "Березнівський":
          return 0.126;
        case "Володимирецький":
          return 0.126;
        case "Гощанський":
          return 0.162;
        case "Демидівський":
          return 0.162;
        case "Дубенський":
          return 0.162;
        case "Дубровицький":
          return 0.126;
        case "Зарічненський":
          return 0.126;
        case "Здолбунівський":
          return 0.162;
        case "Корецький":
          return 0.162;
        case "Костопільський":
          return 0.126;
        case "Млинівський":
          return 0.162;
        case "Острозький":
          return 0.162;
        case "Радивилівський":
          return 0.162;
        case "Рівненський":
          return 0.162;
        case "Рокитнівський":
          return 0.126;
        case "Сарненський":
          return 0.126;
        default:
          return 0.126;
      }

      //throw new Exception("Region not found");
    }

    public virtual double CalculateMechCoef() {
      if (!calculateMechanical) return 1;
      return MechanicalCoefs.GetMechanicalCoef(mechanical) + 0.01 * SoilCoefs.GetSoilCoef(soil);
    }

  }
}