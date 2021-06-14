using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AceTC.Models;

namespace AceTC.Controllers
{
    public class OutstandingController : Controller
    {
        // GET: Oustanding/Index
        public ActionResult Index()
        {
            using (AceDBEntities entity = new AceDBEntities())
            {
                return View(entity.Outstandings.ToList());
            }

        }

        // GET: Oustanding/Details/5
        public ActionResult Details(int id)
        {
            using (AceDBEntities entity = new AceDBEntities())
            {
                return View(entity.Outstandings.Where(x => x.O_ID == id).FirstOrDefault());
            }
        }

        // GET: Oustanding/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Oustanding/Create
        [HttpPost]
        public ActionResult Create(Outstanding outstanding)
        {
            try
            {
                // TODO: Add insert logic here
                using (AceDBEntities entity = new AceDBEntities())
                {
                    entity.Outstandings.Add(outstanding);
                    entity.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Oustanding/Edit/5
        public ActionResult Edit(int id)
        {
            using (AceDBEntities entity = new AceDBEntities())
            {
                return View(entity.Outstandings.Where(x => x.O_ID == id).FirstOrDefault());
            }
        }

        // POST: Oustanding/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Outstanding outstanding)
        {
            try
            {
                // TODO: Add update logic here
                using (AceDBEntities entity = new AceDBEntities())
                {
                    entity.Entry(outstanding).State = EntityState.Modified;
                    entity.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Oustanding/Delete/5
        public ActionResult Delete(int id)
        {
            using (AceDBEntities entity = new AceDBEntities())
            {
                return View(entity.Outstandings.Where(x => x.O_ID == id).FirstOrDefault());
            }
        }

        // POST: Oustanding/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Outstanding outstanding)
        {
            try
            {
                // TODO: Add delete logic here
                using (AceDBEntities entity = new AceDBEntities())
                {
                    outstanding = entity.Outstandings.Where(x => x.O_ID == id).FirstOrDefault();
                    entity.Outstandings.Remove(outstanding);
                    entity.SaveChanges();
                }
                    return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
