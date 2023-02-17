using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevCom.Models;
using DevCom.Models.ViewModels;

namespace DevCom.Controllers
{
    public class AuthController : Controller
    {
        DevCom_DBEntities db = new DevCom_DBEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegistrationVM _user)
        {


            if (ModelState.IsValid)
            {
                if (_user.Password != _user.ConfirmPassword)
                {
                    ViewBag.ErrorMsg = "Password doesn't match";
                }
                else if (db.Users.Any(u => u.Email == _user.Email))
                {
                    ViewBag.ErrorMsg = "This account already exists.";
                }
                else if (db.Users.Any(u => u.Username == _user.Username))
                {
                    ViewBag.ErrorMsg = "This username is already taken.";
                }
                else
                {
                    User user = new User();
                    user.Username = _user.Username;
                    user.Password = _user.Password;
                    user.Email = _user.Email;
                    user.EmailConfirmed = false;
                    db.Users.Add(user);
                    db.SaveChanges();
                    Session["UidSS"] = user.Uid.ToString();
                    Session["UsernameSS"] = user.Username.ToString();
                    return RedirectToAction("WelcomeUser", "Home");
                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM _user)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Where(u => u.Email.Equals(_user.Email) && u.Password.Equals(_user.Password)).FirstOrDefault();
                if(user != null)
                {
                    Session["UidSS"] = user.Uid.ToString();
                    Session["UsernameSS"] = user.Username.ToString();
                    return RedirectToAction("WelcomeUser", "Home");
                }
                else
                {
                    ViewBag.ErrorMsg = "Wrong username or password";
                }
            }
            return View();
        }






    }
}