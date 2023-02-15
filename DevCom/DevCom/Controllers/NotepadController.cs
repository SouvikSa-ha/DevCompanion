using DevCom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevCom.Models.ViewModels;
using System.Threading.Tasks;

namespace DevCom.Controllers
{
    public class NotepadController : Controller
    {
        DevCom_DBEntities db = new DevCom_DBEntities();
        
        // GET: Notepad
        public ActionResult TempIndex()
        {
            Session["NoteIndex"] = "-1";
            return RedirectToAction("Index");
        }
        public ActionResult Index()
        {
            ViewBag.Path = "Home -> Notepad";
            var uid = Convert.ToInt32(Session["UidSS"]);
            NotepadVM myModel = new NotepadVM();
            myModel.Notepads = db.Notepads.Where(n=>n.Uid.Equals(uid)).ToList();



            //myModel.NoteContents = db.NoteContents.ToList();
            //myModel.Tags = db.Tags.ToList();

            List<IEnumerable<string>> temp = new List<IEnumerable<string>>();
            foreach (var item in myModel.Notepads)
            {
                IEnumerable<string> it = from nc in db.NoteContents where nc.Notepad_Id == item.Notepad_Id select nc.Content_Id;
                //string a =  it.ElementAt(0);
                temp.Add(it);
            }
            myModel.Content_ids = temp;
            /*
            myModel.Texts = from nc in db.Texts where content_ids.Contains(nc.Text_Id) select nc;
            myModel.Images = from nc in db.Images where content_ids.Contains(nc.Image_Id) select nc;
            myModel.Audios = from nc in db.Audios where content_ids.Contains(nc.Audio_Id) select nc;
            myModel.Videos = from nc in db.Videos where content_ids.Contains(nc.Video_Id) select nc;
            myModel.Files = from nc in db.Files where content_ids.Contains(nc.File_Id) select nc;
            myModel.Canvases = from nc in db.Canvases where content_ids.Contains(nc.Canvas_Id) select nc;*/

            return View(myModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public EmptyResult Create(Notepad _notepad)
        {
            Notepad notepad = new Notepad();
            notepad.Title = _notepad.Title;
            notepad.Creation_Date = DateTime.Now;
            notepad.Update_Date = DateTime.Now;
            notepad.Uid = Convert.ToInt32(Session["UidSS"]);
            //db.Configuration.ValidateOnSaveEnabled = false;
            db.Notepads.Add(notepad);
            db.SaveChanges();

            return null;
        }

        public EmptyResult Details(Notepad noteid)
        {
            /*Notepad notepad = db.Notepads.Where(n => n.Notepad_Id.Equals(122)).First();
            int idint = Convert.ToInt32(noteid.Notepad_Id.ToString());
            var id = myModel.Notepads.ElementAt(idint).Notepad_Id;
            myModel.content_ids = db.NoteContents.Where(
                    x => x.Notepad_Id == id
                ).Select(p => p.Content_Id).ToList();*/
            Session["NoteIndex"] = noteid.Notepad_Id.ToString();
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

    }
}