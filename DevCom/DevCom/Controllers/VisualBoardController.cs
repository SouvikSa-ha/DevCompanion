using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DevCom.Controllers
{
    public class VisualBoardController : Controller
    {
        // GET: VisualBoard
        public ActionResult Index()
        {
            return View();
        }
    }
}