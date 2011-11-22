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

        public ContentTreeController(Func<IContentTreeNodeDisplayViewModelBuilder> contentTreeNodeDisplayViewModelBuilder)
        {
            this.contentTreeNodeDisplayViewModelBuilder = contentTreeNodeDisplayViewModelBuilder;
        }

        public ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Display(string treeNodeId, string actionId)
        {
            return View("Display", contentTreeNodeDisplayViewModelBuilder().BuildViewModel(treeNodeId, actionId));
        }
    }
}
