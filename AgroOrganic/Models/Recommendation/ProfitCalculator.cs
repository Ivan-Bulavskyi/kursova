using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models.Recommendation
{
    public class ProfitCalculator
    {
        private List<UserPlan> plan;
        private double initialHumusContent;
        //private double coefGmuffOrgGood;
        private double area;
        private string region;
        private string mechanical;
        private string soil;
        private bool useMech = true;

        public ProfitCalculator() { }

        public ProfitCalculator(List<UserPlan> plan, double initialHumusContent, string region, double area, string mechanical, string soil, bool useMech)
        {
            this.plan = plan;
            this.initialHumusContent = initialHumusContent;
            this.region = region;
            this.mechanical = mechanical;
            this.soil = soil;
            this.area = area;
            this.useMech = useMech;
        }

        private HumusCalculator GetHumusCalculator(UserPlan plan, double? expectedLevelOfHumusPrev = null)
        {
            HumusCalculator result;
            switch (plan.Culture)
            {
                case CulturesConstants.Buckwheat: // гречка
                    result = new BuckwheatHumusCalculator(region, mechanical, plan.Productivity, plan.AddedFertilizer, expectedLevelOfHumusPrev);
                    break;
                case CulturesConstants.WinterWheat: // озима пшениця з післяжнивними сидератами
                    result =  new WinterWheatHumusCalculator(region, mechanical, plan.Productivity, plan.AddedFertilizer, expectedLevelOfHumusPrev, plan.ProductivityAfter);
                    break;
                case CulturesConstants.Oat:         // овес
                    result =  new OatHumusCalculator(region, mechanical, plan.Productivity, plan.AddedFertilizer, expectedLevelOfHumusPrev);
                    break;
                case CulturesConstants.Siderates:           // люпин
                    result =  new SideratesHumusCalculator(region, mechanical, plan.Productivity, plan.AddedFertilizer, expectedLevelOfHumusPrev);
                    break;
                case CulturesConstants.Soy:         // соя
                    result =  new SoyHumusCalculator(region, mechanical, plan.Productivity, plan.AddedFertilizer, expectedLevelOfHumusPrev);
                    break;
                case CulturesConstants.PerennialGrasses: // need to check, багаторічна трава, конюшина
                    result =  new PerennialGrassesHumusCalculator(region, mechanical, plan.Productivity, plan.AddedFertilizer, expectedLevelOfHumusPrev);
                    break;
                case CulturesConstants.WinterWheatPure: // озима пшениця
                    result =  new WinterWheatPureHumusCalculator(region, mechanical, plan.Productivity, plan.AddedFertilizer, expectedLevelOfHumusPrev);
                    break;
                case CulturesConstants.Wheat:           // жито
                    result =  new WheatPureHumusCalculator(region, mechanical, plan.Productivity, plan.AddedFertilizer, expectedLevelOfHumusPrev);
                    break;
                case CulturesConstants.WheatAndSiderates: // жито з післяжнивними сидератами. checked
                    result =  new WheatAndSideratesHumusCalculator(region, mechanical, plan.Productivity, plan.AddedFertilizer, expectedLevelOfHumusPrev, plan.ProductivityAfter);
                    break;
                case CulturesConstants.Maize:           // кукурудза
                    result =  new MaizeHumusCalculator(region, mechanical, plan.Productivity, plan.AddedFertilizer, expectedLevelOfHumusPrev);
                    break;
                default:
                    throw new Exception("Cultures class not found");
            }

            result.calculateMechanical = useMech;
            result.soil = soil;
            return result;
        }

        public List<ProductCosts> ProductCosts { get; set; } = new List<ProductCosts>
        {
            new ProductCosts { ProductName = "PerennialGrasses", Cost = 0 },//немає ціни чомусь
            new ProductCosts { ProductName = "Siderates", Cost = 0 },       //checked
            new ProductCosts { ProductName = "Soy", Cost = 20 },            //checked
            new ProductCosts { ProductName = "WinterWheat", Cost = 6.8 },   //checked
            new ProductCosts { ProductName = "Oat", Cost = 13 },            //checked овес + солома
            new ProductCosts { ProductName = "Buckwheat", Cost = 20 },      //checked
            new ProductCosts { ProductName = "Wheat", Cost = 12.6 },        //
            new ProductCosts { ProductName = "Maize", Cost = 17.4 },            //checked
            new ProductCosts { ProductName = "WheatAndSiderates", Cost = 12.6 }, //жито + післяжнивні сидерати
            new ProductCosts { ProductName = "WinterWheatPure", Cost = 6.8 }    //checked
    };

        private const double CostHumusPerHectar = 4.2;

        public double GetExpectedLevelOfHumus(double initialHumusContent, double humusBalance)
        {
            return Math.Round((initialHumusContent / 100.0 * 3500.0 + humusBalance) / 3500.0 * 100.0, 2);
        }

        public double GetAdditionalExpencesOnFertilizer(double addedFertilizer)
        {
            return Math.Round(addedFertilizer * CostHumusPerHectar, 2);
        }

        public double GetAdditionalExepencesOnCertification()
        {
            return 1.5;
        }

        public double GetSummaryProductCost(double productivity, string productName)
        {
            var cost = GetProductCost(productName);
            return productivity * cost / 10;
        }

        public double GetProductCost(string productName)
        {
            return ProductCosts.FirstOrDefault(x => x.ProductName == productName).Cost;
        }

        public double GetHumusBalance() {
            var humusCalculator = GetHumusCalculator(plan[0], initialHumusContent);
            var humusBalance = humusCalculator.GetHumusBalance();
            return humusBalance;
        }

        public List<Recomendation> GetRecommendations()
        {
            var recommendations = new List<Recomendation>();
            var plan_i = -1;
            plan.ForEach(x =>
            {
                plan_i++;
                var expectedLevelOfHumusPrev = plan_i > 0 ? (double?)recommendations[plan_i - 1].ExpectedLevelOfHumus : initialHumusContent;
                var humusCalculator = GetHumusCalculator(x, expectedLevelOfHumusPrev);
                var humusBalance = humusCalculator.GetHumusBalance();
                var product = humusCalculator.GetProduct();
                recommendations.Add(
                    new Recomendation
                    {
                        Year = recommendations.Count + 1,
                        HumusBalance = humusBalance,
                        ExpectedLevelOfHumus = recommendations.Count == 0 ? 
                          GetExpectedLevelOfHumus(initialHumusContent, humusBalance) :
                          GetExpectedLevelOfHumus(recommendations.Last().ExpectedLevelOfHumus, humusBalance),
                        AdditionalExepencesOnHumus = GetAdditionalExpencesOnFertilizer(x.AddedFertilizer),
                        SummaryProductCost = GetSummaryProductCost(x.Productivity, x.Culture) * area,
                        AdditionalExepencesOnCertification = GetAdditionalExepencesOnCertification() * area,
                        AddedFertilizer = x.AddedFertilizer,
                        Culture = x.Culture,
                        Product = product,
                        Productivity = x.Productivity,
                        ProductCost = GetProductCost(x.Culture),
                        Income = GetSummaryProductCost(x.Productivity, x.Culture) * area - GetAdditionalExpencesOnFertilizer(x.AddedFertilizer) * area - GetAdditionalExepencesOnCertification() * area
                    }
                );
            });

            return recommendations;
        }

        public List<List<Recomendation>> GetAllAvailableRecommendations(List<UserPlan> userPlan, int addedFertilizer, double area)
        {
            var combinationsOfRecommendations = new List<List<Recomendation>>();
            new UniqueMultipliers().GetCombinations(userPlan.Count, addedFertilizer).ForEach(x =>
            {
                for (int i = 0; i < userPlan.Count; i++)
                {
                    userPlan[i].AddedFertilizer = x[i];
                }

                var recommendations = new ProfitCalculator(userPlan, initialHumusContent, region, area, mechanical, soil, useMech).GetRecommendations();
                combinationsOfRecommendations.Add(recommendations);
            });

            return combinationsOfRecommendations.OrderByDescending(x => x.Last().ExpectedLevelOfHumus).ToList();
        }
    }

    public class ProductCosts
    {
        public string ProductName { get; set; }
        public double Cost { get; set; }
    }

    public class CulturesConstants
    {
        public const string PerennialGrasses = "PerennialGrasses"; //
        public const string WinterWheat = "WinterWheat";        // озима пшениця
        public const string Oat = "Oat";                        // овес
        public const string Buckwheat = "Buckwheat";            // гречка
        public const string Siderates = "Siderates";            // сидерати, люпин
        public const string Soy = "Soy";                        // соя
        public const string WinterWheatPure = "WinterWheatPure";//
        public const string Wheat = "Wheat";                    // жито
        public const string Maize = "Maize";                    //кукурудза
        public const string WheatAndSiderates = "WheatAndSiderates";    //жито + сидерати

    }

    public class ProductsConstants
    {
        public const string Hay = "сіно";
        public const string Grain = "зерно";
        public const string Siderate = "сидерацію";
    }
    public class MechanicalCoefs
    {
        public static double GetMechanicalCoef(string mechanical) {
            switch (mechanical) {
              case "Піщано-середньосуглинкові":
                return 1.8;
              case "Пилувато-легкосуглинкові":
                return 1.2;
              case "Піщано-легкосугликові":
                return 1.8;
              case "Легкоглиністі":
                return 1.2;
              case "Глинисто-піщані":
                return 1.8;
              case "Важкосуглинкові":
                return 0.81;
              case "Піщані":
                return 1.8;
              case "Крупнопилувато-середньосуглинкові":
                return 1;
              case "Супіщані":
                return 1.4;
              case "Середньо-та важкоглиністі":
                return 0.81;
              case "Піщано-важкосуглинкові":
                return 1.8;
              case "Крупнопилувато-легкосуглинкові":
                return 1.2;
              case "Пилувато-середньосуглинкові":
                return 1;
              case "Щебенюваті":
                return 2.0;
              default:
                return 1;
            }
        }
    }
    public class SoilCoefs
    {
        public static double GetSoilCoef(string soil) {
            switch (soil) {
              case "Дерново -прихованопідзолисті піщані та глинисто-піщані ґрунти (борові піски)":
                return 1.8;
              case "Дерново -слабо-і середньопідзолисті піщані та глинисто-піщані ґрунти":
                return 1.4;
              case "Дерново-середньо-і слабопідзолисті супіщані і суглинкові ґрунти":
                return 1.4;
              case "Дерново-слабопідзолисті глейові піщані та глинисто-піщані ґрунти":
                return 1.8;
              case "Дерново-середньо-і сильнопідзолисті глейові супіщані та суглинкові ґрунти":
                return 1.4;
              case "Дерново-середньо-і сильнопідзолисті поверхнево-оглеєні переважно суглинкові ґрунти":
                return 1.2;
              case "Ясно-сірі опідзолені ґрунти":
                return 1.2;
              case "Сірі опідзолені ґрунти":
                return 1.2;
              case "Темно-сірі опідзолені ґрунти":
                return 1.2;
              case "Чорноземи опідзолені":
                return 1.2;
              case "Ясно-сірі і сірі опідзолені оглеєні ґрунти":
                return 1.2;
              case "Темно-сірі опідзолені оглеєні ґрунти":
                return 1.2;
              case "Чорноземи опідзолені оглеєні":
                return 1.2;
              case "Чорноземи неглибокі слабогумусовані та малогумусні":
                return 1.2;
              case "Чорноземи глибокі на лесових породах":
                return 1.2;
              case "Чорноземи глибокі слабогумусовані":
                return 1.2;
              case "Чорноземи глибокі малогумусні":
                return 1.2;
              case "Чорноземи карбонатні на елювії щільних карбонатних порід":
                return 1.2;
              case "Лучно-чорноземні ґрунти":
                return 1.2;
              case "Лучні та чорноземно-лучні ґрунти":
                return 1.2;
              case "Лучно-болотні ґрунти":
                return 1.2;
              case "Болотні та торфувато-болотні ґрунти":
                return 1.2;
              case "Торфовища низинні та торфово-болотні ґрунти":
                return 1.2;
              case "Дернові піщані та глинисто-піщані ґрунти":
                return 1.2;
              case "Дернові оглеєні ґрунти":
                return 1.2;
              case "Дернові супіщані та суглинкові ґрунти":
                return 1.2;
              case "Дернові карбонатні ґрунти переважно на елювії щільних карбонатних порід":
                return 1.2;
              default:
                return 1.2;
            }
        }
    }
}