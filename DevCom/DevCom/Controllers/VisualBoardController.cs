using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevCom.Models;
using DevCom.Models.ViewModels;

namespace DevCom.Controllers
{
    public class VisualBoardController : Controller
    {
        DevCom_DBEntities db = new DevCom_DBEntities();
        // GET: VisualBoard
        public ActionResult Index()
        {
            ViewBag.Path = "Home -> Visual Board";
            var _uid = Session["UidSS"];
            if (_uid == null) RedirectToAction("Login", "Auth");
            var uid = Convert.ToInt32(_uid);
            var boards = db.Visual_Boards.Where(x => x.Uid.Equals(uid)).ToList();
            return View(boards);
        }



        public ActionResult Board(int id)
        {
            BoardVM vm = new BoardVM();
            vm.Visual_Board = db.Visual_Boards.Where(x => x.Board_Id.Equals(id)).First();
            vm.BoardComponents = db.BoardComponents.Where(x => x.Board_Id.Equals(id)).ToList();
            //vm.Texts

            return View(vm);
        }

        public JsonResult Update_Text(BoardReturnVM vm)
        {
            var temp = db.Texts.Where(x => x.Text_Id.Equals(vm.Text_Id)).ToList();
            if(temp.Count == 0)
            {
                BoardComponent bc = new BoardComponent();
                Text text = new Text();
                if (vm.Text1 == "") text.Text1 = "Add text";
                else text.Text1 = vm.Text1;
                db.Texts.Add(text);
                db.SaveChanges();

                text.Text_Id = "t_" + text.Id.ToString();

                db.SaveChanges();

                vm.Text_Id = text.Text_Id;
                bc.Content_Id = text.Text_Id;
                bc.PositionX = vm.X_pos;
                bc.PositionY = vm.Y_pos;
                bc.Board_Id = vm.Board_Id;
                db.BoardComponents.Add(bc);
                db.SaveChanges();

                var vb = db.Visual_Boards.Where(x => x.Board_Id.Equals(vm.Board_Id)).First();
                vb.Update_Date = DateTime.Now;
                db.SaveChanges();
            }
            else
            {
                Text text = db.Texts.Where(x => x.Id.Equals(vm.Text_Id)).First();
                text.Text1 = vm.Text1;
                db.SaveChanges();
                var vb = db.Visual_Boards.Where(x => x.Board_Id.Equals(vm.Board_Id)).First();
                vb.Update_Date = DateTime.Now;
                db.SaveChanges();
            }
            return Json(new { msg =  vm.Text_Id});
        }

        public ActionResult Plain()
        {
            
            return View();
        }
        public JsonResult Another(string[] s)
        {
            var uid = Convert.ToInt32(Session["UidSS"]);
            return Json(new { msg = "t_101" }); 
        }

        public ActionResult Canvas()
        {
            return View();
        }
    }
}