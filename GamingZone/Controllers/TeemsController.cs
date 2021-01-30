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
    public class TeemsController : Controller
    {
        private GamingZoneEntities db = new GamingZoneEntities();

        // GET: Teems
        public ActionResult Index()
        {
            var teams = db.Teams.Include(t => t.Event);
            return View(teams.ToList());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(string SearchTeam)
        {
            return View(db.Teams.Include(t => t.Event).Where(x => x.Name == SearchTeam).ToList());
        }


        // GET: Teems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // GET: Teems/Create
        public ActionResult Create()
        {
            ViewBag.EventId = new SelectList(db.Events, "Id", "Subject");
            return View();
        }

        // POST: Teems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,NoOfPlayers,TeamRating,Description,EventId")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Teams.Add(team);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventId = new SelectList(db.Events, "Id", "Subject", team.EventId);
            return View(team);
        }

        // GET: Teems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventId = new SelectList(db.Events, "Id", "Subject", team.EventId);
            return View(team);
        }

        // POST: Teems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,NoOfPlayers,TeamRating,Description,EventId")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventId = new SelectList(db.Events, "Id", "Subject", team.EventId);
            return View(team);
        }

        // GET: Teems/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Team team = db.Teams.Find(id);
        //    if (team == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(team);
        //}

        // POST: Teems/Delete/5
        [HttpGet]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Team team = db.Teams.Find(id);
            db.Teams.Remove(team);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public ActionResult IndividualTeam(int id)
        {
            Team team = db.Teams.Find(id);
            db.Teams.Remove(team);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IndividualTeam()
        {
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
