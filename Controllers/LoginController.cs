using AceTC.Models;
using AceTC.Services.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AceTC.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(UserModel userModel)
        {
            // return "Results: Username = " + userModel.Username + " PW = " + userModel.Password;
            SecurityService securityService = new SecurityService();
            Boolean success = securityService.Authenticate(userModel);

            if (userModel.Username == "admin")
            {
                if (success)
                {
                    Session["admin_id"] = userModel.Username;
                    return RedirectToAction("AdminDashboard", "Admin");

                }
                else
                {
                    userModel.LoginErrorMsg = "Invalid Username or Password";
                    return View("Index", userModel);
                }
            }
            else
            {
                if (success)
                {
                    Session["parents_ic"] = userModel.Username;
                    return RedirectToAction("ViewChildren", "Parent");

                }
                else
                {
                    userModel.LoginErrorMsg = "Invalid Username or Password";
                    return View("Index", userModel);
                }
            }

        }

        public ActionResult Logout(UserModel userModel)
        {
            if (userModel.Username == "admin")
            {
                string username = (string)Session["admin_id"];
            }
            else
            {
                string username = (string)Session["parents_ic"];
            }
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }


    }
}