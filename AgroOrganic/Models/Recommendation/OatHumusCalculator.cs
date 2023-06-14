using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models.Recommendation
{
    public class OatHumusCalculator : HumusCalculator
    {
        public OatHumusCalculator(string region, string mechanical, double productivityOfBasicProduction, double addedOrganicFertilizers, double? expectedLevelOfHumusPrev)
            : base(region, mechanical, addedOrganicFertilizers, expectedLevelOfHumusPrev)
        { }

        public override double GetMineralizationOfHumusInSoil() => 1.20 * CalculateMechCoef();

        public override double GetProductivityOfBasicProduction()
        {
            double productivityOfBasicProductionMax = 0;
            switch (region) {
              case "Березнівський":
                productivityOfBasicProductionMax = 18.0;
                break;
              case "Володимирецький":
                productivityOfBasicProductionMax = 18.0;
                break;
              case "Гощанський":
                productivityOfBasicProductionMax = 24.4;
                break;
              case "Демидівський":
                productivityOfBasicProductionMax = 17.7;
                break;
              case "Дубенський":
                productivityOfBasicProductionMax = 21.6;
                break;
              case "Дубровицький":
                productivityOfBasicProductionMax = 22.6;
                break;
              case "Зарічненський":
                productivityOfBasicProductionMax = 24.0;
                break;
              case "Здолбунівський":
                productivityOfBasicProductionMax = 27.8;
                break;
              case "Корецький":
                productivityOfBasicProductionMax = 19.0;
                break;
              case "Костопільський":
                productivityOfBasicProductionMax = 20.8;
                break;
              case "Млинівський":
                productivityOfBasicProductionMax = 25.5;
                break;
              case "Острозький":
                productivityOfBasicProductionMax = 20.0;
                break;
              case "Радивилівський":
                productivityOfBasicProductionMax = 17.4;
                break;
              case "Рівненський":
                productivityOfBasicProductionMax = 20.0;
                break;
              case "Рокитнівський":
                productivityOfBasicProductionMax = 23.6;
                break;
              case "Сарненський":
                productivityOfBasicProductionMax = 19.9;
                break;
            }
            return GetProductivityOfBasicProduction_F(productivityOfBasicProductionMax);
        }


        public double GetAdditioanlProductivity() =>
            GetProductivityOfBasicProduction() * 1.35;

        public override double GetCoefGmuffVegetableMass() => 0.22;

        // кореневі рештки, ц/га
        public override double GetRootStocks() =>
            GetProductivityOfBasicProduction() * 0.42 + 8.4;

        // надійде гумусу за рахунок кореневих решток, ц/га
        public override double WillReceiveHumusAtTheExpenseOfRootStocks() =>
            GetRootStocks() * CoefficientAccumulatedCornResidue;

        // поверхневі рештки, ц/га
        public override double GetSurfaceRemnants() =>
            GetProductivityOfBasicProduction() * 0.19 + 14.8;

        // надійде гумусу за рахунок поверхневих решток, ц/га
        public override double WillReceiveHumusAtTheExpenseOfSurfaceResidues() =>
            GetSurfaceRemnants() * GetCoefGmuffVegetableMass();

        // Надійде гумусу за рах. рослинної маси, т/га
        public override double WillShowHumusForAshPlantMass() =>
            (GetSurfaceRemnants() + GetRootStocks() + GetAdditioanlProductivity()) / 10 * GetCoefGmuffVegetableMass();

        // надійде гумусу за рах. орг.добрив, т/га
        public override double WillGetHumusForAshOrganicFertilizers() =>
            addedOrganicFertilizers * this.coefGmuffOrgGood();

        // Всього новоутвореного гумусу, т /га
        public override double GetTotalNewlyFormedHumus() =>
            WillShowHumusForAshPlantMass() + WillGetHumusForAshOrganicFertilizers();

        // Баланс +/-, т/га
        public override double GetHumusBalance() =>
            GetTotalNewlyFormedHumus() - GetMineralizationOfHumusInSoil();

        public override string GetProduct() => ProductsConstants.Grain;
    }
}