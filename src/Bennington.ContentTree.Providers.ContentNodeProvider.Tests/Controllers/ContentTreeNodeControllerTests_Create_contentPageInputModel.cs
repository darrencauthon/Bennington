using System;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;
using AutoMoq;
using Bennington.ContentTree.Domain.Commands;
using Bennington.ContentTree.Providers.ContentNodeProvider.Context;
using Bennington.ContentTree.Providers.ContentNodeProvider.Controllers;
using Bennington.ContentTree.Providers.ContentNodeProvider.Helpers;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;
using Bennington.Core;
using Bennington.Core.Helpers;
using Bennington.FileUploadHandling.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimpleCqrs.Commanding;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Tests.Controllers
{
	[TestClass]
	public class ContentTreeNodeControllerTests_Create_ContentPageInputModel
	{
		private AutoMoqer mocker;

		[TestInitialize]
		public void Init()
		{
			mocker = new AutoMoqer();

            mocker.GetMock<ICurrentUserContext>()
                .Setup(a => a.GetCurrentPrincipal())
                .Returns(new GenericPrincipal(new GenericIdentity("test"), new string[] { }));
            mocker.GetMock<IContentTree>()
                .Setup(a => a.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new Guid().ToString());
		}

		[TestMethod]
		public void Returns_RedirectResult_when_ModelState_is_valid()
		{
		    var guid = Guid.NewGuid().ToString();
            mocker.GetMock<IContentTree>()
                .Setup(a => a.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(guid);
            var contentTreeNodeInputModel = new ContentTreeNodeInputModel()
			                                        {
				                                        ParentTreeNodeId = "2",
				                                        Type = typeof(string).AssemblyQualifiedName
			                                        };

            var result = mocker.Resolve<ContentTreeNodeController>().Create(contentTreeNodeInputModel);

            Assert.AreEqual(guid, (result as RedirectToRouteResult).RouteValues["TreeNodeId"].ToString());
            Assert.AreEqual(typeof(ContentTreeNodeController).Name.Replace("Controller", string.Empty), (result as RedirectToRouteResult).RouteValues["controller"].ToString());
            Assert.AreEqual("Modify", (result as RedirectToRouteResult).RouteValues["action"].ToString());
		}

		[TestMethod]
		public void Returns_RedirectToRouteResult_when_ModelState_is_valid_and_FormAction_is_save_and_exit()
		{
			var contentTreeNodeInputModel = new ContentTreeNodeInputModel()
			{
				ParentTreeNodeId = "2",
				Type = typeof(string).AssemblyQualifiedName,
				FormAction = "save and exit",
			};

			var contentTreeNodeController = mocker.Resolve<ContentTreeNodeController>();
			var result = contentTreeNodeController.Create(contentTreeNodeInputModel);

			Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
		}

		[TestMethod]
		public void Returns_view_model_with_input_model_set_to_same_input_model_that_was_passed_in_when_ModelState_is_invalid()
		{
			var contentTreeNodeInputModel = new ContentTreeNodeInputModel()
			{
				ParentTreeNodeId = "2",
			};

			var contentTreeNodeController = mocker.Resolve<ContentTreeNodeController>();
			contentTreeNodeController.ModelState.AddModelError("key", "error");
			var result = contentTreeNodeController.Create(contentTreeNodeInputModel);

			Assert.AreEqual(contentTreeNodeInputModel, ((ModifyViewModel)((ViewResult)result).ViewData.Model).ContentTreeNodeInputModel);
		}

		[TestMethod]
		public void Calls_Create_method_of_IContentTreeNodeContext_when_ModelState_is_valid()
		{
            mocker.GetMock<IContentTree>().Setup(a => a.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new Guid().ToString());
			var contentTreeNodeInputModel = new ContentTreeNodeInputModel()
			                            	{
			                            		ParentTreeNodeId = "2",
												Type = typeof(string).AssemblyQualifiedName
			                            	};

			mocker.Resolve<ContentTreeNodeController>().Create(contentTreeNodeInputModel);

            mocker.GetMock<IContentTree>().Verify(a =>a.Create(contentTreeNodeInputModel.ParentTreeNodeId, contentTreeNodeInputModel.Type, contentTreeNodeInputModel.ControllerName), Times.Once());
		}

		[TestMethod]
		public void Does_not_call_Create_method_of_IContentTreeNodeContext_when_ModelState_is_invalid()
		{
			var contentTreeNodeInputModel = new ContentTreeNodeInputModel()
			{
				ParentTreeNodeId = "2",
				Type = typeof(string).AssemblyQualifiedName
			};
			var contentTreeNodeController = mocker.Resolve<ContentTreeNodeController>();
			contentTreeNodeController.ModelState.AddModelError("key", "error");
			contentTreeNodeController.Create(contentTreeNodeInputModel);

            mocker.GetMock<IContentTree>().Verify(a => a.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
		}

		[TestMethod]
		public void Sets_view_model_action_to_create_when_ModelState_is_not_valid()
		{
			var contentTreeNodeController = mocker.Resolve<ContentTreeNodeController>();
			contentTreeNodeController.ModelState.AddModelError("key", "error");
			var result = contentTreeNodeController.Create(new ContentTreeNodeInputModel());

			Assert.AreEqual("Create", ((ModifyViewModel)((ViewResult)result).ViewData.Model).Action);
		}

		[TestMethod]
		public void Sends_CreatePageCommand_command_when_ModelState_is_valid()
		{
			var contentTreeNodeInputModel = new ContentTreeNodeInputModel()
														{
															ParentTreeNodeId = "2",
															Type = typeof(string).AssemblyQualifiedName
														};

			mocker.Resolve<ContentTreeNodeController>().Create(contentTreeNodeInputModel);

			mocker.GetMock<ICommandBus>().Verify(a => a.Send(It.IsAny<CreatePageCommand>()), Times.Once());
		}

		[TestMethod]
		public void Sends_CreatePageCommand_command_with_correct_Body_when_ModelState_is_valid()
		{
			var contentTreeNodeInputModel = new ContentTreeNodeInputModel()
														{
															ParentTreeNodeId = "2",
															Body = "content",
															Type = typeof(string).AssemblyQualifiedName
														};

			mocker.Resolve<ContentTreeNodeController>().Create(contentTreeNodeInputModel);

			mocker.GetMock<ICommandBus>().Verify(a => a.Send(It.Is<CreatePageCommand>(b => b.Body == contentTreeNodeInputModel.Body)), Times.Once());
		}

		[TestMethod]
		public void Sends_CreatePageCommand_command_with_correct_HeaderText_when_ModelState_is_valid()
		{
			var contentTreeNodeInputModel = new ContentTreeNodeInputModel()
														{
															ParentTreeNodeId = "2",
															Body = "content",
															HeaderText = "header text",
															Name = "name",
															Type = typeof(string).AssemblyQualifiedName
														};

			mocker.Resolve<ContentTreeNodeController>().Create(contentTreeNodeInputModel);

			mocker.GetMock<ICommandBus>().Verify(a => a.Send(It.Is<CreatePageCommand>(b => b.HeaderText == contentTreeNodeInputModel.HeaderText)), Times.Once());
		}

		[TestMethod]
		public void Sends_CreatePageCommand_command_with_correct_Sequence_when_ModelState_is_valid()
		{
			var contentTreeNodeInputModel = new ContentTreeNodeInputModel()
														{
															ParentTreeNodeId = "2",
															Body = "content",
															Name = "header text",
															Sequence = 100,
															Type = typeof(string).AssemblyQualifiedName
														};

			mocker.Resolve<ContentTreeNodeController>().Create(contentTreeNodeInputModel);

			mocker.GetMock<ICommandBus>().Verify(a => a.Send(It.Is<CreatePageCommand>(b => b.Sequence == contentTreeNodeInputModel.Sequence)), Times.Once());
		}

		[TestMethod]
		public void Sends_CreatePageCommand_command_with_correct_UrlSegment_when_ModelState_is_valid()
		{
			var contentTreeNodeInputModel = new ContentTreeNodeInputModel()
														{
															ParentTreeNodeId = "2",
															Body = "content",
															Name = "header text",
															Sequence = 100,
															UrlSegment = "url segment",
															Type = typeof(string).AssemblyQualifiedName
														};

			mocker.Resolve<ContentTreeNodeController>().Create(contentTreeNodeInputModel);

			mocker.GetMock<ICommandBus>().Verify(a => a.Send(It.Is<CreatePageCommand>(b => b.UrlSegment == contentTreeNodeInputModel.UrlSegment)), Times.Once());
		}

		[TestMethod]
		public void Sends_CreatePageCommand_command_with_correct_Type_when_ModelState_is_valid()
		{
			var contentTreeNodeInputModel = new ContentTreeNodeInputModel()
														{
															ParentTreeNodeId = "2",
															Body = "content",
															Name = "header text",
															Sequence = 100,
															UrlSegment = "url segment",
															Type = typeof(string).AssemblyQualifiedName,
														};

			mocker.Resolve<ContentTreeNodeController>().Create(contentTreeNodeInputModel);

			mocker.GetMock<ICommandBus>().Verify(a => a.Send(It.Is<CreatePageCommand>(b => b.Type == Type.GetType(contentTreeNodeInputModel.Type))), Times.Once());
		}

		[TestMethod]
		public void Sends_CreatePageCommand_command_with_PageId_set_from_IGuidGetter_when_ModelState_is_valid()
		{
			var guid = new Guid("66666666-6969-6969-6969-666666666666");
			mocker.GetMock<IGuidGetter>().Setup(a => a.GetGuid()).Returns(guid);
			var contentTreeNodeInputModel = new ContentTreeNodeInputModel()
														{
															ParentTreeNodeId = "2",
															Body = "content",
															Name = "header text",
															Sequence = 100,
															UrlSegment = "url segment",
															Type = "type",
														};

			mocker.Resolve<ContentTreeNodeController>().Create(contentTreeNodeInputModel);

			mocker.GetMock<ICommandBus>().Verify(a => a.Send(It.Is<CreatePageCommand>(b => b.PageId == guid)), Times.Once());
		}

		[TestMethod]
		public void Sends_CreatePageCommand_command_with_TreeNodeId_set_to_guid_returned_from_CreateTreeNodeAndReturnTreeNodeId_method_of_IContentTreeNodeContext()
		{
			var guid = new Guid("66666666-6969-6969-6969-666666666666");
		    mocker.GetMock<IContentTree>()
                .Setup(a => a.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
		        .Returns(guid.ToString());
            var contentTreeNodeInputModel = new ContentTreeNodeInputModel()
														{
															ParentTreeNodeId = "2",
															Body = "content",
															Name = "header text",
															Sequence = 100,
															UrlSegment = "url segment",
															Type = "type",
														};

			mocker.Resolve<ContentTreeNodeController>().Create(contentTreeNodeInputModel);

			mocker.GetMock<ICommandBus>().Verify(a => a.Send(It.Is<CreatePageCommand>(b => b.TreeNodeId == guid)), Times.Once());
		}

		[TestMethod]
		public void Sends_CreatePageCommand_command_with_correct_Hidden_property_value_when_ModelState_is_valid()
		{
			var contentTreeNodeInputModel = new ContentTreeNodeInputModel()
			{
				ParentTreeNodeId = "2",
				Type = typeof(string).AssemblyQualifiedName,
				Hidden = true,
			};

			mocker.Resolve<ContentTreeNodeController>().Create(contentTreeNodeInputModel);

			mocker.GetMock<ICommandBus>().Verify(a => a.Send(It.Is<CreatePageCommand>(b => b.Hidden == contentTreeNodeInputModel.Hidden)), Times.Once());
		}

		[TestMethod]
		public void Sends_CreatePageCommand_command_with_correct_Active_property_value_when_ModelState_is_valid()
		{
			var contentTreeNodeInputModel = new ContentTreeNodeInputModel()
			{
				ParentTreeNodeId = "2",
				Type = typeof(string).AssemblyQualifiedName,
				Inactive = true,
			};

			mocker.Resolve<ContentTreeNodeController>().Create(contentTreeNodeInputModel);

			mocker.GetMock<ICommandBus>().Verify(a => a.Send(It.Is<CreatePageCommand>(b => b.Inactive == contentTreeNodeInputModel.Inactive)), Times.Once());
		}

		[TestMethod]
		public void Sends_CreatePageCommand_command_with_correct_HeaderImage_property_value_when_ModelState_is_valid()
		{
			var contentTreeNodeInputModel = new ContentTreeNodeInputModel()
			{
				ParentTreeNodeId = "2",
				Type = typeof(string).AssemblyQualifiedName,
				HeaderImage = new FileUploadInputModel()
				                  {
				                      Id = "test"
				                  }
			};

			mocker.Resolve<ContentTreeNodeController>().Create(contentTreeNodeInputModel);

			mocker.GetMock<ICommandBus>().Verify(a => a.Send(It.Is<CreatePageCommand>(b => b.HeaderImage == contentTreeNodeInputModel.HeaderImage.Id)), Times.Once());
		}

        [TestMethod]
        public void Sends_CreatePageCommand_command_with_LastModifyBy_property_set_when_ModelState_is_valid()
        {
            var contentTreeNodeInputModel = new ContentTreeNodeInputModel()
                                                {
                                                    ParentTreeNodeId = "2",
                                                    Type = typeof(string).AssemblyQualifiedName,
                                                };

            mocker.Resolve<ContentTreeNodeController>().Create(contentTreeNodeInputModel);

            mocker.GetMock<ICommandBus>()
                .Verify(a => a.Send(It.Is<CreatePageCommand>(b => b.LastModifyBy == "test")), Times.Once());
        }

		[TestMethod]
		public void Does_not_send_CreatePageCommand_command_when_ModelState_is_not_valid()
		{
			var contentTreeNodeInputModel = new ContentTreeNodeInputModel()
														{
															ParentTreeNodeId = "2",
														};

			var contentTreeNodeController = mocker.Resolve<ContentTreeNodeController>();
			contentTreeNodeController.ModelState.AddModelError("key", "error");
			contentTreeNodeController.Create(contentTreeNodeInputModel);

			mocker.GetMock<ICommandBus>().Verify(a => a.Send(It.IsAny<CreatePageCommand>()), Times.Never());
		}
	}
}
