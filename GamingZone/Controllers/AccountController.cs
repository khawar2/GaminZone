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
            if(Session["Role"]!=null)
            {
                return RedirectToAction("Index", "Home");
            }
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
        public ActionResult Register(Register register)
        {
            if (ModelState.IsValid)
            {
                bool isExisted = db.Users.Where(m => m.Email == register.email).Any();
                if (isExisted)
                {
                    ModelState.AddModelError("UserName", "User Name Already Exits");
                }
                else
                {
                    User user = new User();
                    user.Email = register.email;
                    user.Password = register.Password;
                    user.FirstName = register.firstname;
                    user.LastName = register.lastname;
                    user.Role = register.Role;
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Login", "Account");
                }
            }
            return View();
        }
    }
}