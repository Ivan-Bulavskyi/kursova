using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using AgroOrganic.Models.Recommendation;
using AgroOrganic.Models.Recommendation.FertilizersNeeds;
using Newtonsoft.Json;

namespace AgroOrganic.Controllers.WebAPIControllers {
  public class RecommendationsController : ApiController {

    [HttpPost]
    [Route("api/recommendation/get")]
    public string Recommendation(string region, double initialHumusContent, double area, string mechanical, string soil, bool useMech, [FromBody] List<UserPlan> userPlan) {
      var recommendations = new ProfitCalculator(userPlan, initialHumusContent, region, area, mechanical, soil, useMech).GetRecommendations();
      //Newtonsoft.Json.JsonSerializerSettings
      return JsonConvert.SerializeObject(recommendations);
    }

    [HttpPost]
    [Route("api/recommendation/getHumusBalance")]
    public double GetHumusBalance(string region, double initialHumusContent, double area, string mechanical, string soil, bool useMech, [FromBody] List<UserPlan> userPlan) {
      var humusBalance = new ProfitCalculator(userPlan, initialHumusContent, region, area, mechanical, soil, useMech).GetHumusBalance();
      return Math.Round(humusBalance, 3);
    }

    [HttpPost]
    [Route("api/recommendation/getAllAvailableCombinations")]
    public List<List<Recomendation>> GetAllAvailableCombinations(string region, double initialHumusContent, int allAddedFertilizer, double area, string mechanical, string soil, bool useMech, [FromBody] List<UserPlan> userPlan) {
      var recommendations = new ProfitCalculator(userPlan, initialHumusContent, region, area, mechanical, soil, useMech).GetAllAvailableRecommendations(userPlan, allAddedFertilizer, area);

      return recommendations;
    }

    [HttpPost]
    [Route("api/recommendation/getFertNeeds")]
    public string GetFertilizersRecommendations(string mechanical, string soil, string NPK, [FromBody] List<UserPlanFert> userPlan) {

      var npkArr = NPK.Replace(" ", string.Empty).Split(',');
      NPK npk = new NPK(Int32.Parse(npkArr[0]), Int32.Parse(npkArr[1]), Int32.Parse(npkArr[2]));
      var recommendations = new FertilizersProfitCalculator(mechanical, soil, npk, userPlan).GetRecommendations();

      return JsonConvert.SerializeObject(recommendations);
    }


  }
}
