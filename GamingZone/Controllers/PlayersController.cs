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
    public class PlayersController : Controller
    {
        private GamingZoneEntities db = new GamingZoneEntities();

        // GET: Players
        public ActionResult Index()
        {
            var players = db.Players.Include(p => p.Team).Include(p => p.User);
            return View(players.ToList());
        }

        // GET: Players/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // GET: Players/Create
        public ActionResult Create()
        {
            ViewBag.TeamId = new SelectList(db.Teams, "Id", "Name");
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Age,ImagePath,TeamId")] Player player, HttpPostedFileBase ImagePath)
        {
            if (ModelState.IsValid)
            {
                var countPlayer = db.Teams.Where(x => x.Id == player.Id).Include(x => x.Players).FirstOrDefault();

                if (countPlayer.Players != null)
                {
                    if (countPlayer.Players.Count >= countPlayer.NoOfPlayers)
                    {

                        ViewBag.TeamId = new SelectList(db.Teams, "Id", "Name", player.TeamId);
                        ViewBag.error = "Team is full select another";
                        return View(player);
                    }
                }

                if (ImagePath != null)
                {
                    ImagePath.SaveAs(HttpContext.Server.MapPath("~/Images/")
                                                          + ImagePath.FileName);
                    player.ImagePath = ImagePath.FileName;
                }

                db.Players.Add(player);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TeamId = new SelectList(db.Teams, "Id", "Name", player.TeamId);
            return View(player);
        }

        // GET: Players/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            ViewBag.TeamId = new SelectList(db.Teams, "Id", "Name", player.TeamId);
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Age,ImagePath,TeamId,UserId")] Player player, HttpPostedFileBase ImagePath)
        {
            var countPlayer = db.Teams.Where(x => x.Id == player.Id).Include(x => x.Players).FirstOrDefault();
            if(countPlayer.Players!=null)
            {
                if (countPlayer.Players.Count >= countPlayer.NoOfPlayers)
                {

                    ViewBag.TeamId = new SelectList(db.Teams, "Id", "Name", player.TeamId);
                    ViewBag.error = "Team is full select another";
                    return View(player);
                }
            }
         
            if (ImagePath != null)
            {
                ImagePath.SaveAs(HttpContext.Server.MapPath("~/Images/")
                                                      + ImagePath.FileName);
                player.ImagePath = ImagePath.FileName;
            }
            if (ModelState.IsValid)
            {
                db.Entry(player).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TeamId = new SelectList(db.Teams, "Id", "Name", player.TeamId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", player.UserId);
            return View(player);
        }
        
        [HttpGet]
        // POST: Players/Delete/5
        public ActionResult Delete(int id)
        {
            Player player = db.Players.Find(id);
            db.Players.Remove(player);
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
