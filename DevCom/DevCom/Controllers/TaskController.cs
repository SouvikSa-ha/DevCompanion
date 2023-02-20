using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevCom.Models;

namespace DevCom.Controllers
{
    public class TaskController : Controller
    {
        DevCom_DBEntities db = new DevCom_DBEntities();
        // GET: Task
        public ActionResult Index()
        {
            ViewBag.Path = "Home -> Task";
            var uid = Convert.ToInt32(Session["UidSS"]);
            var tasks = db.Tasks.Where(x => x.Uid.Equals(uid)).ToList();
            return View(tasks);
        }

        [HttpPost]
        public EmptyResult ChangeState(Task task)
        {
            Task _task = db.Tasks.Where(x => x.Task_Id.Equals(task.Task_Id)).First();
            _task.Status = task.Status;
            db.SaveChanges();
            return null;
        }

        [HttpPost]
        public EmptyResult Add(Task task)
        {
            task.Creation_Time = DateTime.Now;
            task.Uid = Convert.ToInt32(Session["UidSS"]);
            db.Tasks.Add(task);
            db.SaveChanges();
            return null;
        }

        [HttpPost]
        public EmptyResult Edit(Task task)
        {
            Task _task = db.Tasks.Where(x => x.Task_Id.Equals(task.Task_Id)).First();
            _task.Short_Description = task.Short_Description;
            db.SaveChanges();
            return null;
        }

        [HttpPost]
        public EmptyResult Delete(Task task)
        {
            Task _task = db.Tasks.Where(x => x.Task_Id.Equals(task.Task_Id)).First();
            db.Tasks.Remove(_task);
            db.SaveChanges();
            return null;
        }
    }
}