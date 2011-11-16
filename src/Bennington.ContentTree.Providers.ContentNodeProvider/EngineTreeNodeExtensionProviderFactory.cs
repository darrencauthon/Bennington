using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bennington.ContentTree.Models;

namespace Bennington.ContentTree.Providers.ContentNodeProvider
{
    public class EngineTreeNodeExtensionProviderFactory : IAmATreeNodeExtensionProviderFactory
    {
        public IAmATreeNodeExtensionProvider[] GetTreeNodeExtensionProviders()
        {
            return new IAmATreeNodeExtensionProvider[]
                       {
                           new FakeIAmATreeNodeExtensionProvider()
                       };
        }

        public class FakeIAmATreeNodeExtensionProvider : IAmATreeNodeExtensionProvider
        {
            public IQueryable<IAmATreeNodeExtension> GetAll()
            {
                return new IAmATreeNodeExtension[]{}.AsQueryable();
            }

            public string Name
            {
                get { return "Engine"; }
            }

            public string ControllerToUseForModification
            {
                get { return "Unknown"; }
                set { throw new NotImplementedException(); }
            }

            public string ActionToUseForModification
            {
                get { return "Unknown"; }
                set { throw new NotImplementedException(); }
            }

            public string ControllerToUseForCreation
            {
                get { return "Unknown"; }
                set { throw new NotImplementedException(); }
            }

            public string ActionToUseForCreation
            {
                get { return "Unknown"; }
                set { throw new NotImplementedException(); }
            }

            public IEnumerable<ContentTreeNodeContentItem> ContentTreeNodeContentItems
            {
                get { return new ContentTreeNodeContentItem[]{}; }
                set { throw new NotImplementedException(); }
            }

            public bool MayHaveChildNodes
            {
                get { return false; }
                set { throw new NotImplementedException(); }
            }

            public void RegisterRouteForTreeNodeId(string treeNodeId)
            {
                throw new NotImplementedException();
            }
        }
    }
}
