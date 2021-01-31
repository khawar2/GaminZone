using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GamingZone.Infrastructure;
using GamingZone.Models;

namespace GamingZone.Controllers
{
    [CustomAuthorize("Admin")]
    public class EventsController : Controller
    {
        private GamingZoneEntities db = new GamingZoneEntities();

        // GET: Events
        public ActionResult Index()
        {
            return View(db.Events.ToList());
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,Subject,Description,Start,End,Date")] Event @event)
        {
            if (ModelState.IsValid)
            {
                @event.Date = @event.Date.Value.Date;
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@event);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Subject,Description,Start,End,Date")] Event @event)
        {
            if (ModelState.IsValid)
            {
                @event.Date = @event.Date.Value.Date;
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@event);
        }
        

        // POST: Events/Delete/5
        [HttpGet, ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            Event @event = db.Events.Find(id);
            var teams = db.Teams.Where(c => c.EventId == id).ToList();
            if (teams != null)
            {
                foreach (var itemTeams in teams)
                {
                    var players = db.Players.Where(x => x.TeamId == itemTeams.Id).ToList();

                    foreach (var itempalyers in players)
                    {
                        var prod = db.Ratings.Where(x => x.PlayerId == itempalyers.Id).ToList();
                        db.Ratings.RemoveRange(prod);
                    }
                    db.Players.RemoveRange(players);
                }

                db.Teams.RemoveRange(teams);
            }
            db.Events.Remove(@event);
            db.SaveChanges();
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
