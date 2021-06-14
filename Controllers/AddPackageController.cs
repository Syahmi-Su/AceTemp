using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AceTC.Models;

namespace AceTC.Controllers
{
    public class AddPackageController : Controller
    {
        AceDBEntities db = new AceDBEntities();
        // GET: AddPackage
        public ActionResult AddPackage()
        {
            PackageClass addpack = new PackageClass();
            var Id = db.Packages.OrderByDescending(c => c.package_id).FirstOrDefault();
            if(Id == null)
            {
                addpack.package_id = 1;
            }
            else
            {
                addpack.package_id = Id.package_id + 1;
            }
            return View(addpack);
        }

        [HttpPost]
        public ActionResult AddPackage([Bind(Include = "package_id,package_desc,package_category,package_price")] Package package)
        {
            using (AceDBEntities db = new AceDBEntities())
            {
                db.Packages.Add(package);
                db.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Add Package Successfully. ";
            return RedirectToAction("PackageList", "Admin");
        }
    }
}