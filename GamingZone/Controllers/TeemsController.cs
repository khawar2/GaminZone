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
using GamingZone.ViewModels;

namespace GamingZone.Controllers
{
    [CustomAuthorize("Admin","User")]
    public class TeemsController : Controller
    {
        private GamingZoneEntities db = new GamingZoneEntities();
       
        // GET: Teems
        public ActionResult Index()
        {
            var teams = db.Teams.Include(t => t.Event);
            if (!IsAdmin())
            {
                int userid = GetUserId();
                return View(teams.Where(x=>x.UserId==userid).ToList());
            }
            return View(teams.ToList());
        }

        [HttpPost]
        public ActionResult Index(string SearchTeam)
        {
            var teams = db.Teams.Where(x => x.Name == SearchTeam).Include(t => t.Event);
            if (!IsAdmin())
            {
                int userid = GetUserId();
                return View(teams.Where(x => x.UserId == userid).ToList());
            }
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
            if (!IsAdmin())
            {
                int userid = GetUserId();
                ViewBag.User = userid;
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.EventId = new SelectList(db.Events, "Id", "Subject");
            return View();
        }

        // POST: Teems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,Name,NoOfPlayers,TeamRating,Description,EventId,UserId")] Team team)
        {
            int userid = GetUserId();
            if (!IsAdmin())
            {
                if (ModelState.IsValid)
                {
                    TeamRequest teamRequest = new TeamRequest();
                    teamRequest.Name = team.Name;
                    teamRequest.NoOfPlayers = team.NoOfPlayers;
                    teamRequest.Description = team.Description;
                    teamRequest.EventId = team.EventId;
                    teamRequest.UserId = userid ;

                    db.TeamRequests.Add(teamRequest);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.User = userid;
            }
            if (ModelState.IsValid)
            {
                db.Teams.Add(team);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.EventId = new SelectList(db.Events, "Id", "Subject");
            return View(team);
        }

        // GET: Teems/Edit/5
        public ActionResult Edit(int? id)
        {
            int userid = GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
         
            if (!IsAdmin())
            {
                ViewBag.User = userid;
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.EventId = new SelectList(db.Events, "Id", "Subject");
            return View(team);
        }

        // POST: Teems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Name,NoOfPlayers,TeamRating,Description,EventId,UserId")] Team team)
        {
            int userid = GetUserId();
            ViewBag.User = userid;
            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                db.SaveChanges();
                if (!IsAdmin())
                {
                    ViewBag.UserId = userid;
                }
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.EventId = new SelectList(db.Events, "Id", "Subject");
            return View(team);
        }
        
        // POST: Teems/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var players = db.Players.Where(c => c.TeamId == id).ToList();
            if (players != null)
            {
                foreach (var item in players)
                {
                    var prod = db.Ratings.Where(x => x.PlayerId == item.Id).ToList();
                    db.Ratings.RemoveRange(prod);
                }
                db.Players.RemoveRange(players);
                TempData["Delete"] = "Subcategory to this category exsits!";
            }
            Team team = db.Teams.Find(id);
            db.Teams.Remove(team);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public bool IsAdmin()
        {
            if (HttpContext.Session["Role"].ToString() == "User")
            {
                return false;
            }
            return true;
        }
        public int GetUserId()
        {
            var userId = Convert.ToString(HttpContext.Session["UserId"]);
            int id = Convert.ToInt32(userId);
            return id;
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
