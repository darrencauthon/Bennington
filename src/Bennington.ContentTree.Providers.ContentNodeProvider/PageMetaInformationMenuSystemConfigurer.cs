using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Bennington.Cms.MenuSystem;
using Bennington.ContentTree.Providers.ContentNodeProvider.Controllers;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;

namespace Bennington.ContentTree.Providers.ContentNodeProvider
{
    public class PageMetaInformationMenuSystemConfigurer : IMenuSystemConfigurer
    {
        public void Configure(IMenuRegistry menuRegistry)
        {
            menuRegistry.Add(new SubActionOnlyVisibleDuringModifyAction("Page Information", "Modify", typeof(ContentTreeNodeController).Name.Replace("Controller", string.Empty)));
            menuRegistry.Add(new SubActionOnlyVisibleDuringModifyAction("Meta Information", "ManageMetaInformation", typeof(ContentTreeNodeController).Name.Replace("Controller", string.Empty)));
        }
    }

    public class SubActionOnlyVisibleDuringModifyAction : ActionSubMenuItem
    {
        private readonly string name;
        private readonly string actionName;
        private readonly string controllerName;
        private readonly object routeValues;

        public SubActionOnlyVisibleDuringModifyAction(string name, string actionName, string controllerName) : this(name, actionName, controllerName, null)
        {
            this.name = name;
            this.actionName = actionName;
            this.controllerName = controllerName;
        }

        public SubActionOnlyVisibleDuringModifyAction(string name, string actionName, string controllerName, object routeValues) : base(name, actionName, controllerName, routeValues)
        {
            this.name = name;
            this.actionName = actionName;
            this.controllerName = controllerName;
            this.routeValues = routeValues;
        }

        public override Cms.Models.MenuSystem.SubMenuItemViewModel GetViewModel(ControllerContext controllerContext)
        {
            var urlHelper = new UrlHelper(controllerContext.RequestContext);
            var subMenuItemViewModel = base.GetViewModel(controllerContext);
            var routeData = GetRootRouteData(controllerContext);

            subMenuItemViewModel.Visible = subMenuItemViewModel.Visible && routeData.Values["action"].ToString() == "Modify";

            subMenuItemViewModel.Url = urlHelper.Action(actionName, controllerName, new { TreeNodeId = GetTreeNodeId(controllerContext), contentItemId = GetContentItemId(controllerContext) });

            return subMenuItemViewModel;
        }

        private static string GetContentItemId(ControllerContext controllerContext)
        {
            if (string.IsNullOrEmpty(controllerContext.RequestContext.HttpContext.Request["contentItemId"]))
                return controllerContext.RequestContext.HttpContext.Request.Form[typeof (ContentTreeNodeInputModel).Name + ".Action"];

            return controllerContext.RequestContext.HttpContext.Request["contentItemId"];
        }

        private static string GetTreeNodeId(ControllerContext controllerContext)
        {
            if (string.IsNullOrEmpty(controllerContext.RequestContext.HttpContext.Request["treeNodeId"]))
                return controllerContext.RequestContext.HttpContext.Request.Form[typeof(ContentTreeNodeInputModel).Name + ".TreeNodeId"];

            return controllerContext.RequestContext.HttpContext.Request["treeNodeId"];
        }

        private static RouteData GetRootRouteData(ControllerContext controllerContext)
        {
            return controllerContext.IsChildAction ? GetRootRouteData(controllerContext.ParentActionViewContext) : controllerContext.RouteData;
        }
    }
}
