using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevCom.Models.ViewModels
{
    public class BoardVM
    {
        public Visual_Boards Visual_Board { get; set; }
        public IEnumerable<BoardComponent> BoardComponents { get; set; }

        public string textsubstr = "t";
        public string imagesubstr = "i";
        public string audiosubstr = "a";
        public string videosubstr = "v";
        public string filesubstr = "f";
    }
}