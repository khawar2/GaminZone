﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GamingWebsite.Models.Ecommerce;
using GamingWebsite.Models.Ecommerce.Database_Model_Initialize;

namespace GamingWebsite.Controllers
{
    [Authorize]
    public class SubCategoriesController : Controller
    {
        private GamingContext db = new GamingContext();

        // GET: SubCategories
        public ActionResult Index()
        {
            var subCategories = db.subCategories.Include(s => s.Category);
            return View(subCategories.ToList());
        }


        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(string SearchSubCategory)
        {


            return View(db.subCategories.Where(x => x.SubcategoryName == SearchSubCategory).ToList());

        }

        // GET: SubCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = db.subCategories.Find(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            return View(subCategory);
        }

        // GET: SubCategories/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: SubCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubcategoryID,SubcategoryName,Description,CategoryID")] SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                db.subCategories.Add(subCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", subCategory.CategoryID);
            return View(subCategory);
        }

        // GET: SubCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = db.subCategories.Find(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", subCategory.CategoryID);
            return View(subCategory);
        }

        // POST: SubCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubcategoryID,SubcategoryName,Description,CategoryID")] SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", subCategory.CategoryID);
            return View(subCategory);
        }

        // GET: SubCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = db.subCategories.Find(id);     
           
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            return View(subCategory);
        }

        // POST: SubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubCategory subCategory = db.subCategories.Find(id);
            //var subcate = db.Products.Where(c => c.SubcategoryID == subCategory.SubcategoryID).ToList();
            //if (subcate != null)
            //{
            //    TempData["DeleteUpdate"] = "Products to this Subcategory exsits!";
            //    TempData.Keep();
            //}
            //else
            //{
           db.subCategories.Remove(subCategory);
            db.SaveChanges();
            //}
           
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