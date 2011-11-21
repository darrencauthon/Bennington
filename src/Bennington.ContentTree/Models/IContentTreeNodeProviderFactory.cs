namespace Bennington.ContentTree.Models
{
    public interface IContentTreeNodeProviderFactory
    {
        IContentTreeNodeProvider[] GetTreeNodeExtensionProviders();
    }
}