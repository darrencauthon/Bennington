using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bennington.ContentTree.Providers.ContentNodeProvider.Mappers;
using Bennington.ContentTree.Providers.ContentNodeProvider.Repositories;
using Bennington.ContentTree.Providers.SectionNodeProvider.Mappers;
using Bennington.ContentTree.Providers.SectionNodeProvider.Repositories;
using Bennington.ContentTree.Providers.ToolLinkNodeProvider.Mappers;
using Bennington.ContentTree.Providers.ToolLinkNodeProvider.Repositories;
using MvcTurbine;
using MvcTurbine.Blades;

namespace Bennington.ContentTree.Caching.Blades
{
    public class RegisterSingletonsBlade : Blade
    {
        public override void Spin(IRotorContext context)
        {
            context.ServiceLocator.Register<IContentNodeProviderDraftToContentTreeNodeMapper>(context.ServiceLocator.Resolve<ContentNodeProviderDraftToContentTreeNodeMapper>());
            context.ServiceLocator.Register<IContentNodeProviderPublishedVersionToContentTreeNodeMapper>(context.ServiceLocator.Resolve<ContentNodeProviderPublishedVersionToContentTreeNodeMapper>());
            context.ServiceLocator.Register<IContentTreePageNodeToContentTreeNodeDisplayMetaViewModelMapper>(context.ServiceLocator.Resolve<ContentTreePageNodeToContentTreeNodeDisplayMetaViewModelMapper>());
            context.ServiceLocator.Register<ISectionNodeProviderDraftToContentTreeSectionNodeMapper>(context.ServiceLocator.Resolve<SectionNodeProviderDraftToContentTreeSectionNodeMapper>());
            context.ServiceLocator.Register<IToolLinkProviderDraftToToolLinkMapper>(context.ServiceLocator.Resolve<ToolLinkProviderDraftToToolLinkMapper>());

            context.ServiceLocator.Register<IContentTreeSectionNodeRepository>(context.ServiceLocator.Resolve<ContentTreeSectionNodeRepository>());
            context.ServiceLocator.Register<IToolLinkProviderDraftRepository>(context.ServiceLocator.Resolve<ToolLinkProviderDraftRepository>());
            context.ServiceLocator.Register<IContentNodeProviderPublishedVersionRepository>(context.ServiceLocator.Resolve<ContentNodeProviderPublishedVersionRepository>());
            context.ServiceLocator.Register<IContentNodeProviderDraftRepository>(context.ServiceLocator.Resolve<ContentNodeProviderDraftRepository>());
        }
    }
}
