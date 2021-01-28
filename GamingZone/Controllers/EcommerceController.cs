using GamingWebsite.Models;
using GamingWebsite.Models.Ecommerce;
using GamingWebsite.Models.Ecommerce.Database_Model_Initialize;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using GamingWebsite.Models.Account;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebMatrix.WebData;
using System.Web.Security;
using static GamingWebsite.Models.General.SystemEnums;

namespace GamingWebsite.Controllers
{
    public class EcommerceController : Controller
    {
        private GamingContext db = new GamingContext();
        private Models.CartEntities DB = new Models.CartEntities();
  

        // GET: Ecommerce
        public ActionResult Index(string SubcategoryName)
        {

            var viewModel = new MyViewModel();

            if (TempData["cart"] != null)
            {
                float x = 0;
                List<Models.Cart> li2 = TempData["cart"] as List<Models.Cart>;

                foreach(var item in li2)
                {
                    float price = float.Parse(item.TotalPrice);
                    x += price;
                }
                TempData["Total"] = x;
            }

            TempData.Keep();



            if (SubcategoryName == null) { 
            ViewBag.ListA = db.Products.ToList();

            }
            else
            {
                ViewBag.ListA = db.Products.Where(x => x.Subcategory.SubcategoryName == SubcategoryName ).ToList(); 
            }
            ViewBag.ListB = db.subCategories.ToList();
           
            viewModel.ListA = ViewBag.ListA;
            viewModel.ListB = ViewBag.ListB;

         

            return View(viewModel);

        }
   

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(string SearchCategory, string option)
        {
            
            var viewModel = new MyViewModel();
            ViewBag.ListB = db.subCategories.ToList();
            viewModel.ListB = ViewBag.ListB;
            TempData.Keep();


            if (SearchCategory == null)
            {
            ViewBag.ListA = db.Products.ToList();

            viewModel.ListA = ViewBag.ListA;
                return View(viewModel);
            }
            else if(option == null)
            {

                ViewBag.ListA = db.Products.ToList();
            
                viewModel.ListA = ViewBag.ListA;
                return View(viewModel);
            }else if(option == "All")
            {
                ViewBag.ListA = db.Products.ToList();
            }

            else if (option == "Name")
            {

                ViewBag.ListA = db.Products.Where(x => x.ProductName == SearchCategory || SearchCategory == null).ToList();

                viewModel.ListA = ViewBag.ListA;
                return View(viewModel);
            }
            else if (option == "Price")
            {
                double price = Double.Parse(SearchCategory);

                ViewBag.ListA = db.Products.Where(x => x.UnitPrice == price || SearchCategory == null).ToList();

                viewModel.ListA = ViewBag.ListA;

                return View(viewModel);
            }
            else if (option == "Category")
            {

                ViewBag.ListA = db.Products.Where(x => x.Subcategory.SubcategoryName == SearchCategory || SearchCategory == null).ToList();

                viewModel.ListA = ViewBag.ListA;

                return View(viewModel);
            }

            ViewBag.ListA = db.Products.ToList();

            viewModel.ListA = ViewBag.ListA;

            return View(viewModel);

        }

        // GET: Products/Details/5

        public ActionResult Register()
        {
            return GetRolesForCurrentUser();
     
        }
        private ActionResult GetRolesForCurrentUser()
        {
            if (Roles.IsUserInRole(WebSecurity.CurrentUserName, "Administrator"))
                ViewBag.RoleId = (int)Role.Administrator;
            else
                ViewBag.RoleId = (int)Role.NoRole;
            return View();
        }

        List<Models.Account.Register> register = new List<Models.Account.Register>();
        [HttpPost]
        public ActionResult Register(Register register)
        {
            GetRolesForCurrentUser();



            return View();
        }


        public ActionResult Details(  int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        List<Models.Cart> li = new List<Models.Cart>();
        [HttpPost]
        public ActionResult Details( string qty,int id)
        {
            string username = WebSecurity.CurrentUserName.ToString();
           var  product = db.Products.Where( c => c.ProductID == id).SingleOrDefault();
            int quant = int.Parse(qty);
            

            Models.Cart cart = new Models.Cart();
    
            float unit = (float)product.UnitPrice;
           

            var total = unit * quant;

            cart.ProductId = id;
            cart.fullName = username;
            cart.DateCreated = DateTime.Now;
            cart.Quantity = quant;
            cart.TotalPrice = total.ToString();
            cart.productName = product.ProductName;
            cart.Price = product.UnitPrice;
           // DB.Carts.Add(cart);


            if (TempData["cart"] == null)
            {
                li.Add(cart);
                TempData["cart"] = li;
            }
            else
            {

                List<Models.Cart> li2 = TempData["cart"] as List<Models.Cart>;
                int flag = 0;
                foreach(var item in li2)
                {
                    if(item.ProductId == cart.ProductId)
                    {
                        item.Quantity += cart.Quantity;

                        int tprice = int.Parse(item.TotalPrice);
                        int cartTprice = int.Parse(cart.TotalPrice);
                        int totalval = tprice + cartTprice;

                        string t = totalval.ToString();
                        item.TotalPrice= t ;
                        flag = 1;
                    }

                }
                if(flag == 0)
                {
                    li2.Add(cart);
                }
                //li2.Add(cart);
                TempData["cart"] = li2;
            }
           
           
            TempData.Keep();

            return RedirectToAction("Index");
        }



        public ActionResult remove(int ? id)
        {
            List<Models.Cart> li2 = TempData["cart"] as List<Models.Cart>;

            Models.Cart cart = li2.Where(c => c.ProductId == id).SingleOrDefault();

            li2.Remove(cart);
            int h = 0;
            string t = null;


            foreach (var item in li2)
            {       int tprice = int.Parse(item.TotalPrice);
                        int totalval = h + tprice;

              t  = totalval.ToString();

                 t = item.TotalPrice ;
            }
            TempData["Total"] = t;
            return RedirectToAction("checkout");
        }


        [HttpGet]
        public ActionResult checkout()
        {
            TempData.Keep();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult check(string fullName,string Address,string Phone1,string Phone2,  Order order)
        {
            List<Models.Cart> li = TempData["cart"] as List<Models.Cart>;
            string username = WebSecurity.CurrentUserName.ToString();

              order.fullName = fullName;
                order.Address = Address;
                order.Phone1 = Phone1;
                order.Phone2 = Phone2;
         
                if (ModelState.IsValid)
            {  
                DB.Orders.Add(order);
                DB.SaveChanges();
          } 

            foreach (var item in li)
            { 
                Models.Cart cart = new Models.Cart();
  

             cart.ProductId = item.ProductId;
            cart.fullName = username;
            cart.DateCreated = DateTime.Now;
            cart.Quantity = item.Quantity;
            cart.TotalPrice = item.TotalPrice;
                cart.Price = item.Price;
                cart.productName = item.productName;
                cart.orderId = order.orderId;
                DB.Carts.Add(cart);
                DB.SaveChanges(); 
                
            
             
                

            }
         

    
            TempData.Remove("Total");
            TempData.Remove("cart");

            TempData["msg"] = "Transaction Complete...";
            TempData.Keep();

            return RedirectToAction("Index");
        }

         public ActionResult Contact()
        {
            return View();
        }
       
        public ActionResult Tournaments()
        {

            return View();
        }


        public ActionResult Work()
        {

            return View();
        }
    }
}