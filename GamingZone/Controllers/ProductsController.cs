using GamingZone.Infrastructure;
using GamingZone.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GamingZone.Controllers
{
    [CustomAuthorize("Admin")]
    public class ProductsController : Controller
    {
        private GamingZoneEntities db = new GamingZoneEntities();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.SubCategory);
            return View(products.ToList());
        }


        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(string SearchCategory, string option)
        {
            if (option == "Name")
            {
                return View(db.Products.Where(x => x.ProductName == SearchCategory || SearchCategory == null).ToList());
            }
            else if (option == "Price")
            {
                double price = Double.Parse(SearchCategory);
                return View(db.Products.Where(x => x.UnitPrice == price || SearchCategory == null).ToList());
            }
            else if (option == "Category")
            {
                return View(db.Products.Where(x => x.SubCategory.SubcategoryName == SearchCategory || SearchCategory == null).ToList());
            }

            return View(db.Products.Where(x => x.ProductName == SearchCategory).ToList());

        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
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

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.SubcategoryID = new SelectList(db.SubCategories, "SubcategoryID", "SubcategoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,Description,ImagePath,UnitPrice,Seller,Color,Date,SubcategoryID")] Product product, HttpPostedFileBase ImagePath)
        {
            if (ModelState.IsValid)
            {
                if (ImagePath != null)
                {
                    ImagePath.SaveAs(HttpContext.Server.MapPath("~/Images/")
                                                          + ImagePath.FileName);
                    product.ImagePath = ImagePath.FileName;
                }

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SubcategoryID = new SelectList(db.SubCategories, "SubcategoryID", "SubcategoryName", product.SubcategoryID);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.SubcategoryID = new SelectList(db.SubCategories, "SubcategoryID", "SubcategoryName", product.SubcategoryID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,Description,ImagePath,UnitPrice,Seller,Color,Date,SubcategoryID")] Product product,HttpPostedFileBase ImagePath)
        {
            if (ModelState.IsValid)
            {
                if (ImagePath != null)
                {
                    ImagePath.SaveAs(HttpContext.Server.MapPath("~/Images/") + ImagePath.FileName);
                    product.ImagePath = ImagePath.FileName;

                    db.Entry(product).State = EntityState.Added;
                }
                else
                {
                    db.Entry(product).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SubcategoryID = new SelectList(db.SubCategories, "SubcategoryID", "SubcategoryName", product.SubcategoryID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        public ActionResult Store()
        {
            var products = db.Products.Include(p => p.SubCategory);
            return View(products.ToList());
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
