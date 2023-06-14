using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models.Recommendation
{
    public class SideratesHumusCalculator : HumusCalculator
    {
        public SideratesHumusCalculator(string region, string mechanical, double productivityOfBasicProduction, double addedOrganicFertilizers, double? expectedLevelOfHumusPrev)
            : base(region, mechanical, addedOrganicFertilizers, expectedLevelOfHumusPrev)
        { }

        //Мінералізація гумусу в грунті люпин, т/га
        public override double GetMineralizationOfHumusInSoil() => 1.1 * CalculateMechCoef();

        public override double GetProductivityOfBasicProduction()
        {
            double productivityOfBasicProductionMax = 0;
            switch (region) {
              case "Березнівський":
                productivityOfBasicProductionMax = 250;
                break;
              case "Володимирецький":
                productivityOfBasicProductionMax = 250;
                break;
              case "Гощанський":
                productivityOfBasicProductionMax = 300;
                break;
              case "Демидівський":
                productivityOfBasicProductionMax = 300;
                break;
              case "Дубенський":
                productivityOfBasicProductionMax = 300;
                break;
              case "Дубровицький":
                productivityOfBasicProductionMax = 300;
                break;
              case "Зарічненський":
                productivityOfBasicProductionMax = 250;
                break;
              case "Здолбунівський":
                productivityOfBasicProductionMax = 300;
                break;
              case "Корецький":
                productivityOfBasicProductionMax = 300;
                break;
              case "Костопільський":
                productivityOfBasicProductionMax = 250;
                break;
              case "Млинівський":
                productivityOfBasicProductionMax = 300;
                break;
              case "Острозький":
                productivityOfBasicProductionMax = 300;
                break;
              case "Радивилівський":
                productivityOfBasicProductionMax = 300;
                break;
              case "Рівненський":
                productivityOfBasicProductionMax = 300;
                break;
              case "Рокитнівський":
                productivityOfBasicProductionMax = 250;
                break;
              case "Сарненський":
                productivityOfBasicProductionMax = 250;
                break;
            }
            double tmp = GetProductivityOfBasicProduction_F(productivityOfBasicProductionMax);
            return tmp;
        }


        //коеф.гуміф. орг. добр
        public double GetCoefHumOrgFert() => 0.23;

        // Надійде гумусу за рах. рослинної маси, т/га
        public override double WillShowHumusForAshPlantMass() =>
            GetProductivityOfBasicProduction() / 10.0 * 0.25 * GetCoefHumOrgFert();

        // надійде гумусу за рах. орг.добрив, т/га
        public override double WillGetHumusForAshOrganicFertilizers() =>
           addedOrganicFertilizers * coefGmuffOrgGood();

        // Всього новоутвореного гумусу, т /га
        public override double GetTotalNewlyFormedHumus() =>
            WillShowHumusForAshPlantMass() + WillGetHumusForAshOrganicFertilizers();

        // Баланс +/-, т/га
        public override double GetHumusBalance() =>
            GetTotalNewlyFormedHumus() - GetMineralizationOfHumusInSoil();

        public override string GetProduct() =>
            ProductsConstants.Siderate;//TODO: Check
    }
}