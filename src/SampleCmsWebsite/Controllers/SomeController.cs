using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SampleCmsWebsite.Models;

namespace SampleCmsWebsite.Controllers
{
    public class SomeController : Controller
    {
        public ActionResult Index()
        {
            return View("Index", new SomeControllerIndexViewModel());
        }
    }
}
