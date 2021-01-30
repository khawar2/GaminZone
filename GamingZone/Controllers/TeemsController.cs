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
    [CustomAuthorize("Admin","User")]
    public class TeemsController : Controller
    {
        private GamingZoneEntities db = new GamingZoneEntities();
        private HttpContextBase _httpContext;
        public TeemsController(HttpContextBase http)
        {
            _httpContext = http;

        }
        // GET: Teems
        public ActionResult Index()
        {
            var userId = Convert.ToString(_httpContext.Session["UserId"]);
            int id = Convert.ToInt32(userId);
            var teams = db.Teams.Include(t => t.Event);
            if (_httpContext.Session["Role"].ToString()=="User")
            {
                return View(teams.Where(x=>x.UserId==id).ToList());
            }
            return View(teams.ToList());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(string SearchTeam)
        {
            var userId = Convert.ToString(_httpContext.Session["UserId"]);
            int id = Convert.ToInt32(userId);
            var teams = db.Teams.Where(x => x.Name == SearchTeam).Include(t => t.Event);
            if (_httpContext.Session["Role"].ToString() == "User")
            {
                return View(teams.Where(x => x.UserId == id).ToList());
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
            var userId = Convert.ToString(_httpContext.Session["UserId"]);
            int id = Convert.ToInt32(userId);
            if (_httpContext.Session["Role"].ToString() == "User")
            {
                ViewBag.User = id;
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.EventId = new SelectList(db.Events, "Id", "Subject");
            return View();
        }

        // POST: Teems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,NoOfPlayers,TeamRating,Description,EventId,UserId")] Team team)
        {
            if (_httpContext.Session["Role"].ToString() == "User")
            {
                var userId = Convert.ToString(_httpContext.Session["UserId"]);
                int user = Convert.ToInt32(userId);
                if (ModelState.IsValid)
                {
                    TeamRequest teamRequest = new TeamRequest();
                    teamRequest.Name = team.Name;
                    teamRequest.NoOfPlayers = team.NoOfPlayers;
                    teamRequest.Description = team.Description;
                    teamRequest.EventId = team.EventId;
                    teamRequest.UserId = team.UserId;

                    db.TeamRequests.Add(teamRequest);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.User = userId;
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
         
            if (_httpContext.Session["Role"].ToString() == "User")
            {
                var userId = Convert.ToString(_httpContext.Session["UserId"]);
                int user = Convert.ToInt32(userId);
                ViewBag.UserId = user;
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
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,NoOfPlayers,TeamRating,Description,EventId,UserId")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.EventId = new SelectList(db.Events, "Id", "Subject");
            return View(team);
        }
        
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
