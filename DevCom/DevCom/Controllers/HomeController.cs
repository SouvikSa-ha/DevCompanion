using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevCom.Models;

namespace DevCom.Controllers
{
    public class HomeController : Controller
    {
        DevCom_DBEntities db = new DevCom_DBEntities();
        public ActionResult Index()
        {
            ViewBag.Path = "Home";
            var userid = db.Texts.OrderByDescending(x => x.Id).First().Id + 1;
            Session["UidSS"] = userid;
            Session["GuestSS"] = 1;
            return View();
        }

        public ActionResult LoginReminder()
        {
            if(Session["GuestSS"] == null)
            {
                ViewBag.Path = "Home";
                var userid = db.Texts.OrderByDescending(x => x.Id).First().Id + 1;
                Session["UidSS"] = userid;
                Session["GuestSS"] = 1;
            }
            return View();
        }

        public ActionResult WelcomeUser()
        {
            var uid = Convert.ToInt32(Session["UidSS"].ToString());
            var name = db.Users.FirstOrDefault(x => x.Uid == uid).Username;
            Session["GuestSS"] = null;
            return View((object)name);
        }
    }
}