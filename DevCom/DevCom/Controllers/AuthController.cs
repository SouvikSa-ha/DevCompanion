using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

                    string generatedSalt = GenerateSalt();
                    byte[] hashedPassword = GetHash(user.Password, generatedSalt);
                    string hashedString64Password = Convert.ToBase64String(hashedPassword);

                    user.Password = hashedString64Password;
                    user.Salt = generatedSalt;

                    user.Email = _user.Email;
                    user.EmailConfirmed = false;
                    user.CreationDate = DateTime.Now;
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
                var user = db.Users.Where(u => u.Email.Equals(_user.Email)).FirstOrDefault();
                bool matched = ComnpareHashedPasswords(_user.Password, user.Password, user.Salt);
                
                if(matched)
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

        public ActionResult ShowProfile()
        {
            return View();
        }

        public ActionResult EditProfile(HttpPostedFileBase file)
        {
            if(file!=null && file.ContentLength > 0)
            {
                string filename = Path.GetFileName(file.FileName);
                //string filepath = Path.Combine()
            }
            return View();
        }

        private string GenerateSalt()
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            return Convert.ToBase64String(salt);
        }

        private byte[] GetHash(string plainPassword, string salt)
        {
            byte[] byteArray = Encoding.Unicode.GetBytes(String.Concat(salt, plainPassword));
            SHA256Managed SHA256 = new SHA256Managed();
            byte[] hashedBytes = SHA256.ComputeHash(byteArray);
            return hashedBytes;
        }

        private bool ComnpareHashedPasswords(string userInputPassword, string existingHashedPassword, string salt)
        {
            string userInputHashedPassword = Convert.ToBase64String(GetHash(userInputPassword, salt));
            return existingHashedPassword == userInputHashedPassword;
        }


    }
}