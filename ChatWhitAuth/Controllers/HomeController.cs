using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChatWhitAuth.Models;

namespace ChatWhitAuth.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult ChatView()
        {
            return View("ChatView");
        }
    }
}