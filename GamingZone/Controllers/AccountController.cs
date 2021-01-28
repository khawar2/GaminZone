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
              var user=  db.Users.Where(m => m.Email.Contains(login.UserName)).FirstOrDefault();

           
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
                    return RedirectToAction("Dashboard", "Home");
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


        //[HttpGet]
        //[Authorize]
        // public ActionResult Register()
        // {
        //     return GetRolesForCurrentUser();
        // }

        // private ActionResult GetRolesForCurrentUser()
        // {
        //     if (Roles.IsUserInRole(WebSecurity.CurrentUserName, "Administrator"))
        //         ViewBag.RoleId = (int)Role.Administrator;
        //     else
        //         ViewBag.RoleId = (int)Role.NoRole;
        //     return View();
        // }



        // [HttpPost,ValidateAntiForgeryToken]
        // [Authorize]
        // public ActionResult Register(Register register)
        // {
        //     GetRolesForCurrentUser();

        //     if (ModelState.IsValid)
        //     {

        //         bool isExisted = WebSecurity.UserExists(register.UserName);
        //         if (isExisted)
        //         {
        //             ModelState.AddModelError("UserName", "User Name Already Exits");
        //         }
        //         else
        //         {
        //             WebSecurity.CreateUserAndAccount(register.UserName, register.Password, new
        //             { FirstName = register.firstname,LastName= register.lastname, Email = register.email,Phone= register.phone,PhoneCode= register.phonecode, Age = register.Age });
        //             Roles.AddUserToRole(register.UserName, register.Role);

        //             return RedirectToAction("Index", "Dashboard");


        //         }

        //     }


        //     return View();
        // }




    }
}