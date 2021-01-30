using GamingZone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GamingZone.Controllers
{
    public class TeamsRequestController : Controller
    {
        private GamingZoneEntities db = new GamingZoneEntities();
        // GET: TeamsRequest
        public ActionResult Index()
        {
            var list = db.TeamRequests.Where(m => m.isApproved != true).ToList();
            return View(list);
        }
        public ActionResult Approve(int id)
        {
           TeamRequest list= db.TeamRequests.Where(m => m.isApproved != true && m.Id==id).FirstOrDefault();

            Team teams = new Team();
            teams.Name = list.Name;
            teams.NoOfPlayers = list.NoOfPlayers;
            teams.Description = list.Description;
            teams.EventId = list.EventId;
            teams.UserId = list.UserId;

            db.TeamRequests.Remove(list);
            db.Teams.Add(teams);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
