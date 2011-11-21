using System;
using System.Collections.Generic;
using System.Linq;
using Bennington.Content.Data;
using Bennington.ContentTree.Models;
using Bennington.ContentTree.Providers.ContentNodeProvider.Context;
using Bennington.ContentTree.Providers.ContentNodeProvider.Repositories;
using Bennington.ContentTree.Repositories;
using MvcTurbine.ComponentModel;
using Action = Bennington.ContentTree.Models.Action;
using ContentType = Bennington.ContentTree.Data.ContentType;

namespace Bennington.ContentTree.Providers.ContentNodeProvider
{
    public class EngineTreeNodeProvider : ContentNodeProvider
    {
        private readonly Func<IContentTreeRepository> contentTreeRepository;

        public EngineTreeNodeProvider(IServiceLocator serviceLocator,
                                        Func<IContentTreeRepository> contentTreeRepository)
            : base(serviceLocator.Resolve<IContentTreeNodeVersionContext>())
        {
            this.contentTreeRepository = contentTreeRepository;
        }

        public override string Name { get; set; }

        public override IQueryable<ContentTreeNode> GetAll()
        {
            var treeNodeExtensions = new List<ContentTreeNode>();
            foreach (var item in base.GetAll())
            {
                item.IconUrl = "Content/ContentNodeProvider/controller.gif";
                treeNodeExtensions.Add(item);
            }

            return treeNodeExtensions.AsQueryable();
        }


        public override string Controller { get; set; }

        public override IEnumerable<Action> Actions
        {
            get
            {
                var allContentTypes = contentTreeRepository().GetAllContentTypes();
                var contentType = allContentTypes.Where(a => a.ControllerName == Controller).FirstOrDefault() ?? new ContentType();
                var contentActions = contentTreeRepository().GetAllContentActions().Where(a => a.ContentTypeId == contentType.ContentTypeId);
                var list = new List<Action>();
                foreach (var action in contentActions)
                {
                    list.Add(new Action()
                                 {
                                     ControllerAction = action.Action,
                                     DisplayName = action.DisplayName
                                 });
                }
                return list;
            }
            set
            {
                base.Actions = value;
            }
        }
    }
}