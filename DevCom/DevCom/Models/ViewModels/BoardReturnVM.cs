using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevCom.Models.ViewModels
{
    public class BoardReturnVM
    {
        public string Text_Id { get; set; }
        public string Text1 { get; set; }
        public int X_pos { get; set; }
        public int Y_pos { get; set; }
        public int Board_Id { get; set; }
    }
}