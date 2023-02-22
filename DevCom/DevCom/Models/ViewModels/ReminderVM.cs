using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevCom.Models.ViewModels
{
    public class ReminderVM
    {
        public int Reminder_Id { get; set; }
        public System.DateTime Reminder_Time { get; set; }
        public string Title { get; set; }
        public string Info { get; set; }
        public Nullable<System.DateTime> Deadline { get; set; }
        public string ThemeColor { get; set; }
        public bool IsFullDay { get; set; }
    }
}