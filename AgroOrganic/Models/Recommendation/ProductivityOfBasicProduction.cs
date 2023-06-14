using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models.Recommendation
{
    public static class ProductivityOfBasicProduction
    {
        public static double Get(string region, double productivity, double expectedLevelOfHumusPrev)
        {
            switch (region)
            {
                case "Радивилівський":
                    return F_forestStepp(productivity, expectedLevelOfHumusPrev);
                case "Гощанський":
                    return F_forestStepp(productivity, expectedLevelOfHumusPrev);
                case "Сарненський":
                    return F_woodland(productivity, expectedLevelOfHumusPrev);
                default:
                  return F_forestStepp(productivity, expectedLevelOfHumusPrev);
            }

            throw new Exception("Region not found");
        }

        // полісся
        private static double F_woodland(double productivity, double expectedLevelOfHumusPrev)
        {
            if (expectedLevelOfHumusPrev >= 2.1)
            {
                return productivity * 1.1;
            }
            else if (expectedLevelOfHumusPrev >= 2)
            {
                return productivity * 1.05;
            }
            else if (expectedLevelOfHumusPrev >= 1.9)
            {
                return productivity;
            }
            else return productivity * 0.8;
        }

        // лісостеп
        private static double F_forestStepp(double productivity, double expectedLevelOfHumusPrev)
        {
            if (expectedLevelOfHumusPrev >= 2.4)
            {
                return productivity * 1.1;
            }
            else if (expectedLevelOfHumusPrev >= 2.3)
            {
                return productivity * 1.05;
            }
            else if (expectedLevelOfHumusPrev >= 2.2)
            {
                return productivity;
            }
            else return productivity * 0.8;
        }
    }
}