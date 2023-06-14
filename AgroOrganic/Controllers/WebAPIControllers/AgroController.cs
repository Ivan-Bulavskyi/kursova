using System;
using System.Collections.Generic;
using System.Linq;
using AgroOrganic.Models;
using System.Web.Mvc;
using AgroOrganic.Models.ModelContext;
using AgroOrganic.Models.MapModels;

namespace AgroOrganic.Controllers.WebAPIControllers
{
    public class AgroController : Controller
    {
        ModelContext modelContext = new ModelContext();
        MapContext mapContext = new MapContext();

        [HttpGet]
        public ActionResult GetAdvices()
        {
            List<MapLayer> mapPlayers = mapContext.MapLayers.ToList();
            List<Plant> plants = modelContext.Plants.ToList();
            List<AdviceToGrowForElement> advicesToGrow = modelContext.AdviceToGrowForElements.ToList();
            List<Advice> advices = advicesToGrow.Select(x => new Advice()
            {
                ElementName = mapPlayers.FirstOrDefault(e => e.Id == x.MapLayerId).Name,
                PlantName = plants.FirstOrDefault(e => e.PlantId == x.PlantId).Name,
                ImageName = plants.FirstOrDefault(e => e.PlantId == x.PlantId).ImageName
            }).ToList();
            return Json(new { advices }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSymptoms()
        {
            List<MapLayer> mapPlayers = mapContext.MapLayers.ToList();
            List<SymptomDescription> symptomsDescriptions = modelContext.SymptomDescriptions.ToList();

            var symptoms = symptomsDescriptions.Select(x => new SymptomDescriptionModel()
            {
                ElementName = mapPlayers.FirstOrDefault(e => e.Id == x.MapLayerId).Name,
                IsForDeficit = x.IsForDeficit,
                Description = x.Description
            });
            return Json(new { symptoms }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetRanges()
        {
            List<MapLayer> mapPlayers = mapContext.MapLayers.ToList();
            List<Ranx> ranges = modelContext.Ranges.ToList();
            
            //var chemElemRanges = ranges.Select(x => new OptimalData()
            //{
            //    nameElem = mapPlayers.FirstOrDefault(e => e.Id == x.MapLayerId).Name,
            //    badRanges = ranges.Where(e => e.RangeName == "Bad" && e.MapLayerId == x.MapLayerId).Select(r => new Range() { b = r.StartRange, e = r.EndRange }).ToList(),
            //    averageRanges = ranges.Where(e => e.RangeName == "Average" && e.MapLayerId == x.MapLayerId).Select(r => new Range() { b = r.StartRange, e = r.EndRange }).ToList(),
            //    optimalRange = ranges.Where(e => e.RangeName == "Optimal" && e.MapLayerId == x.MapLayerId).Select(r => new Range() { b = r.StartRange, e = r.EndRange }).FirstOrDefault()
            //});

            var chemElemRanges = mapPlayers.Select(x => new OptimalData()
            {
                nameElem = x.Name,
                badRanges = ranges.Where(e => e.RangeName == "Bad" && e.MapLayerId == x.Id).Select(r => new Range() { b = r.StartRange, e = r.EndRange }).ToList(),
                averageRanges = ranges.Where(e => e.RangeName == "Average" && e.MapLayerId == x.Id).Select(r => new Range() { b = r.StartRange, e = r.EndRange }).ToList(),
                optimalRange = ranges.Where(e => e.RangeName == "Optimal" && e.MapLayerId == x.Id).Select(r => new Range() { b = r.StartRange, e = r.EndRange }).FirstOrDefault()
            });
            return Json(new { chemElemRanges }, JsonRequestBehavior.AllowGet);
        }
    }
}