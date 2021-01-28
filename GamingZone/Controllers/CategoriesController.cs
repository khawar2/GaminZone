//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using GamingWebsite.Models.Ecommerce;
//using GamingWebsite.Models.Ecommerce.Database_Model_Initialize;

//namespace GamingWebsite.Controllers
//{
//    [Authorize]
//    public class CategoriesController : Controller
//    {
//        private GamingContext db = new GamingContext();

//        // GET: Categories
//        public ActionResult Index()
//        {
//            return View(db.Categories.ToList());
//        }

//        [HttpPost, ValidateAntiForgeryToken]
//        public ActionResult Index(string SearchCategory)
//        {


//            return View(db.Categories.Where(x => x.CategoryName == SearchCategory).ToList());

//        }
//        // GET: Categories/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Category category = db.Categories.Find(id);
//            if (category == null)
//            {
//                return HttpNotFound();
//            }
//            return View(category);
//        }

//        // GET: Categories/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: Categories/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "CategoryID,CategoryName,Description")] Category category)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Categories.Add(category);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(category);
//        }

//        // GET: Categories/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Category category = db.Categories.Find(id);
//            if (category == null)
//            {
//                return HttpNotFound();
//            }
//            return View(category);
//        }

//        // POST: Categories/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "CategoryID,CategoryName,Description")] Category category)
//        {
//            if (ModelState.IsValid)
//            {      
//                db.Entry(category).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(category);
//        }

//        // GET: Categories/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Category category = db.Categories.Find(id);
//            if (category == null)
//            {
//                return HttpNotFound();
//            }
//            return View(category);
//        }

//        // POST: Categories/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            Category category = db.Categories.Find(id);
//            var subcate = db.subCategories.Where(c => c.CategoryID == category.CategoryID).ToList();
//            if (subcate != null)
//            {
//                TempData["Delete"] = "Subcategory to this category exsits!";
//                //TempData.Keep();
//            }
//            else
//            {
//        db.Categories.Remove(category);
//            db.SaveChanges();
//            }

    
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
