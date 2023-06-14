using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models.Recommendation
{
    public class SoyHumusCalculator : HumusCalculator
    {
        public SoyHumusCalculator(string region, string mechanical, double productivityOfBasicProduction, double addedOrganicFertilizers, double? expectedLevelOfHumusPrev)
            : base(region, mechanical, addedOrganicFertilizers, expectedLevelOfHumusPrev)
        { }

        //Мінералізація гумусу в грунті, т/га
        public override double GetMineralizationOfHumusInSoil() => 1.5 * CalculateMechCoef();

        public override double GetProductivityOfBasicProduction()
        {
            double productivityOfBasicProductionMax = 0;
            switch (region) {
              case "Березнівський":
                productivityOfBasicProductionMax = 6.5;
                break;
              case "Володимирецький":
                productivityOfBasicProductionMax = 6.5;
                break;
              case "Гощанський":
                productivityOfBasicProductionMax = 14;
                break;
              case "Демидівський":
                productivityOfBasicProductionMax = 10;
                break;
              case "Дубенський":
                productivityOfBasicProductionMax = 17;
                break;
              case "Дубровицький":
                productivityOfBasicProductionMax = 1;
                break;
              case "Зарічненський":
                productivityOfBasicProductionMax = 1;
                break;
              case "Здолбунівський":
                productivityOfBasicProductionMax = 15;
                break;
              case "Корецький":
                productivityOfBasicProductionMax = 15;
                break;
              case "Костопільський":
                productivityOfBasicProductionMax = 1;
                break;
              case "Млинівський":
                productivityOfBasicProductionMax = 16;
                break;
              case "Острозький":
                productivityOfBasicProductionMax = 17;
                break;
              case "Радивилівський":
                productivityOfBasicProductionMax = 13;
                break;
              case "Рівненський":
                productivityOfBasicProductionMax = 10;
                break;
              case "Рокитнівський":
                productivityOfBasicProductionMax = 1;
                break;
              case "Сарненський":
                productivityOfBasicProductionMax = 7;
                break;
            }
            return GetProductivityOfBasicProduction_F(productivityOfBasicProductionMax);
        }


        //Урожайність основної продукції, ц/га
        public double GetAdditioanlProductivity() => 
            GetProductivityOfBasicProduction() * 1.02;

        //коеф.гуміф.рослинної маси
        public override double GetCoefGmuffVegetableMass() => 0.14;

        // кореневі рештки, ц/га
        public override double GetRootStocks() =>
             GetProductivityOfBasicProduction() * 0.36 + 8.9;

        // поверхневі рештки, ц/га
        public override double GetSurfaceRemnants() =>
             GetProductivityOfBasicProduction() * 0.21 + 4.5;

        // Надійде гумусу за рах. рослинної маси, т/га
        public override double WillShowHumusForAshPlantMass() =>
            (GetSurfaceRemnants() + GetRootStocks() + GetAdditioanlProductivity()) / 10 * GetCoefGmuffVegetableMass();

        // надійде гумусу за рах. орг.добрив, т/га
        public override double WillGetHumusForAshOrganicFertilizers() => 
            addedOrganicFertilizers * coefGmuffOrgGood();

        // Всього новоутвореного гумусу, т /га
        public override double GetTotalNewlyFormedHumus() =>
            WillShowHumusForAshPlantMass() + WillGetHumusForAshOrganicFertilizers();

        // Баланс +/-, т/га
        public override double GetHumusBalance() =>
            GetTotalNewlyFormedHumus() - GetMineralizationOfHumusInSoil();

        public override string GetProduct() => ProductsConstants.Grain;
    }
}