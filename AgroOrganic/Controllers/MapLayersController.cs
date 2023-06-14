﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AgroOrganic.Models.MapModels;

namespace AgroOrganic.Controllers
{
    public class MapLayersController : Controller
    {
        private MapContext db = new MapContext();

        // GET: MapLayers
        public async Task<ActionResult> Index()
        {
            return View(await db.MapLayers.ToListAsync());
        }

        // GET: MapLayers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MapLayer mapLayer = await db.MapLayers.FindAsync(id);
            if (mapLayer == null)
            {
                return HttpNotFound();
            }
            return View(mapLayer);
        }

        // GET: MapLayers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MapLayers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Measure,MinValue,MaxValue")] MapLayer mapLayer)
        {
            if (ModelState.IsValid)
            {
                db.MapLayers.Add(mapLayer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(mapLayer);
        }

        // GET: MapLayers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MapLayer mapLayer = await db.MapLayers.FindAsync(id);
            if (mapLayer == null)
            {
                return HttpNotFound();
            }
            return View(mapLayer);
        }

        // POST: MapLayers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Measure,MinValue,MaxValue")] MapLayer mapLayer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mapLayer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(mapLayer);
        }

        // GET: MapLayers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MapLayer mapLayer = await db.MapLayers.FindAsync(id);
            if (mapLayer == null)
            {
                return HttpNotFound();
            }
            return View(mapLayer);
        }

        // POST: MapLayers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MapLayer mapLayer = await db.MapLayers.FindAsync(id);
            db.MapLayers.Remove(mapLayer);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
