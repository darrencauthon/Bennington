using System;
using System.Collections.Generic;
using System.Linq;
using Bennington.Content.Data;
using Bennington.ContentTree.Models;
using Bennington.ContentTree.Providers.ContentNodeProvider.Context;
using Bennington.ContentTree.Providers.ContentNodeProvider.Repositories;
using MvcTurbine.ComponentModel;
using ContentType = Bennington.ContentTree.Providers.ContentNodeProvider.Data.ContentType;

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

        public override string Controller { get; set; }

        public override IEnumerable<ContentTree.Models.ContentTreeNodeContentItem> ContentTreeNodeContentItems
        {
            get
            {
                var allContentTypes = contentTreeRepository().GetAllContentTypes();
                var contentType = allContentTypes.Where(a => a.ControllerName == Controller).FirstOrDefault() ?? new ContentType();
                var contentActions = contentTreeRepository().GetAllContentActions().Where(a => a.ContentTypeId == contentType.ContentTypeId);
                var list = new List<ContentTreeNodeContentItem>();
                foreach (var action in contentActions)
                {
                    list.Add(new ContentTreeNodeContentItem()
                                 {
                                     Id = action.Action,
                                     Name = action.DisplayName
                                 });
                }
                return list;
            }
            set
            {
                base.ContentTreeNodeContentItems = value;
            }
        }
    }
}