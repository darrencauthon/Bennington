using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Bennington.ContentTree.Providers.ContentNodeProvider.ViewModelBuilders;
using Bennington.Core.Helpers;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Controllers
{
    public class ContentTreeController : Controller
    {
        private readonly Func<IContentTreeNodeDisplayViewModelBuilder> contentTreeNodeDisplayViewModelBuilder;
        private readonly Func<IRawUrlGetter> rawUrlGetter;

        public ContentTreeController(Func<IContentTreeNodeDisplayViewModelBuilder> contentTreeNodeDisplayViewModelBuilder,
                                 Func<IRawUrlGetter> rawUrlGetter)
        {
            this.rawUrlGetter = rawUrlGetter;
            this.contentTreeNodeDisplayViewModelBuilder = contentTreeNodeDisplayViewModelBuilder;
        }

        public ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Display(string currentController, string currentAction)
        {
            return View("Display", contentTreeNodeDisplayViewModelBuilder().BuildViewModel(rawUrlGetter().GetRawUrl(), (ControllerContext ?? new ControllerContext()).RouteData, currentAction));
        }
    }
}
