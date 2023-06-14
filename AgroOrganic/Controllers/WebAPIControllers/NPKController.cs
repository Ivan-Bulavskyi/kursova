using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Http;
using Newtonsoft.Json;
using AgroOrganic.Models.Recommendation;

namespace AgroOrganic.Controllers.WebAPIControllers {
  public class NPKController : ApiController {

    private Dictionary<string, NPKCalculator> _calculators = new Dictionary<string, NPKCalculator>() {
        { CulturesConstants.Buckwheat, new BuckwheatNPKCalculator() }, // гречка
        { CulturesConstants.WinterWheatPure, new WinterWheatPureNPKCalculator() }, // озима пшениця з післяжнивними сидератами
        { CulturesConstants.WinterWheat, new WinterWheatNPKCalculator() }, // озима пшениця
        { CulturesConstants.Oat, new OatNPKCalculator() }, // овес
        { CulturesConstants.Soy, new SoyNPKCalculator() }, // соя
        { CulturesConstants.PerennialGrasses, new PerennialGrassesNPKCalculator() }, // багаторічна трава, конюшина
        { CulturesConstants.Siderates, new SideratesNPKCalculator() }, // люпин
        { CulturesConstants.Wheat, new WheatPureNPKCalculator() }, // жито
        { CulturesConstants.WheatAndSiderates, new WheatNPKCalculator() }, // жито з післяжнивними сидератами
        { CulturesConstants.Maize, new MaizeNPKCalculator() } // кукурудза
      };

    [HttpPost]
    [Route("api/npkbalance/get")]
    public string GetBalance(string culture, [FromBody] List<FertilizerData> fertilizers) {

      if (!_calculators.ContainsKey(culture)) return "";

      var calc = _calculators[culture];
      calc.SetFertilizers(fertilizers);
      return calc.GetBalance().ToString();
    }
  }

}