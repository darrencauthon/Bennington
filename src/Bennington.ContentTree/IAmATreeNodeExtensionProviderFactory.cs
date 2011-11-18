using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bennington.ContentTree
{
    public interface IAmATreeNodeExtensionProviderFactory
    {
        IContentTreeNodeProvider[] GetTreeNodeExtensionProviders();
    }
}