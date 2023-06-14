namespace AgroOrganic.Models.Recommendation
{

    public class WheatAndSideratesHumusCalculator : HumusCalculator
    {
        HumusCalculator after; 
        public WheatAndSideratesHumusCalculator(string region, string mechanical, double productivityOfBasicProduction, double addedOrganicFertilizers, double? expectedLevelOfHumusPrev, double productivityAfter)
            : base(region, mechanical, addedOrganicFertilizers, expectedLevelOfHumusPrev, productivityAfter)
        {
            after = new SideratesHumusCalculator(region, mechanical, productivityOfBasicProduction, addedOrganicFertilizers, expectedLevelOfHumusPrev);
        }

        //Мінералізація гумусу в грунті жита, т/га
        public override double GetMineralizationOfHumusInSoil() => 1.35 * CalculateMechCoef();

        public override double GetProductivityOfBasicProduction()
        {
            double productivityOfBasicProductionMax = 0;
            switch (region) {
              case "Березнівський":
                productivityOfBasicProductionMax = 24.2;
                break;
              case "Володимирецький":
                productivityOfBasicProductionMax = 20;
                break;
              case "Гощанський":
                productivityOfBasicProductionMax = 29.7;
                break;
              case "Демидівський":
                productivityOfBasicProductionMax = 26.1;
                break;
              case "Дубенський":
                productivityOfBasicProductionMax = 19.5;
                break;
              case "Дубровицький":
                productivityOfBasicProductionMax = 22.2;
                break;
              case "Зарічненський":
                productivityOfBasicProductionMax = 22.4;
                break;
              case "Здолбунівський":
                productivityOfBasicProductionMax = 27.1;
                break;
              case "Корецький":
                productivityOfBasicProductionMax = 23;
                break;
              case "Костопільський":
                productivityOfBasicProductionMax = 22;
                break;
              case "Млинівський":
                productivityOfBasicProductionMax = 23;
                break;
              case "Острозький":
                productivityOfBasicProductionMax = 29.7;
                break;
              case "Радивилівський":
                productivityOfBasicProductionMax = 26.7;
                break;
              case "Рівненський":
                productivityOfBasicProductionMax = 27;
                break;
              case "Рокитнівський":
                productivityOfBasicProductionMax = 25.4;
                break;
              case "Сарненський":
                productivityOfBasicProductionMax = 21;
                break;
            }
            return GetProductivityOfBasicProduction_F(productivityOfBasicProductionMax);
        }


        //Мінералізація гумусу в грунті люпин, т/га
        //public double GetMineralizationOfHumusInSoilAfter() => 1.5;

        //побічна продукція
        public double GetAdditioanlProductivity(double productivityOfBasicProduction) =>
            productivityOfBasicProduction * 2.17;

        //коеф.гуміф. орг. добр
        //public double GetCoefGumOrgFertAfter() => 0.0575;// 0.23; // 0.126;

        // коеф. гуміф. рослинної маси
        public override double GetCoefGmuffVegetableMass() => 0.2;

        // кореневі рештки, ц/га
        public override double GetRootStocks() =>
            GetProductivityOfBasicProduction() * 0.71 + 10.0;

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
//        public double WillShowHumusForAshPlantMassAfter() =>
//            (GetProductivityOfBasicProductionAfter() / 10) /* 0.25 */ * GetCoefGumOrgFertAfter();

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