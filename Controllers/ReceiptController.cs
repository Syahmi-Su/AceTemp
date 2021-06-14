using AceTC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AceTC.ViewModel;

namespace AceTC.Controllers
{
    public class ReceiptController : Controller
    {
        // GET: Receipt
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Slip(int id)
        {
            AceDBEntities entity = new AceDBEntities();
            List<Payment> payment = entity.Payments.Where(a => a.confirmation_id.Equals(id)).ToList();
            List<Status> status = entity.Status.ToList();
            List<Package> package = entity.Packages.ToList();
            List<Student> student = entity.Students.ToList();
            List<Parent> parent = entity.Parents.ToList();
            var multipletable = from a in payment
                                join b in status on a.status_id equals b.status_id
                                join s in student on a.student_ic equals s.student_ic
                                join par in parent on a.parent_ic equals par.parents_ic
                                select new MultipleClass { paymentdetails = a, statusdetails = b, parentdetails = par, studentdetails = s };
            return View(multipletable);
        }
    }
}