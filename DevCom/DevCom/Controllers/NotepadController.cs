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
        

        public ActionResult Index()
        {
            NotepadVM myModel = new NotepadVM();
            if (Session["UidSS"] != null)
            {
                ViewBag.Path = "Home -> Notepad";
                var uid = Convert.ToInt32(Session["UidSS"]);
                
                myModel.Notepads = db.Notepads.Where(n=>n.Uid.Equals(uid)).ToList();

            

                List<IEnumerable<string>> temp = new List<IEnumerable<string>>();
                foreach (var item in myModel.Notepads)
                {
                    IEnumerable<string> it = from nc in db.NoteContents where nc.Notepad_Id == item.Notepad_Id select nc.Content_Id;
                    //string a =  it.ElementAt(0);
                    temp.Add(it);
                }
                myModel.Content_ids = temp;

                List<string> _temp= new List<string>();

                foreach (var item in myModel.Notepads)
                {
                    IEnumerable<string> it = from nc in db.Tags where nc.Tag_Id == item.Tag_Id select nc.Tag_Name;
                    if (it != null && it.Any())
                        _temp.Add(it.ElementAt(0));
                }
                myModel.Tags = _temp;
            }
            else
            {
                var userid = db.Texts.OrderByDescending(x => x.Id).First().Id + 1;
                myModel.Notepads = new List<Notepad>();
                Session["UidSS"] = userid;
                Session["GuestSS"] = 1;
                ViewBag.Path = "Home -> Visual Board";
            }


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
            Session["NoteIndex"] = noteid.Notepad_Id.ToString();
            return null;
        }

        [HttpPost]
        [ValidateInput(false)]
        public EmptyResult Update(Text _text)
        {

            var text = db.Texts.FirstOrDefault(x => x.Id == _text.Id);
            if(text != null)
            {
                if (_text.Text1 != null)
                    text.Text1 = _text.Text1.Trim();
                else
                    text.Text1 = "add text";
                db.SaveChanges();
            }

            return null;
        }

        [HttpPost]
        public EmptyResult AddText(Notepad model)
        {
            var _text = db.Texts.OrderByDescending(x => x.Id).First().Id + 1;
            Text text = new Text();
            text.Text_Id = "t_" + _text.ToString();
            text.Text1 = "add text";
            
            
            NoteContent nc = new NoteContent();
            nc.Content_Id = "t_" + _text.ToString(); ;
            nc.Notepad_Id = model.Notepad_Id;

            db.NoteContents.Add(nc);
            db.SaveChanges();

            db.Texts.Add(text);
            db.SaveChanges();
            return null;
        }

    }
}