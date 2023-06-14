namespace AgroOrganic.Models.Recommendation
{
    //курурудза
    public class MaizeHumusCalculator : HumusCalculator
    {
        public MaizeHumusCalculator(string region, string mechanical, double productivityOfBasicProduction, double addedOrganicFertilizers, double? expectedLevelOfHumusPrev)
            : base(region, mechanical, addedOrganicFertilizers, expectedLevelOfHumusPrev)
        { }

        public override double GetMineralizationOfHumusInSoil() => 1.56 * CalculateMechCoef();

        public override double GetProductivityOfBasicProduction()
        {
            double productivityOfBasicProductionMax = 0;
            switch (region) {
              case "Березнівський":
                productivityOfBasicProductionMax = 25.0;
                break;
              case "Володимирецький":
                productivityOfBasicProductionMax = 20.0;
                break;
              case "Гощанський":
                productivityOfBasicProductionMax = 62.7;
                break;
              case "Демидівський":
                productivityOfBasicProductionMax = 34.0;
                break;
              case "Дубенський":
                productivityOfBasicProductionMax = 46.7;
                break;
              case "Дубровицький":
                productivityOfBasicProductionMax = 25.0;
                break;
              case "Зарічненський":
                productivityOfBasicProductionMax = 25.0;
                break;
              case "Здолбунівський":
                productivityOfBasicProductionMax = 38.9;
                break;
              case "Корецький":
                productivityOfBasicProductionMax = 38.7;
                break;
              case "Костопільський":
                productivityOfBasicProductionMax = 40.9;
                break;
              case "Млинівський":
                productivityOfBasicProductionMax = 54.7;
                break;
              case "Острозький":
                productivityOfBasicProductionMax = 41.6;
                break;
              case "Радивилівський":
                productivityOfBasicProductionMax = 60.6;
                break;
              case "Рівненський":
                productivityOfBasicProductionMax = 40.3;
                break;
              case "Рокитнівський":
                productivityOfBasicProductionMax = 25.0;
                break;
              case "Сарненський":
                productivityOfBasicProductionMax = 29;
                break;
            }
            return GetProductivityOfBasicProduction_F(productivityOfBasicProductionMax);
        }


        public double GetAdditioanlProductivity() =>
            GetProductivityOfBasicProduction() * 1.76;

        public override double GetCoefGmuffVegetableMass() => 0.2;

        // кореневі рештки, ц/га
        public override double GetRootStocks() =>
            GetProductivityOfBasicProduction() * 0.83 + 7.2;

        // надійде гумусу за рахунок кореневих решток, ц/га
        public override double WillReceiveHumusAtTheExpenseOfRootStocks() =>
            GetRootStocks() * CoefficientAccumulatedCornResidue;

        // поверхневі рештки, ц/га
        public override double GetSurfaceRemnants() =>
            GetProductivityOfBasicProduction() * 0.2 + 1.6;

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