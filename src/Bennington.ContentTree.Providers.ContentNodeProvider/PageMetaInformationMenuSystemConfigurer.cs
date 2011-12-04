using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Bennington.Cms.MenuSystem;
using Bennington.ContentTree.Providers.ContentNodeProvider.Controllers;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;
using Bennington.ContentTree.Providers.ContentNodeProvider.Repositories;

namespace Bennington.ContentTree.Providers.ContentNodeProvider
{
    public class PageMetaInformationMenuSystemConfigurer : IMenuSystemConfigurer
    {
        private readonly IContentNodeProviderDraftRepository contentNodeProviderDraftRepository;

        public PageMetaInformationMenuSystemConfigurer(IContentNodeProviderDraftRepository contentNodeProviderDraftRepository)
        {
            this.contentNodeProviderDraftRepository = contentNodeProviderDraftRepository;
        }

        public void Configure(IMenuRegistry menuRegistry)
        {
            menuRegistry.Add(new SubActionOnlyVisibleDuringModifyAction(contentNodeProviderDraftRepository, "Page Information", "Modify", typeof(ContentTreeNodeController).Name.Replace("Controller", string.Empty)));
            menuRegistry.Add(new SubActionOnlyVisibleDuringModifyAction(contentNodeProviderDraftRepository, "Meta Information", "ManageMetaInformation", typeof(ContentTreeNodeController).Name.Replace("Controller", string.Empty)));
        }
    }

    public class SubActionOnlyVisibleDuringModifyAction : ActionSubMenuItem
    {
        private readonly string name;
        private readonly string actionName;
        private readonly string controllerName;
        private readonly object routeValues;
        private readonly IContentNodeProviderDraftRepository contentNodeProviderDraftRepository;

        public SubActionOnlyVisibleDuringModifyAction(IContentNodeProviderDraftRepository contentNodeProviderDraftRepository, string name, string actionName, string controllerName) : this(contentNodeProviderDraftRepository, name, actionName, controllerName, null)
        {
            this.contentNodeProviderDraftRepository = contentNodeProviderDraftRepository;
            this.name = name;
            this.actionName = actionName;
            this.controllerName = controllerName;
        }

        public SubActionOnlyVisibleDuringModifyAction(IContentNodeProviderDraftRepository contentNodeProviderDraftRepository, string name, string actionName, string controllerName, object routeValues)
            : base(name, actionName, controllerName, routeValues)
        {
            this.contentNodeProviderDraftRepository = contentNodeProviderDraftRepository;
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

            subMenuItemViewModel.Visible = subMenuItemViewModel.Visible && (routeData.Values["action"].ToString() == "Modify" || routeData.Values["action"].ToString() == "ManageMetaInformation");

            subMenuItemViewModel.Url = urlHelper.Action(actionName, controllerName, new { TreeNodeId = GetTreeNodeId(controllerContext), pageId = GetPageId(controllerContext, GetTreeNodeId(controllerContext)) });

            return subMenuItemViewModel;
        }

        private string GetPageId(ControllerContext controllerContext, string treeNodeId)
        {
            var draft = contentNodeProviderDraftRepository
                            .GetAllContentNodeProviderDrafts()
                            .Where(a => a.TreeNodeId == treeNodeId && a.Action == GetContentItemId(controllerContext))
                            .FirstOrDefault();

            return draft == null ? null : draft.PageId;
        }

        private string GetContentItemId(ControllerContext controllerContext)
        {
            if (string.IsNullOrEmpty(controllerContext.RequestContext.HttpContext.Request["contentItemId"]))
            {
                if (!string.IsNullOrEmpty(controllerContext.RequestContext.HttpContext.Request.Form[typeof(ContentTreeNodeInputModel).Name + ".Action"]))
                {
                    return controllerContext.RequestContext.HttpContext.Request.Form[typeof(ContentTreeNodeInputModel).Name + ".Action"];
                }                
            } else
            {
                return controllerContext.RequestContext.HttpContext.Request["contentItemId"];                
            }

            return "Index";
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
