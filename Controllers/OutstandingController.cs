using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
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

        public ActionResult ApprovalOutstanding(int id)
        {
            AceDBEntities entity = new AceDBEntities();
            Outstanding outs = entity.Outstandings.Find(id);
            if (outs == null)
                return View("NotFound");
            else
                return View(outs);
        }

        public ActionResult ApprovalOutstanding(Outstanding outstanding)
        {
            using (AceDBEntities entity = new AceDBEntities())
            {
                entity.Entry(outstanding).State = EntityState.Modified;
                entity.SaveChanges();
            }
            ModelState.Clear();
            //ViewBag.SuccessMessage = "Save Changes Successful. ";
            return RedirectToAction("Index", "Outstanding");
        }

        //// GET: Oustanding/Details/5
        //public ActionResult Details(int id)
        //{
        //    using (AceDBEntities entity = new AceDBEntities())
        //    {
        //        return View(entity.Outstandings.Where(x => x.O_ID == id).FirstOrDefault());
        //    }
        //}

        //// GET: Oustanding/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Oustanding/Create
        //[HttpPost]
        //public ActionResult Create(Outstanding outstanding)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here
        //        using (AceDBEntities entity = new AceDBEntities())
        //        {
        //            entity.Outstandings.Add(outstanding);
        //            entity.SaveChanges();
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Oustanding/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    using (AceDBEntities entity = new AceDBEntities())
        //    {
        //        return View(entity.Outstandings.Where(x => x.O_ID == id).FirstOrDefault());
        //    }
        //}

        //// POST: Oustanding/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, Outstanding outstanding)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here
        //        using (AceDBEntities entity = new AceDBEntities())
        //        {
        //            entity.Entry(outstanding).State = EntityState.Modified;
        //            entity.SaveChanges();
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Oustanding/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    using (AceDBEntities entity = new AceDBEntities())
        //    {
        //        return View(entity.Outstandings.Where(x => x.O_ID == id).FirstOrDefault());
        //    }
        //}

        //// POST: Oustanding/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, Outstanding outstanding)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here
        //        using (AceDBEntities entity = new AceDBEntities())
        //        {
        //            outstanding = entity.Outstandings.Where(x => x.O_ID == id).FirstOrDefault();
        //            entity.Outstandings.Remove(outstanding);
        //            entity.SaveChanges();
        //        }
        //            return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        public ActionResult SendOutstandingEmail(int id)
        {
            AceDBEntities db = new AceDBEntities();

            List<Outstanding> outstandings = db.Outstandings.Where(a => a.O_ID.Equals(id)).ToList();
            List<Parent> parents = db.Parents.ToList();

            var multipleclass = from o in outstandings
                                join p in parents on o.O_pID equals p.parents_ic
                                select new CascadingClass { receiver = p, message = o };

            return View(multipleclass);
        }

        [HttpPost]
        public ActionResult SendOutstandingEmail(string receiver, string subject, string message)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("nurulasyikin.s390@gmail.com", "Ace Education");
                    var receiverEmail = new MailAddress(receiver, "Receiver");
                    var password = "Nurul990626_";
                    var sub = subject;
                    var body = message;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = "Your Latest Outstanding Balance " + subject,
                        Body = "Total Outstanding Balance : RM" + body
                    })
                    {
                        smtp.Send(mess);
                    }

                    ViewBag.msg = "Email sent successfully";

                    return RedirectToAction("Index", "Outstanding");

                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Send Email failed!!";
            }
            return RedirectToAction("Index", "Outstanding");
        }
    }
}
