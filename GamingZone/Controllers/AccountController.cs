using GamingZone.Models;
using GamingZone.ViewModels;
using System.Web.Mvc;
using System.Linq;
using System.Net;
using System.Web;


namespace GamingZone.Controllers
{
    public class AccountController : Controller
    {
        GamingZoneEntities db = new GamingZoneEntities();
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login login)
        {
            if (ModelState.IsValid)
            {
              var user=  db.Users.Where(m => m.Email==login.UserName && m.Password==login.Password).FirstOrDefault();

           
                if (user!=null)
                {
                    if(user.Role=="Admin")
                    {
                        ViewData["user"] = user.Role;
                        Session["Role"]  = user.Role;
                    }
                    else
                    {
                        ViewData["user"] = user.Role;
                        Session["Role"] = user.Role;
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "User Name or Password are Incorred");
            }
            return View();
        }

        public ActionResult SignOut()
        {
            Session.RemoveAll();
            return RedirectToAction("Login", "Account");
        }


        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [Authorize]
        public ActionResult Register(Register register)
        {
            if (ModelState.IsValid)
            {
                bool isExisted = db.Users.Where(m => m.Email == register.UserName).Any();
                if (isExisted)
                {
                    ModelState.AddModelError("UserName", "User Name Already Exits");
                }
                else
                {
                 
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            return View();
        }

        //private ActionResult GetRolesForCurrentUser()
        //{
        //    if (Roles.IsUserInRole(WebSecurity.CurrentUserName, "Administrator"))
        //        ViewBag.RoleId = (int)Role.Administrator;
        //    else
        //        ViewBag.RoleId = (int)Role.NoRole;
        //    return View();
        //}







    }
}