using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevCom.Models;
using DevCom.Models.ViewModels;

namespace DevCom.Controllers
{
    public class ReminderController : Controller
    {
        DevCom_DBEntities dc = new DevCom_DBEntities();
        // GET: Reminder
        public ActionResult Index()
        {
            ViewBag.Path = "Home -> Reminder";
            return View();
        }

        [HttpGet]
        public JsonResult GetEvents()
        {
            var uid = Convert.ToInt32(Session["UidSS"]);
            var reminders = dc.Reminders.Where(x=>x.Uid.Equals(uid)).ToList();
            var events = new List<ReminderVM>();
            foreach (var item in reminders)
            {
                var _event = new ReminderVM();
                _event.Reminder_Id = item.Reminder_Id;
                _event.Title = item.Title;
                _event.Info = item.Info;
                _event.Reminder_Time = item.Reminder_Time;
                _event.Deadline = item.Deadline;
                _event.ThemeColor = item.ThemeColor;
                _event.IsFullDay = item.IsFullDay;
                events.Add(_event);
            }
            

            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult SaveEvent(Reminder e)
        {
            var status = false;
                if (e.Reminder_Id > 0)
                {
                    //Update the event
                    var v = dc.Reminders.Where(a => a.Reminder_Id == e.Reminder_Id).FirstOrDefault();
                    if (v != null)
                    {
                        v.Title = e.Title;
                        v.Reminder_Time = e.Reminder_Time;
                        v.Deadline = e.Deadline;
                        v.Info = e.Info;
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColor = e.ThemeColor;
                    }
                }
                else
                {
                    dc.Reminders.Add(e);
                }

                dc.SaveChanges();
                status = true;
            
            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public JsonResult DeleteEvent(string eventID)
        {
            var status = false;
            var _eventID = Convert.ToInt32(eventID);
                var v = dc.Reminders.Where(a => a.Reminder_Id == _eventID).FirstOrDefault();
                if (v != null)
                {
                    dc.Reminders.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            return new JsonResult { Data = new { status = status } };
        }
    }
}