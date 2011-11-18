using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Bennington.ContentTree.Helpers;
using Bennington.ContentTree.Providers.ToolLinkNodeProvider.Repositories;
using Bennington.ContentTree.Repositories;
using Bennington.Core.Helpers;
using MvcTurbine.Routing;

namespace Bennington.ContentTree.Providers.ToolLinkNodeProvider.Controllers
{
    public class ToolLinkController : Controller
    {
        private readonly ITreeNodeRepository treeNodeRepository;
        private readonly ITreeNodeIdToUrl treeNodeIdToUrl;
        private readonly IUrlToTreeNodeSummaryMapper urlToTreeNodeSummaryMapper;
        private readonly IRawUrlGetter rawUrlGetter;
        private readonly IToolLinkProviderDraftRepository toolLinkProviderDraftRepository;

        public ToolLinkController(ITreeNodeRepository treeNodeRepository, 
                                    ITreeNodeIdToUrl treeNodeIdToUrl,
                                    IUrlToTreeNodeSummaryMapper urlToTreeNodeSummaryMapper,
                                    IRawUrlGetter rawUrlGetter,
                                    IToolLinkProviderDraftRepository toolLinkProviderDraftRepository)
        {
            this.toolLinkProviderDraftRepository = toolLinkProviderDraftRepository;
            this.rawUrlGetter = rawUrlGetter;
            this.urlToTreeNodeSummaryMapper = urlToTreeNodeSummaryMapper;
            this.treeNodeIdToUrl = treeNodeIdToUrl;
            this.treeNodeRepository = treeNodeRepository;
        }

        public ActionResult Index()
        {
            var url = rawUrlGetter.GetRawUrl();
            var treeNodeSummary = urlToTreeNodeSummaryMapper.CreateInstance(url);

            var toolLink = toolLinkProviderDraftRepository.GetAll().Where(a => a.Id == treeNodeSummary.Id).FirstOrDefault();

            if (toolLink == null) return new HttpNotFoundResult();

            return new RedirectResult(toolLink.Url);
        }
    }
}
