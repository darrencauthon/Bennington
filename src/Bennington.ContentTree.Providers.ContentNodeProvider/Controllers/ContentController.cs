using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Controllers
{
    public class ContentController : Controller
    {
        public ActionResult Index()
        {
            var routeData = RouteData;
            return View();
        }
    }
}
