using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using AutoMoq;
using Bennington.Cms.PrincipalProvider.Mappers;
using Bennington.Cms.PrincipalProvider.Models;
using Bennington.Cms.PrincipalProvider.Repositories;
using Bennington.Cms.PrincipalProvider.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bennington.Cms.PrincipalProvider.Tests.Services
{
	[TestClass]
	public class ProcessUserInputModelServiceTests_ProcessAndReturnId
	{
		private AutoMoqer mocker;

		[TestInitialize]
		public void Init()
		{
			mocker = new AutoMoqer();
		}

		[TestMethod]
		public void Calls_SaveAndReturnId_method_of_user_repository_with_result_of_mapper()
		{
			mocker.GetMock<IUserInputModelToUserMapper>()
				.Setup(a => a.CreateInstance(It.Is<UserInputModel>(b => b.Id == "test")))
				.Returns(new User()
				         	{
				         		Id = "test"
				         	});

			mocker.Resolve<ProcessUserInputModelService>()
				.ProcessAndReturnId(new UserInputModel()
				                    	{
				                    		Id = "test",
				                    	});

			mocker.GetMock<IUserRepository>()
				.Verify(a => a.SaveAndReturnId(It.Is<User>(b => b.Id == "test")), Times.Once());
		}

		[TestMethod]
		public void Returns_result_from_SaveAndReturnId_method_of_user_repository()
		{
			mocker.GetMock<IUserRepository>()
				.Setup(a => a.SaveAndReturnId(It.IsAny<User>()))
				.Returns("test");

			var result = mocker.Resolve<ProcessUserInputModelService>().ProcessAndReturnId(new UserInputModel());

			Assert.AreEqual("test", result);
		}

        [TestMethod]
        public void Writes_original_password_to_repo_when_there_is_an_existing_user_and_the_input_model_Password_is_not_set()
        {
            mocker.GetMock<IUserRepository>()
                .Setup(a => a.GetAll())
                .Returns(new User[]
                             {
                                 new User()
                                     {
                                         Id = "test",
                                         Password = "password"
                                     }, 
                             });
            mocker.GetMock<IUserInputModelToUserMapper>()
                .Setup(a => a.CreateInstance(It.Is<UserInputModel>(b => b.Id == "test")))
                .Returns(new User()
                                {
                                    Id = "test"
                                });

            mocker.Resolve<ProcessUserInputModelService>()
                .ProcessAndReturnId(new UserInputModel()
                                            {
                                                Id = "test",
                                            });

            mocker.GetMock<IUserRepository>()
                .Verify(a => a.SaveAndReturnId(It.Is<User>(b => b.Id == "test" && b.Password == "password")), Times.Once());
        }

        [TestMethod]
        public void Writes_new_password_to_repo_when_there_is_an_existing_user_and_the_input_model_Password_is_set()
        {
            mocker.GetMock<IUserRepository>()
                .Setup(a => a.GetAll())
                .Returns(new User[]
                             {
                                 new User()
                                     {
                                         Id = "test",
                                         Password = "password1"
                                     }, 
                             });
            mocker.GetMock<IUserInputModelToUserMapper>()
                .Setup(a => a.CreateInstance(It.Is<UserInputModel>(b => b.Id == "test" && b.Password == "password2")))
                .Returns(new User()
                                {
                                    Id = "test",
                                    Password = "password2",
                                });

            mocker.Resolve<ProcessUserInputModelService>()
                .ProcessAndReturnId(new UserInputModel()
                                            {
                                                Id = "test",
                                                Password = "password2"
                                            });

            mocker.GetMock<IUserRepository>()
                .Verify(a => a.SaveAndReturnId(It.Is<User>(b => b.Id == "test" && b.Password == "password2")), Times.Once());
        }
	}
}
