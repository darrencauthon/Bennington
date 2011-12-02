using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Bennington.ContentTree.Domain.Commands;
using Bennington.ContentTree.Helpers;
using Bennington.ContentTree.Providers.ContentNodeProvider.Helpers;
using Bennington.ContentTree.Providers.ContentNodeProvider.Mappers;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;
using Bennington.ContentTree.Providers.ContentNodeProvider.ViewModelBuilders;
using Bennington.ContentTree.Repositories;
using Bennington.Core;
using Bennington.Core.Helpers;
using SimpleCqrs.Commanding;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Controllers
{
	public class ContentTreeNodeController : Controller
	{
		private readonly IContentTreeNodeToContentTreeNodeInputModelMapper contentTreeNodeToContentTreeNodeInputModelMapper;
        private readonly IContentTreePageNodeContext contentTreePageNodeContext;
		private readonly ITreeNodeRepository treeNodeRepository;
		private readonly IContentTreeNodeProviderContext contentTreeNodeProviderContext;
		private readonly ICommandBus commandBus;
		private readonly IGuidGetter guidGetter;
		private readonly IContentTreeNodeFileUploadPersister contentTreeNodeFileUploadPersister;
	    private readonly ICurrentUserContext currentUserContext;
	    private readonly ITreeNodeIdToUrl treeNodeIdToUrl;
	    private readonly IGetUrlOfFrontSideWebsite getUrlOfFrontSideWebsite;
	    private readonly IContentTree contentTree;
	    private readonly IContentTreeNodeMetaInformationViewModelBuilder contentTreeNodeMetaInformationViewModelBuilder;

	    public ContentTreeNodeController(IContentTreePageNodeContext contentTreePageNodeContext, 
											IContentTreeNodeToContentTreeNodeInputModelMapper contentTreeNodeToContentTreeNodeInputModelMapper,
                                            ITreeNodeRepository treeNodeRepository, 
											IContentTreeNodeProviderContext contentTreeNodeProviderContext,  
											IContentTreeNodeDisplayViewModelBuilder contentTreeNodeDisplayViewModelBuilder, 
											IRawUrlGetter rawUrlGetter,
											ICommandBus commandBus,
											IGuidGetter guidGetter,
											IContentTreeNodeFileUploadPersister contentTreeNodeFileUploadPersister,
                                            ICurrentUserContext currentUserContext,
                                            ITreeNodeIdToUrl treeNodeIdToUrl,
                                            IGetUrlOfFrontSideWebsite getUrlOfFrontSideWebsite,
                                            IContentTree contentTree,
                                            IContentTreeNodeMetaInformationViewModelBuilder contentTreeNodeMetaInformationViewModelBuilder)
		{
	        this.contentTreeNodeMetaInformationViewModelBuilder = contentTreeNodeMetaInformationViewModelBuilder;
	        this.contentTree = contentTree;
	        this.getUrlOfFrontSideWebsite = getUrlOfFrontSideWebsite;
	        this.treeNodeIdToUrl = treeNodeIdToUrl;
	        this.currentUserContext = currentUserContext;
	        this.contentTreeNodeFileUploadPersister = contentTreeNodeFileUploadPersister;
			this.guidGetter = guidGetter;
			this.commandBus = commandBus;
			this.contentTreeNodeProviderContext = contentTreeNodeProviderContext;
			this.treeNodeRepository = treeNodeRepository;
			this.contentTreeNodeToContentTreeNodeInputModelMapper = contentTreeNodeToContentTreeNodeInputModelMapper;
			this.contentTreePageNodeContext = contentTreePageNodeContext;
		}

		[Authorize]
		public virtual ActionResult Delete(string treeNodeId)
		{
			var treeNodeGuidId = new Guid(treeNodeId);
			commandBus.Send(new DeleteTreeNodeCommand()
			                	{
									AggregateRootId = treeNodeGuidId
			                	});
			commandBus.Send(new DeletePageCommand()
			                	{
									TreeNodeId = treeNodeGuidId,
									AggregateRootId = treeNodeGuidId,
                                    LastModifyBy = currentUserContext.GetCurrentPrincipal().Identity.Name
			                	});
			return new RedirectToRouteResult(new RouteValueDictionary { { "controller", "TreeManager" }, { "action", "Index" }});
		}
		
		[Authorize]
		[HttpPost]
		[ValidateInput(false)]
		public virtual ActionResult Create(ContentTreeNodeInputModel contentTreeNodeInputModel)
		{
			if (string.IsNullOrEmpty(contentTreeNodeInputModel.Action)) contentTreeNodeInputModel.Action = "Index";
			if (ModelState.IsValid == false)
				return View("Modify", new ModifyViewModel()
				                      	{
				                      		ContentTreeNodeInputModel = contentTreeNodeInputModel,
											Action = "Create",
				                      	});

		    var treeNodeId = contentTree.Create(contentTreeNodeInputModel.ParentTreeNodeId, contentTreeNodeInputModel.Type, contentTreeNodeInputModel.ControllerName);
			
			commandBus.Send(new CreatePageCommand()
			                	{
									PageId = guidGetter.GetGuid(),
									TreeNodeId = new Guid(treeNodeId),
			                		Body = contentTreeNodeInputModel.Body,
									HeaderText = contentTreeNodeInputModel.HeaderText,
                                    HeaderImage = contentTreeNodeInputModel.HeaderImage == null ? null : contentTreeNodeInputModel.HeaderImage.Id,
									Name = contentTreeNodeInputModel.Name,
									Sequence = contentTreeNodeInputModel.Sequence,
									UrlSegment = contentTreeNodeInputModel.UrlSegment,
									Type = Type.GetType(contentTreeNodeInputModel.Type),
									Inactive = contentTreeNodeInputModel.Inactive,
									Hidden = contentTreeNodeInputModel.Hidden,
                                    LastModifyBy = currentUserContext.GetCurrentPrincipal().Identity.Name,
			                	});

			if (!string.IsNullOrEmpty(contentTreeNodeInputModel.FormAction))
			{
				if (contentTreeNodeInputModel.FormAction.ToLower() == "save and exit")
					return new RedirectToRouteResult(new RouteValueDictionary()
			                                 	{
			                                 		{"controller", "TreeManager"},
													{"action", "Index"},
			                                 	});
			}
		    return new RedirectToRouteResult(new RouteValueDictionary()
		                                         {
		                                             {"controller", GetType().Name.Replace("Controller", string.Empty)},
		                                             {"action", "Modify"},
		                                             {"TreeNodeId", treeNodeId}
		                                         });
		}

		[Authorize]
        public virtual ActionResult Create(string parentTreeNodeId, string providerType, string controllerName)
		{
			return View("Modify", new ModifyViewModel()
			                      	{
										Action = "Create",
			                      		ContentTreeNodeInputModel = new ContentTreeNodeInputModel()
			                      		                        	{
			                      		                        		ParentTreeNodeId = parentTreeNodeId,
																		Type = providerType,
                                                                        ControllerName = controllerName
			                      		                        	}
			                      	});
		}

		[Authorize]
		[HttpPost]
		[ValidateInput(false)]
		public virtual ActionResult Modify(ContentTreeNodeInputModel contentTreeNodeInputModel)
		{
			contentTreeNodeFileUploadPersister.SaveFilesByTreeNodeIdAndAction(contentTreeNodeInputModel.TreeNodeId, contentTreeNodeInputModel.Action);
			if (ModelState.IsValid == false)
				return View("Modify", new ModifyViewModel() { Action = "Modify", ContentTreeNodeInputModel = contentTreeNodeInputModel });

			var contentTreeNode = contentTreePageNodeContext.GetAllContentTreePageNodes().Where(a => a.Id == contentTreeNodeInputModel.TreeNodeId && a.Action == contentTreeNodeInputModel.Action).FirstOrDefault();
			if (contentTreeNode != null)
			{
				var modifyPageComand = new ModifyPageCommand()
				{
					AggregateRootId = new Guid(contentTreeNodeInputModel.PageId),
					TreeNodeId = new Guid(contentTreeNodeInputModel.TreeNodeId),
					HeaderText = contentTreeNodeInputModel.HeaderText,
					HeaderImage = contentTreeNodeInputModel.HeaderImage == null ? null : contentTreeNodeInputModel.HeaderImage.Id,
					Name = contentTreeNodeInputModel.Name,
					Body = contentTreeNodeInputModel.Body,
					ParentId = contentTreeNodeInputModel.ParentTreeNodeId,
					Sequence = contentTreeNodeInputModel.Sequence,
					UrlSegment = contentTreeNodeInputModel.UrlSegment,
					ActionId = contentTreeNodeInputModel.Action,
					Hidden = contentTreeNodeInputModel.Hidden,
					Inactive = contentTreeNodeInputModel.Inactive,
                    LastModifyBy = currentUserContext.GetCurrentPrincipal().Identity.Name,
                    ControllerName = contentTreeNodeInputModel.ControllerName
				};
				commandBus.Send(modifyPageComand);
			} else {
				commandBus.Send(new CreatePageCommand()
				                	{
				                		TreeNodeId = new Guid(contentTreeNodeInputModel.TreeNodeId),
										PageId = guidGetter.GetGuid(),
										Body = contentTreeNodeInputModel.Body,
										HeaderText = contentTreeNodeInputModel.HeaderText,
										HeaderImage = contentTreeNodeInputModel.HeaderImage == null ? null : contentTreeNodeInputModel.HeaderImage.Id,
										Name = contentTreeNodeInputModel.Name,
										UrlSegment = contentTreeNodeInputModel.UrlSegment,
										Action = contentTreeNodeInputModel.Action,
										Hidden = contentTreeNodeInputModel.Hidden,
										Inactive = contentTreeNodeInputModel.Inactive,
                                        LastModifyBy = currentUserContext.GetCurrentPrincipal().Identity.Name,
				                	});
			}

			if ((!string.IsNullOrEmpty(contentTreeNodeInputModel.FormAction)) && (contentTreeNodeInputModel.FormAction.StartsWith("Publish")))
				commandBus.Send(new PublishPageCommand()
				{
					PageId = new Guid(contentTreeNodeInputModel.PageId)
				});

			if (!string.IsNullOrEmpty(contentTreeNodeInputModel.FormAction))
			{
				if (contentTreeNodeInputModel.FormAction.ToLower() == "save and exit")
					return new RedirectToRouteResult(new RouteValueDictionary()
			                                 	{
			                                 		{"controller", "ContentTree"},
													{"action", "Index"},
			                                 	});
			}

			return new RedirectToRouteResult(new RouteValueDictionary()
			                                 	{
			                                 		{"controller", "ContentTreeNode"},
													{"action", "Modify"},
													{ "treeNodeId", contentTreeNodeInputModel == null ? "0" : contentTreeNodeInputModel.TreeNodeId },
													{ "contentItemId", contentTreeNodeInputModel == null ? "Index" : contentTreeNodeInputModel.Action },
			                                 	});
		}

		[Authorize]
		public virtual ActionResult Modify(string treeNodeId, string contentItemId)
		{
			if (string.IsNullOrEmpty(contentItemId)) contentItemId = "Index";		    

			var contentTreeNode = contentTreePageNodeContext.GetAllContentTreePageNodes().Where(a => a.Id == treeNodeId && a.Action == contentItemId).FirstOrDefault();
			var contentTreeNodeInputModel = contentTreeNode == null ? new ContentTreeNodeInputModel()
			                		                        	{
			                		                        		TreeNodeId = treeNodeId,
																	Action = contentItemId,
			                		                        	}
										: contentTreeNodeToContentTreeNodeInputModelMapper.CreateInstance(contentTreeNode);

			var viewModel = new ModifyViewModel()
			                	{
									Action = "Modify",
			                		ContentTreeNodeInputModel = contentTreeNodeInputModel,
			                	};
			if (string.IsNullOrEmpty(viewModel.ContentTreeNodeInputModel.Action))
				viewModel.ContentTreeNodeInputModel.Action = "Index";

		    viewModel.Url = string.Format("{0}{1}", getUrlOfFrontSideWebsite.GetUrlOfFrontSide(), treeNodeIdToUrl.GetUrlByTreeNodeId(treeNodeId));

			return View("Modify", viewModel);
		}

        [Authorize]
        public virtual ActionResult ManageMetaInformation(string treeNodeId, string contentItemId)
        {
            return View("ManageMetaInformation", contentTreeNodeMetaInformationViewModelBuilder.BuildViewModel(null));
        }

        [Authorize]
        [HttpPost]
        public virtual ActionResult ManageMetaInformation(ContentTreeNodeMetaInformationInputModel contentTreeNodeMetaInformationInputModel)
        {
            if (ModelState.IsValid)
            {
                commandBus.Send(new ModifyPageMetaInformationCommand()
                                        {
                                            MetaDescription = contentTreeNodeMetaInformationInputModel.MetaDescription,
                                            MetaKeywords = contentTreeNodeMetaInformationInputModel.MetaKeywords,
                                            MetaTitle = contentTreeNodeMetaInformationInputModel.MetaTitle,
                                        });
                return new RedirectToRouteResult(new RouteValueDictionary(new Dictionary<string, object>()
                                                                              {
                                                                                  { "controller", typeof(ContentTreeNodeController).Name.Replace("Controller", string.Empty) },
                                                                                  { "action", "ManageMetaInformation" }
                                                                              }));
            }

            return View("ManageMetaInformation", contentTreeNodeMetaInformationViewModelBuilder.BuildViewModel(contentTreeNodeMetaInformationInputModel));
        }

		[Authorize]
		public virtual ActionResult ContentItemNavigation(string treeNodeId)
		{
			var viewModel = new ContentItemNavigationViewModel()
			                	{
			                		TreeNodeId = treeNodeId
			                	};
			var treeNode = treeNodeRepository.GetAll().Where(a => a.TreeNodeId == treeNodeId).FirstOrDefault();
			if (treeNode != null)
			{
			    var provider = contentTreeNodeProviderContext.GetProviderForTreeNode(treeNode);
			    provider.Controller = treeNode.ControllerName;
				viewModel.ContentTreeNodeContentItems = provider.Actions;
			}
			if ((viewModel.ContentTreeNodeContentItems == null) || (viewModel.ContentTreeNodeContentItems.Count() == 0)) return null;
			return View("ContentItemNavigation", viewModel);
		}
	}
}
