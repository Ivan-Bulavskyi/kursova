using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models.Recommendation
{
    public class PerennialGrassesHumusCalculator : HumusCalculator
    {
//        public PerennialGrassesHumusCalculator() { }

        public PerennialGrassesHumusCalculator(string region, string mechanical, double productivityOfBasicProduction, double addedOrganicFertilizers, double? expectedLevelOfHumusPrev = null)
            : base(region, mechanical, addedOrganicFertilizers, expectedLevelOfHumusPrev)
        { }

        public override double GetProductivityOfBasicProduction()
        {
            double productivityOfBasicProductionMax = 0;
            switch (region) {
              case "Березнівський":
                productivityOfBasicProductionMax = 20.7;
                break;
              case "Володимирецький":
                productivityOfBasicProductionMax = 20;
                break;
              case "Гощанський":
                productivityOfBasicProductionMax = 30;
                break;
              case "Демидівський":
                productivityOfBasicProductionMax = 25;
                break;
              case "Дубенський":
                productivityOfBasicProductionMax = 25;
                break;
              case "Дубровицький":
                productivityOfBasicProductionMax = 20;
                break;
              case "Зарічненський":
                productivityOfBasicProductionMax = 20;
                break;
              case "Здолбунівський":
                productivityOfBasicProductionMax = 20;
                break;
              case "Корецький":
                productivityOfBasicProductionMax = 25;
                break;
              case "Костопільський":
                productivityOfBasicProductionMax = 20;
                break;
              case "Млинівський":
                productivityOfBasicProductionMax = 35;
                break;
              case "Острозький":
                productivityOfBasicProductionMax = 36.7;
                break;
              case "Радивилівський":
                productivityOfBasicProductionMax = 35;
                break;
              case "Рівненський":
                productivityOfBasicProductionMax = 20;
                break;
              case "Рокитнівський":
                productivityOfBasicProductionMax = 15;
                break;
              case "Сарненський":
                productivityOfBasicProductionMax = 16;
                break;
            }
            return GetProductivityOfBasicProduction_F(productivityOfBasicProductionMax);
        }

        public override double GetMineralizationOfHumusInSoil() => 0.6 * CalculateMechCoef();

        public override double GetCoefGmuffVegetableMass() => 0.25;
        
        // кореневі рештки, ц/га
        public override double GetRootStocks()
        {
            return GetProductivityOfBasicProduction() * 1.02 + 4.7;
        }

        // надійде гумусу за рахунок кореневих решток, ц/га
        public override double WillReceiveHumusAtTheExpenseOfRootStocks()
        {
            return GetRootStocks() * CoefficientAccumulatedCornResidue;
        }

        // поверхневі рештки, ц/га
        public override double GetSurfaceRemnants()
        {
            return GetProductivityOfBasicProduction() * 0.12 + 5.9;
        }

        // надійде гумусу за рахунок поверхневих решток, ц/га
        public override double WillReceiveHumusAtTheExpenseOfSurfaceResidues()
        {
            return GetSurfaceRemnants() * GetCoefGmuffVegetableMass();
        }

        // Надійде гумусу за рах. рослинної маси, т/га
        public override double WillShowHumusForAshPlantMass()
        {
            return (WillReceiveHumusAtTheExpenseOfRootStocks() + WillReceiveHumusAtTheExpenseOfSurfaceResidues()) / 10;
        }

        // надійде гумусу за рах. орг.добрив, т/га
        public override double WillGetHumusForAshOrganicFertilizers()
        {
            return addedOrganicFertilizers * coefGmuffOrgGood();
        }

        // Всього новоутвореного гумусу, т /га
        public override double GetTotalNewlyFormedHumus()
        {
            return WillShowHumusForAshPlantMass() + WillGetHumusForAshOrganicFertilizers();
        }

        // Баланс +/-, т/га
        public override double GetHumusBalance()
        {
            return GetTotalNewlyFormedHumus() - GetMineralizationOfHumusInSoil();
        }

        public override string GetProduct()
        {
            return ProductsConstants.Hay;
        }
    }
}