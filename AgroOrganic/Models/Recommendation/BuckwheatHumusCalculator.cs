using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models.Recommendation
{
    public class BuckwheatHumusCalculator : HumusCalculator
    {
        public BuckwheatHumusCalculator(string region, string mechanical, double productivityOfBasicProduction, double addedOrganicFertilizers,  double? expectedLevelOfHumusPrev)
            : base(region, mechanical, productivityOfBasicProduction, addedOrganicFertilizers, expectedLevelOfHumusPrev)
        { }

        //Мінералізація гумусу в грунті, т/га
        public override double GetMineralizationOfHumusInSoil() => 1.10 * CalculateMechCoef();

        public override double GetProductivityOfBasicProduction() {
            double productivityOfBasicProductionMax = 0;
            switch (region) {
              case "Березнівський":
                productivityOfBasicProductionMax = 6.10;
                break;
              case "Володимирецький":
                productivityOfBasicProductionMax = 4.70;
                break;
              case "Гощанський":
                productivityOfBasicProductionMax = 10.00;
                break;
              case "Демидівський":
                productivityOfBasicProductionMax = 12.00;
                break;
              case "Дубенський":
                productivityOfBasicProductionMax = 7.20;
                break;
              case "Дубровицький":
                productivityOfBasicProductionMax = 4.00;
                break;
              case "Зарічненський":
                productivityOfBasicProductionMax = 10.00;
                break;
              case "Здолбунівський":
                productivityOfBasicProductionMax = 5.00;
                break;
              case "Корецький":
                productivityOfBasicProductionMax = 9.30;
                break;
              case "Костопільський":
                productivityOfBasicProductionMax = 5.10;
                break;
              case "Млинівський":
                productivityOfBasicProductionMax = 14.60;
                break;
              case "Острозький":
                productivityOfBasicProductionMax = 6.70;
                break;
              case "Радивилівський":
                productivityOfBasicProductionMax = 9.80;
                break;
              case "Рівненський":
                productivityOfBasicProductionMax = 8.10;
                break;
              case "Рокитнівський":
                productivityOfBasicProductionMax = 4.70;
                break;
              case "Сарненський":
                productivityOfBasicProductionMax = 9.40;
                break;
            }
            return GetProductivityOfBasicProduction_F(productivityOfBasicProductionMax);
        }


        public double GetAdditioanlProductivity() =>
            GetProductivityOfBasicProduction() * 2.05;

        //коеф.гуміф. рослинної маси
        public override double GetCoefGmuffVegetableMass() => 0.22;

        // кореневі рештки, ц/га
        public override double GetRootStocks() =>
            GetProductivityOfBasicProduction() * 0.65 + 11.5;

        // надійде гумусу за рахунок кореневих решток, ц/га
        public override double WillReceiveHumusAtTheExpenseOfRootStocks() =>
            GetRootStocks() * CoefficientAccumulatedCornResidue;

        // поверхневі рештки, ц/га
        public override double GetSurfaceRemnants() =>
            GetProductivityOfBasicProduction() * 0.12 + 8.5;

        // надійде гумусу за рахунок поверхневих решток, ц/га
        public override double WillReceiveHumusAtTheExpenseOfSurfaceResidues() =>
            GetSurfaceRemnants() * GetCoefGmuffVegetableMass();

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