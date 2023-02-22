using DevCom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevCom.Models.ViewModels;
using System.Threading.Tasks;
using System.IO;

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
                var _temp2 = db.Tags.Where(x => x.Uid.Equals(uid)).ToList();
                myModel.AllTags = new List<string>();
                if(_temp2.Count > 0)
                {
                    foreach (var item in _temp2)
                    {
                        myModel.AllTags.Add(item.Tag_Name);
                    }
                    myModel.AllTags.Sort();
                }
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
        public EmptyResult Update_Note_Title(Notepad _notepad)
        {
            var notepad = db.Notepads.Where(x => x.Notepad_Id.Equals(_notepad.Notepad_Id)).First();
            notepad.Title = _notepad.Title;
            notepad.Update_Date = DateTime.Now;
            db.SaveChanges();
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
                string fullPath = Request.MapPath(@image.Image_link);
                System.IO.File.Delete(fullPath);
                db.Images.Remove(image);
            }
            else if (data.Contains(model.audiosubstr))
            {
                var audio = db.Audios.Where(x => x.Audio_Id.Equals(data)).First();
                string fullPath = Request.MapPath(@audio.Audio_link);
                System.IO.File.Delete(fullPath);
                db.Audios.Remove(audio);
            }
            else if (data.Contains(model.videosubstr))
            {
                var video = db.Videos.Where(x => x.Video_Id.Equals(data)).First();
                string fullPath = Request.MapPath(@video.Video_link);
                System.IO.File.Delete(fullPath);
                db.Videos.Remove(video);
            }
            else if (data.Contains(model.filesubstr))
            {
                var file = db.Files.Where(x => x.File_Id.Equals(data)).First();
                string fullPath = Request.MapPath(@file.File_link);
                System.IO.File.Delete(fullPath);
                db.Files.Remove(file);
            }
            else if (data.Contains(model.canvassubstr))
            {
                var canvas = db.Canvases.Where(x => x.Canvas_Id.Equals(data)).First();
                string fullPath = Request.MapPath(@canvas.Canvas_link);
                System.IO.File.Delete(fullPath);
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
                var tag = db.Tags.Where(x => x.Tag_Name.Equals(data.TagName)).ToList();
                if (tag.Count == 0) //not added
                {
                    Tag _tag = new Tag();
                    _tag.Tag_Name = data.TagName;
                    _tag.Uid = Convert.ToInt32(Session["UidSS"]);
                    tag.Add(_tag);
                    db.Tags.Add(_tag);
                    db.SaveChanges();
                }

                notepad.Tag_Id = tag.First().Tag_Id;
                db.SaveChanges();
                if (data.ApplyContents)
                {
                    var contents = db.NoteContents.Where(x => x.Notepad_Id.Equals(notepad.Notepad_Id)).ToList();
                    NotepadVM model = new NotepadVM();
                    foreach (var item in contents)
                    {
                        dynamic content = GetContent(item);
                        content.Tag_Id = tag.First().Tag_Id;
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
                var tag = db.Tags.Where(x => x.Tag_Name.Equals(data.TagName)).ToList();
                if (tag.Count == 0) //not added
                {
                    Tag _tag = new Tag(); 
                    _tag.Tag_Name = data.TagName;
                    _tag.Uid = Convert.ToInt32(Session["UidSS"]);
                    tag.Add(_tag);
                    db.Tags.Add(_tag);
                    db.SaveChanges();
                }

                dynamic content = GetContent(item);
                
                content.Tag_Id = tag.First().Tag_Id;
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

        [HttpPost]
        public EmptyResult AddContent(string content_id)
        {
            NoteContent nc = new NoteContent();
            nc.Notepad_Id = Convert.ToInt32(TempData["noteid"]);
            
            nc.Content_Id = content_id;

            db.NoteContents.Add(nc);
            db.SaveChanges();

            var note = db.Notepads.Where(x => x.Notepad_Id.Equals(nc.Notepad_Id)).First();
            note.Update_Date = DateTime.Now;
            db.SaveChanges();
            return null;
        }

        [HttpPost]
        public EmptyResult UploadFiles(IEnumerable<HttpPostedFileBase> files, string content_type)
        {
            NoteContent nc = new NoteContent();
            nc.Notepad_Id = Convert.ToInt32(TempData["noteid"]);
            foreach (var file in files)
            {
                string filePath = Guid.NewGuid() + Path.GetExtension(file.FileName);
                if (content_type.Equals("Image"))
                {
                    Image content = new Image();
                    file.SaveAs(Path.Combine(Server.MapPath("~/Assets/Images"), filePath));
                    content.Image_link = @"~/Assets/Images/" + @filePath;
                    db.Images.Add(content);
                    db.SaveChanges();

                    content.Image_Id = "i_" + content.Id.ToString();

                    db.SaveChanges();
                    nc.Content_Id = content.Image_Id;
                    db.NoteContents.Add(nc);
                    db.SaveChanges();
                }
                else if (content_type.Equals("Audio"))
                {
                    Audio content = new Audio();
                    file.SaveAs(Path.Combine(Server.MapPath("~/Assets/Audios"), filePath));
                    content.Audio_link = @"~/Assets/Audios/" + @filePath;
                    db.Audios.Add(content);
                    db.SaveChanges();

                    content.Audio_Id = "a_" + content.Id.ToString();

                    db.SaveChanges();
                    nc.Content_Id = content.Audio_Id;
                    db.NoteContents.Add(nc);
                    db.SaveChanges();
                }
                else if (content_type.Equals("Video"))
                {
                    Video content = new Video();
                    file.SaveAs(Path.Combine(Server.MapPath("~/Assets/Videos"), filePath));
                    content.Video_link = @"~/Assets/Videos/" + @filePath;
                    db.Videos.Add(content);
                    db.SaveChanges();

                    content.Video_Id = "v_" + content.Id.ToString();

                    db.SaveChanges();
                    nc.Content_Id = content.Video_Id;
                    db.NoteContents.Add(nc);
                    db.SaveChanges();
                }
                else if (content_type.Equals("PDF"))
                {
                    Models.File content = new Models.File();
                    file.SaveAs(Path.Combine(Server.MapPath("~/Assets/Files"), filePath));
                    content.File_link = @"~/Assets/Files/" + @filePath;
                    db.Files.Add(content);
                    db.SaveChanges();

                    content.File_Id = "f_" + content.Id.ToString();

                    db.SaveChanges();
                    nc.Content_Id = content.File_Id;
                    db.NoteContents.Add(nc);
                    db.SaveChanges();
                }
            }

            var note = db.Notepads.Where(x => x.Notepad_Id.Equals(nc.Notepad_Id)).First();
            note.Update_Date = DateTime.Now;
            db.SaveChanges();

            return null;
        }

        public EmptyResult ShiftContentRow(RowShiftVM model)
        {
            var content = db.NoteContents.Where(x => x.Notepad_Id.Equals(model.Notepad_Id)).ToList();
            if(model.Direction == "up")
            {
                for (int i = 1; i < content.Count; i++)
                {
                    if (content[i].Content_Id == model.Content_Id)
                    {
                        var temp = content[i].Content_Id;
                        content[i].Content_Id = content[i - 1].Content_Id;
                        content[i - 1].Content_Id = temp;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < content.Count-1; i++)
                {
                    if (content[i].Content_Id == model.Content_Id)
                    {
                        var temp = content[i].Content_Id;
                        content[i].Content_Id = content[i + 1].Content_Id;
                        content[i + 1].Content_Id = temp;
                        break;
                    }
                }
            }

            db.SaveChanges();
            return null;
        }


    }
}