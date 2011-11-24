using System;
using System.Linq;
using System.Web.Routing;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;
using Bennington.Core.Helpers;
using Bennington.FileUploadHandling.Context;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.ViewModelBuilders
{
	public interface IContentTreeNodeDisplayViewModelBuilder
	{
		ContentTreeNodeDisplayViewModel BuildViewModel(string treeNodeId, string actionId);
	}

	public class ContentTreeNodeDisplayViewModelBuilder : IContentTreeNodeDisplayViewModelBuilder
	{
        private readonly IContentTreePageNodeContext contentTreePageNodeContext;
	    private readonly Func<IFileUploadContext> fileUploadContext;

        public ContentTreeNodeDisplayViewModelBuilder(IContentTreePageNodeContext contentTreePageNodeContext,
                                                      Func<IFileUploadContext> fileUploadContext)
	    {
	        this.fileUploadContext = fileUploadContext;
	        this.contentTreePageNodeContext = contentTreePageNodeContext;
	    }

	    public ContentTreeNodeDisplayViewModel BuildViewModel(string treeNodeId, string actionId)
	    {
            var node = contentTreePageNodeContext.GetAllContentTreePageNodes().Where(a => a.Id == treeNodeId && a.PageId == actionId).FirstOrDefault();

            if (node == null) return new ContentTreeNodeDisplayViewModel();

            return new ContentTreeNodeDisplayViewModel()
                       {
                           Body = node.Body,
                           Header = node.HeaderText,
                           HeaderImage = GetHeaderImageUrl(node)
                       };
		}

	    private string GetHeaderImageUrl(ContentTreePageNode node)
	    {
            if (string.IsNullOrEmpty(node.HeaderImage)) return null;

            var fileUpload = fileUploadContext();
            return string.Format("{0}{1}", fileUpload.GetUrlForFileUploadFolder(), fileUpload.GetUrlRelativeToUploadRoot(typeof(ContentTreeNodeInputModel).Name, "HeaderImage", node.HeaderImage));
	    }
	}
}
