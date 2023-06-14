using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models.Recommendation.FertilizersNeeds {
  public class FertilizersNeedsCalculator {

    protected readonly NPK m_takeoutNPK;

    protected readonly NPK m_soilTakeoutNPKcoef;
    protected readonly NPK m_mineralTakeoutNPKcoef;
    protected readonly NPK m_organicTakeoutNPKcoef1;
    protected readonly NPK m_organicTakeoutNPKcoef2;
    protected readonly double m_harvestPrice;

    // надходження NPK з біогумосом в Лісостепі
    protected readonly NPK m_incomeNPK = new NPK(0.84, 1.5, 0.9);
    protected readonly NPK m_sideratesIncomeNPK = new NPK(4.1, 1.1, 2.3);
    protected readonly double azotFix = 9.4;

    protected bool isSiderate;
    protected string mechanical;
    protected double addedFertilizer;
    protected double plannedHarvest;
    protected NPK NPKContents;
    protected FertilizersNeedsCalculator prevYearCalculator = null;

    protected double soilDensity => MechanicalCoefs.GetMechanicalCoef(mechanical); //1.3;//

    public FertilizersNeedsCalculator(FertilizersNeedsCalculator calc) {
      m_takeoutNPK = calc.m_takeoutNPK;
      m_soilTakeoutNPKcoef = calc.m_soilTakeoutNPKcoef;
      m_mineralTakeoutNPKcoef = calc.m_mineralTakeoutNPKcoef;
      m_organicTakeoutNPKcoef1 = calc.m_organicTakeoutNPKcoef1;
      m_organicTakeoutNPKcoef2 = calc.m_organicTakeoutNPKcoef2;
      m_harvestPrice = calc.m_harvestPrice;
      isSiderate = calc.isSiderate;

      mechanical = calc.mechanical;
      plannedHarvest = calc.plannedHarvest;
      addedFertilizer = calc.addedFertilizer;
      NPKContents = calc.NPKContents;
      //prevYearCalculator = calc.prevYearCalculator;
    }

    public FertilizersNeedsCalculator(NPK takeoutNPK, NPK soilTakeoutNPKcoef, NPK mineralTakeoutNPKcoef, NPK organicTakeoutNPKcoef1, NPK organicTakeoutNPKcoef2, double harvestPrice, bool isSiderate = false) {
      m_takeoutNPK = takeoutNPK;
      m_soilTakeoutNPKcoef = soilTakeoutNPKcoef;
      m_mineralTakeoutNPKcoef = mineralTakeoutNPKcoef;
      m_organicTakeoutNPKcoef1 = organicTakeoutNPKcoef1;
      m_organicTakeoutNPKcoef2 = organicTakeoutNPKcoef2;
      m_harvestPrice = harvestPrice;
      this.isSiderate = isSiderate;
    }
    
    public void SetData(string mechanical, double plannedHarvest, double addedFertilizer, NPK NPKContents, FertilizersNeedsCalculator prevYearCalculator) {
      this.mechanical = mechanical;
      this.plannedHarvest = plannedHarvest;//isSiderate ? 30 : plannedHarvest;
      this.addedFertilizer = addedFertilizer;
      this.NPKContents = NPKContents;
      this.prevYearCalculator = prevYearCalculator;
    }

    public NPK GetPlantNeeds() {
      return m_takeoutNPK * plannedHarvest;
    }

    //public NPK GetPlantNeedsPrev() {
    //  if (prevYearCalculator == null || !isSiderate) return NPK.Default;
    //  return prevYearCalculator.GetPlantNeeds()/30;
    //}

    public NPK GetAssimilateFromSoil() {
      return (NPKContents * 2 * soilDensity * m_soilTakeoutNPKcoef) / 1000;
    }
    //public NPK GetAssimilateFromSoilPrev() {
    //  if (prevYearCalculator == null || !isSiderate) return NPK.Default;
    //  return prevYearCalculator.GetAssimilateFromSoil();
    //}

    public NPK GetAssimilateFromBiohumus() {
      return m_incomeNPK * addedFertilizer / 100;
    }

    public NPK GetAssimilateFromBiohumusPrev() {
      if (prevYearCalculator == null) return NPK.Default;
      return prevYearCalculator.GetAssimilateFromBiohumus();
    }

    public NPK GetAssimilateFromSiderates() {
      if (!isSiderate) return NPK.Default;
      var result = (m_sideratesIncomeNPK * plannedHarvest);
      result.N += azotFix;

      return result / 1000;
    }

    //public NPK GetAssimilateFromSideratesPrev() {
    //  if (prevYearCalculator == null || !isSiderate) return NPK.Default;
    //  return (m_sideratesIncomeNPK * prevYearCalculator.plannedHarvest / 30) / 1000;
    //}

    public NPK GetBalance() {
      NPK takeOut = GetPlantNeeds();// + GetPlantNeedsPrev();

      NPK fromSoil = GetAssimilateFromSoil();// + GetAssimilateFromSoilPrev()
      NPK fromBiohumus = GetAssimilateFromBiohumus() * m_organicTakeoutNPKcoef1;
      NPK fromBiohumusPrev = GetAssimilateFromBiohumusPrev() * m_organicTakeoutNPKcoef2;
      NPK fromSiderates = GetAssimilateFromSiderates();// + GetAssimilateFromSideratesPrev();

      NPK income = fromSoil + fromBiohumus + fromBiohumusPrev + fromSiderates;

      return income - takeOut;
    }

    public double GetNeededFertilizer() {

      double neededFertilizer = 0;
      NPK lastBalance = NPK.Default;

      for (int j = 0; j < 1000; j++) {
        addedFertilizer = neededFertilizer;
        var balance = GetBalance();

        bool isNegativeN = balance.N < 0;
        bool isNegativeP = balance.P < 0;
        bool isNegativeK = balance.K < 0;

        if ((isNegativeN && !isNegativeK && !isNegativeP && lastBalance.N == balance.N) ||
            (isNegativeP && !isNegativeN && !isNegativeK && lastBalance.P == balance.P) ||
            (isNegativeK && !isNegativeN && !isNegativeP && lastBalance.K == balance.K))
          break;

        if (isNegativeN || isNegativeP || isNegativeK)
          neededFertilizer += 0.15;
        else
          break;

        if (neededFertilizer < 0) {
          neededFertilizer = 0;
          break;
        }

        lastBalance = balance;
      }

      addedFertilizer = neededFertilizer;
      return Math.Round(addedFertilizer, 3);
    }

    public double GetExpenses() {
      return Math.Round(addedFertilizer * 4000, 3);
    }

    public double GetIncome() {
      return Math.Round(plannedHarvest * m_harvestPrice, 3);
    }


  }
}