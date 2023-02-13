using DevCom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevCom.Models.ViewModels;

namespace DevCom.Controllers
{
    public class NotepadController : Controller
    {
        DevCom_DBEntities db = new DevCom_DBEntities();
        // GET: Notepad
        public ActionResult Index()
        {
            ViewBag.Path = "Home -> Notepad";
            NotepadVM myModel = new NotepadVM();
            myModel.Notepads = db.Notepads.ToList();
            //myModel.NoteContents = db.NoteContents.ToList();
            //myModel.Tags = db.Tags.ToList();
            myModel.Texts = db.Texts.ToList();
            myModel.Notepad = new Notepad();
            //myModel.NoteContent = new NoteContent();
            myModel.Text = new Text();
            //myModel.Tag = new Tag();
            return View(myModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(String title)
        {
            Notepad notepad = new Notepad();
            notepad.Title = title;
            notepad.Creation_Date = DateTime.Now;
            notepad.Update_Date = DateTime.Now;
            notepad.Uid = (int)Session["UidSS"];
            //db.Configuration.ValidateOnSaveEnabled = false;
            db.Notepads.Add(notepad);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public EmptyResult Details(/*string noteid*/)
        {
            //var item = db.Notepads.Where(m => m.Notepad_Id == noteid).FirstOrDefault();
            return null;
        }

        /*[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(NotepadViewModel notepadViewModel)
        {

            if (ModelState.IsValid)
            {
                Notepad notepad = new Notepad();
                notepad.Title = notepadViewModel.Notepad.Title;
                notepad.Creation_Date = DateTime.Now;
                notepad.Update_Date = DateTime.Now;
                notepad.Uid = 1;
                db.Notepads.Add(notepad);
                int a = db.SaveChanges();
                TempData["CreateMessage"] = (a > 0) ? 
                    "<script>alert('Created!')</script>":
                    "<script>alert('Error!')</script>";
            }
            TempData["CreateMessage"] =
                    "<script>alert('Here!')</script>";
            return RedirectToAction("Index");
        } */


        public ActionResult AutoCreate()
        {
            
            ViewBag.UserList = db.Users.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AutoCreate(Notepad notepad)
        {
            notepad.Creation_Date = DateTime.Now;
            notepad.Update_Date = DateTime.Now;
            db.Notepads.Add(notepad);
            db.SaveChanges();
            return View();
        }

    }
}