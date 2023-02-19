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
                    else
                        _temp.Add("");
                }
                myModel.Tags = _temp;
            }
            else
            {
                var userid = db.Texts.OrderByDescending(x => x.Id).First().Id + 1;
                myModel.Notepads = new List<Notepad>();
                Session["UidSS"] = userid;
                Session["GuestSS"] = 1;
                ViewBag.Path = "Home -> Notepad";
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

            var text = db.Texts.FirstOrDefault(x => x.Text_Id == _text.Text_Id);
            if(text != null)
            {
                if (_text.Text1 != null)
                    text.Text1 = _text.Text1.Trim();
                else
                    text.Text1 = "add text";
                db.SaveChanges();
                int id = Convert.ToInt32(TempData["noteid"]);

                var note = db.Notepads.Where(x => x.Notepad_Id.Equals(id)).First();
                note.Update_Date = DateTime.Now;
                db.SaveChanges();
            }

            return null;
        }

        [HttpPost]
        public EmptyResult AddText(string text_id)
        {
            NoteContent nc = new NoteContent();
            nc.Notepad_Id = Convert.ToInt32(TempData["noteid"]);

            if (text_id.Equals("New"))
            {
                Text text = new Text();
                
                text.Text1 = "add text";
            
                db.Texts.Add(text);
                db.SaveChanges();

                text.Text_Id = "t_" + text.Id.ToString();
                
                db.SaveChanges();
                nc.Content_Id = text.Text_Id;
                
            } else
            {
                nc.Content_Id = text_id;
            }

            db.NoteContents.Add(nc);
            db.SaveChanges();

            var note = db.Notepads.Where(x => x.Notepad_Id.Equals(nc.Notepad_Id)).First();
            note.Update_Date = DateTime.Now;
            db.SaveChanges();
            return null;
        }

        [HttpPost]
        public EmptyResult Delete(string data)
        {
            NotepadVM model = new NotepadVM();
            int noteid = Convert.ToInt32(TempData["noteid"]);

            //remove from notecontent
            var content = db.NoteContents.Where(x => x.Content_Id.Equals(data) && x.Notepad_Id.Equals(noteid)).First();
            db.NoteContents.Remove(content);
            db.SaveChanges();


            //check if it exists in another row of notecontent(get only first row)
            content = null;

            var temp1 = db.NoteContents.Where(x => x.Content_Id.Equals(data)).ToList();
            if (temp1.Count > 0) content = temp1.First();

            BoardComponent boardcontent = null;
            var temp2 = db.BoardComponents.Where(x => x.Content_Id.Equals(data)).ToList();
            if (temp2.Count > 0) boardcontent = temp2.First();

            if (content != null || boardcontent != null)
                return null;

            
            if (data.Contains(model.textsubstr))
            {
                var text = db.Texts.Where(x => x.Text_Id.Equals(data)).First();
                db.Texts.Remove(text);
            }
            else if (data.Contains(model.imagesubstr))
            {
                var image = db.Images.Where(x => x.Image_Id.Equals(data)).First();
                db.Images.Remove(image);
            }
            else if (data.Contains(model.audiosubstr))
            {
                var audio = db.Audios.Where(x => x.Audio_Id.Equals(data)).First();
                db.Audios.Remove(audio);
            }
            else if (data.Contains(model.videosubstr))
            {
                var video = db.Videos.Where(x => x.Video_Id.Equals(data)).First();
                db.Videos.Remove(video);
            }
            else if (data.Contains(model.filesubstr))
            {
                var file = db.Files.Where(x => x.File_Id.Equals(data)).First();
                db.Files.Remove(file);
            }
            else if (data.Contains(model.canvassubstr))
            {
                var canvas = db.Canvases.Where(x => x.Canvas_Id.Equals(data)).First();
                db.Canvases.Remove(canvas);
            }
            db.SaveChanges();
            return null;
        }


        public EmptyResult DeleteNotepad()
        {
            int noteid = Convert.ToInt32(TempData["noteid"]);
            Session["NoteIndex"] = null;

            var contents = db.NoteContents.Where(x => x.Notepad_Id.Equals(noteid)).ToList();
            foreach (var item in contents)
            {
                Delete(item.Content_Id);
            }

            TempData["noteid"] = null;
            var notepad = db.Notepads.Where(x => x.Notepad_Id.Equals(noteid)).First();
            db.Notepads.Remove(notepad);
            db.SaveChanges();
            return null;
        }

        public EmptyResult ManageTag(TagVM data)
        {
            int noteid = Convert.ToInt32(TempData["noteid"]);
            var notepad = db.Notepads.Where(x => x.Notepad_Id.Equals(noteid)).First();
            if (data.TagName.Equals("remove"))
            {
                var tempTag = Convert.ToInt32(notepad.Tag_Id);
                
                notepad.Tag_Id = null;
                db.SaveChanges();

                if (data.ApplyContents)
                {
                    var contents = db.NoteContents.Where(x => x.Notepad_Id.Equals(notepad.Notepad_Id)).ToList();
                    NotepadVM model = new NotepadVM();
                    foreach (var item in contents)
                    {
                        dynamic content = GetContent(item);
                        content.Tag_Id = null;
                        db.SaveChanges();
                    }
                }

                if (tempTag == 0) return null;
                var tag = db.Tags.Where(x => x.Tag_Id.Equals(tempTag)).First();
                if (tag.Notepads.Count == 0 && tag.Texts.Count == 0)
                {
                    db.Tags.Remove(tag);
                    db.SaveChanges();
                }
            }
            else
            {
                var tag = db.Tags.Where(x => x.Tag_Name.Equals(data.TagName)).First();
                if (tag != null) //not added
                {
                    tag.Tag_Name = data.TagName;
                    db.SaveChanges();
                }

                notepad.Tag_Id = tag.Tag_Id;
                db.SaveChanges();
                if (data.ApplyContents)
                {
                    var contents = db.NoteContents.Where(x => x.Notepad_Id.Equals(notepad.Notepad_Id)).ToList();
                    NotepadVM model = new NotepadVM();
                    foreach (var item in contents)
                    {
                        dynamic content = GetContent(item);
                        content.Tag_Id = tag.Tag_Id;
                        db.SaveChanges();
                    }
                }
                
            }

            return null;
        }

        public EmptyResult ManageContentTag(TagVM data)
        {
            int noteid = Convert.ToInt32(TempData["noteid"]);
            var notepad = db.Notepads.Where(x => x.Notepad_Id.Equals(noteid)).First();
            var item = db.NoteContents.Where(x => x.Content_Id.Equals(data.Content_Id)).First();

            if (data.TagName.Equals("remove") || (data.TagName.Equals("parent") && notepad.Tag_Id == null))
            {
                dynamic content = GetContent(item);

                int tempTag = Convert.ToInt32(content.Tag_Id);
                content.Tag_Id = null;
                db.SaveChanges();

                if (tempTag == 0) return null;
                var tag = db.Tags.Where(x => x.Tag_Id.Equals(tempTag)).First();
                if (tag.Notepads.Count == 0 && tag.Texts.Count == 0)
                {
                    db.Tags.Remove(tag);
                    db.SaveChanges();
                }
            }
            else if (data.TagName.Equals("parent"))
            {
                var tempTag = Convert.ToInt32(notepad.Tag_Id);
                dynamic content = GetContent(item);
                content.Tag_Id = tempTag;
                db.SaveChanges();
            }
            else
            {
                var tag = db.Tags.Where(x => x.Tag_Name.Equals(data.TagName)).First();
                if (tag != null) //not added
                {
                    tag.Tag_Name = data.TagName;
                    db.SaveChanges();
                }

                dynamic content = GetContent(item);
                
                content.Tag_Id = tag.Tag_Id;
                db.SaveChanges();
            }

            return null;
        }

        dynamic GetContent(NoteContent item)
        {
            NotepadVM model = new NotepadVM();
            dynamic content = null;
            if (item.Content_Id.Contains(model.textsubstr))
                content = db.Texts.Where(x => x.Text_Id.Equals(item.Content_Id)).First();
            else if (item.Content_Id.Contains(model.imagesubstr))
                content = db.Images.Where(x => x.Image_Id.Equals(item.Content_Id)).First();
            else if (item.Content_Id.Contains(model.audiosubstr))
                content = db.Audios.Where(x => x.Audio_Id.Equals(item.Content_Id)).First();
            else if (item.Content_Id.Contains(model.videosubstr))
                content = db.Videos.Where(x => x.Video_Id.Equals(item.Content_Id)).First();
            else if (item.Content_Id.Contains(model.filesubstr))
                content = db.Files.Where(x => x.File_Id.Equals(item.Content_Id)).First();
            else if (item.Content_Id.Contains(model.canvassubstr))
                content = db.Canvases.Where(x => x.Canvas_Id.Equals(item.Content_Id)).First();
            return content;
        }
    }
}