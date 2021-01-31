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
    [CustomAuthorize("Admin", "User")]
    public class PlayersController : Controller
    {
        private GamingZoneEntities db = new GamingZoneEntities();


        // GET: Players
        public ActionResult Index()
        {
            var players = db.Players.Include(p => p.Team);
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
            if (!IsAdmin())
            {
                int userid = GetUserId();
                ViewBag.TeamId = new SelectList(db.Teams.Where(c => c.UserId == userid), "Id", "Name");
            }
            else
            {
                ViewBag.TeamId = new SelectList(db.Teams, "Id", "Name");
            }
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,Name,Age,ImagePath,TeamId")] Player player, HttpPostedFileBase ImagePath)
        {
            int userid = GetUserId();
            if (ModelState.IsValid)
            {
                var countPlayer = db.Teams.Where(x => x.Id == player.Id).Include(x => x.Players).FirstOrDefault();

                if (countPlayer != null)
                {
                    if (countPlayer.Players.Count >= countPlayer.NoOfPlayers)
                    {

                        if (!IsAdmin())
                        {
                            ViewBag.TeamId = new SelectList(db.Teams.Where(c => c.UserId == userid), "Id", "Name");
                        }
                        else
                        {
                            ViewBag.TeamId = new SelectList(db.Teams, "Id", "Name", player.TeamId);
                        }
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

            if (!IsAdmin())
            {
                ViewBag.TeamId = new SelectList(db.Teams.Where(c => c.UserId == userid), "Id", "Name");
            }
            else
            {
                ViewBag.TeamId = new SelectList(db.Teams, "Id", "Name", player.TeamId);
            }
            return View(player);
        }

        // GET: Players/Edit/5
        public ActionResult Edit(int? id)
        {
            int userid = GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            if (!IsAdmin())
            {
                ViewBag.TeamId = new SelectList(db.Teams.Where(m => m.UserId == userid), "Id", "Name", player.TeamId);
                return View(player);
            }
            ViewBag.TeamId = new SelectList(db.Teams, "Id", "Name", player.TeamId);
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Name,Age,ImagePath,TeamId,UserId")] Player player, HttpPostedFileBase ImagePath)
        {
            int userid = GetUserId();
            var countPlayer = db.Teams.Where(x => x.Id == player.Id).Include(x => x.Players).FirstOrDefault();
            if (countPlayer != null)
            {
                if (countPlayer.Players.Count >= countPlayer.NoOfPlayers)
                {
                    if (!IsAdmin())
                        ViewBag.TeamId = new SelectList(db.Teams.Where(m => m.UserId == userid), "Id", "Name", player.TeamId);
                    else
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

                if (!IsAdmin())
                    return RedirectToAction("MyTeamPlayers");
                else
                    return RedirectToAction("Index");

            }
            if (!IsAdmin())
                ViewBag.TeamId = new SelectList(db.Teams.Where(m => m.UserId == userid), "Id", "Name", player.TeamId);
            else
                ViewBag.TeamId = new SelectList(db.Teams, "Id", "Name", player.TeamId);
            return View(player);
        }


        [HttpGet]
        public ActionResult MyTeamPlayers(int teamID = 0)
        {
            if (teamID != 0)
            {
                var myplayers = (from team in db.Teams
                                 join player in db.Players on team.Id equals player.TeamId
                                 where team.Id == teamID
                                 select new TeamPlayerVM
                                 {
                                     Id = player.Id,
                                     Name = player.Name,
                                     Age = player.Age,
                                     Ratings = player.Ratings,
                                     TeamName = team.Name,
                                     ImagePath = player.ImagePath
                                 }
                        );
                return View(myplayers);
            }
            else
            {
                int userid = GetUserId();
                var myplayers = (from team in db.Teams
                                 join player in db.Players on team.Id equals player.TeamId
                                 where team.UserId == userid
                                 select new TeamPlayerVM
                                 {
                                     Id = player.Id,
                                     Name = player.Name,
                                     Age = player.Age,
                                     Ratings = player.Ratings,
                                     TeamName = team.Name,
                                     ImagePath= player.ImagePath
                                 }
                        );
                return View(myplayers);
            }

        }
        [HttpGet]
        public ActionResult ViewOtherPlayers(int teamID = 0)
        {
            int userid = GetUserId();
            var myplayers = (from team in db.Teams
                             join player in db.Players on team.Id equals player.TeamId
                             where team.UserId != userid
                             select new TeamPlayerVM
                             {
                                 Id = player.Id,
                                 Name = player.Name,
                                 Age = player.Age,
                                 Ratings = player.Ratings,
                                 TeamName = team.Name,
                                 ImagePath= player.ImagePath
                             }
                    );
            return View(myplayers);

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

        [HttpGet]
        // POST: Players/Delete/5
        public ActionResult Delete(int id)
        {
            Player player = db.Players.Find(id);
            db.Players.Remove(player);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Players/Details/5
        public ActionResult Profle(int? id)
        {
            int userid= GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Where(x => x.Id == id).Include(Xm => Xm.Ratings).FirstOrDefault();
            if (player == null)
            {
                return HttpNotFound();
            }
            ViewBag.user = userid;
            ProfileVM profile = new ProfileVM();
            profile.Players = player;
            profile.Ratings = player.Ratings.Where(x=>x.UserId==userid).FirstOrDefault();
            return View(profile);
        }
        [HttpPost]
        public ActionResult Rating([Bind(Include = "Id,Rating1,PlayerId,UserId")] Rating player)
        {
            db.Ratings.Add(player);
            db.SaveChanges();
            return RedirectToAction("ViewOtherPlayers");
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
