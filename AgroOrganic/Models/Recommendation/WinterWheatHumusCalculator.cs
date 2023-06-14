using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models.Recommendation
{
    public class WinterWheatHumusCalculator : HumusCalculator
    {
        HumusCalculator after;
        public WinterWheatHumusCalculator(string region, string mechanical, double productivityOfBasicProduction, double addedOrganicFertilizers, double? expectedLevelOfHumusPrev, double productivityAfter)
            : base(region, mechanical, addedOrganicFertilizers, expectedLevelOfHumusPrev, productivityAfter)
        {
            after = new SideratesHumusCalculator(region, mechanical, productivityOfBasicProduction, addedOrganicFertilizers, expectedLevelOfHumusPrev);
        }

        //Мінералізація гумусу в грунті пшениця, т/га
        public override double GetMineralizationOfHumusInSoil() => 1.25 * CalculateMechCoef();

        public override double GetProductivityOfBasicProduction()
        {
            double productivityOfBasicProductionMax = 0;
            switch (region) {
              case "Березнівський":
                productivityOfBasicProductionMax = 19.5;
                break;
              case "Володимирецький":
                productivityOfBasicProductionMax = 12.0;
                break;
              case "Гощанський":
                productivityOfBasicProductionMax = 45.6;
                break;
              case "Демидівський":
                productivityOfBasicProductionMax = 35.9;
                break;
              case "Дубенський":
                productivityOfBasicProductionMax = 40.3;
                break;
              case "Дубровицький":
                productivityOfBasicProductionMax = 15.5;
                break;
              case "Зарічненський":
                productivityOfBasicProductionMax = 8.3;
                break;
              case "Здолбунівський":
                productivityOfBasicProductionMax = 42.7;
                break;
              case "Корецький":
                productivityOfBasicProductionMax = 34;
                break;
              case "Костопільський":
                productivityOfBasicProductionMax = 12.6;
                break;
              case "Млинівський":
                productivityOfBasicProductionMax = 52;
                break;
              case "Острозький":
                productivityOfBasicProductionMax = 39.3;
                break;
              case "Радивилівський":
                productivityOfBasicProductionMax = 44.3;
                break;
              case "Рівненський":
                productivityOfBasicProductionMax = 35.9;
                break;
              case "Рокитнівський":
                productivityOfBasicProductionMax = 10.8;
                break;
              case "Сарненський":
                productivityOfBasicProductionMax = 25.6;
                break;
            }
            return GetProductivityOfBasicProduction_F(productivityOfBasicProductionMax);
        }


        //Мінералізація гумусу в грунті люпин, т/га
        //public double GetMineralizationOfHumusInSoilAfter() => 1.1;

        //побічна продукція, ц/га
        public double GetAdditioanlProductivity(double productivityOfBasicProduction) =>
            productivityOfBasicProduction * 1.32;

        //коеф.гуміф. орг. добр
        //public double GetCoefGumOrgFertAfter() => 0.23;

        public override double GetCoefGmuffVegetableMass() => 0.2;

        // кореневі рештки, ц/га
        public override double GetRootStocks() =>
            GetProductivityOfBasicProduction() * 0.71 + 10.1;

        // надійде гумусу за рахунок кореневих решток, ц/га
        public override double WillReceiveHumusAtTheExpenseOfRootStocks() =>
            GetRootStocks() * CoefficientAccumulatedCornResidue;

        // поверхневі рештки, ц/га
        public override double GetSurfaceRemnants() =>
            GetProductivityOfBasicProduction() * 0.32 + 13.5;

        // надійде гумусу за рахунок поверхневих решток, ц/га
        public override double WillReceiveHumusAtTheExpenseOfSurfaceResidues() =>
            GetSurfaceRemnants() * GetCoefGmuffVegetableMass();

        // Надійде гумусу за рах. рослинної маси, т/га
        public override double WillShowHumusForAshPlantMass() =>
            (GetSurfaceRemnants() + GetRootStocks() + GetAdditioanlProductivity(GetProductivityOfBasicProduction())) / 10 * GetCoefGmuffVegetableMass();

        // Надійде гумусу за рах. рослинної маси, т/га
        //public double WillShowHumusForAshPlantMassAfter() =>
//            (GetProductivityOfBasicProductionAfter() / 10) * 0.25 * GetCoefGumOrgFertAfter();

        // надійде гумусу за рах. орг.добрив, т/га
        public override double WillGetHumusForAshOrganicFertilizers() =>
            addedOrganicFertilizers * coefGmuffOrgGood();

        // Всього новоутвореного гумусу, т /га
        public override double GetTotalNewlyFormedHumus() =>
            WillShowHumusForAshPlantMass() + WillGetHumusForAshOrganicFertilizers() + after.WillShowHumusForAshPlantMass();

        // Баланс +/-, т/га
        public override double GetHumusBalance() =>
            GetTotalNewlyFormedHumus() - GetMineralizationOfHumusInSoil() - after.GetMineralizationOfHumusInSoil();

        public override string GetProduct() => ProductsConstants.Grain;
    }
}