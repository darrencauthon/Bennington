using System;
using System.Collections.Generic;
using AutoMoq;
using Bennington.EmailCommunication.TextProcessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bennington.EmailCommunication.Tests.TextProcessing
{
	[TestClass]
	public class TagFillerServiceTests_AutoFillTagsFromModel
	{
		private AutoMoqer mocker;

		[TestInitialize]
		public void Init()
		{
			mocker = new AutoMoqer();
		}

		[TestMethod]
		public void Calls_FindAndReplaceTags_method_of_IProcessReplaceTagService_for_each_property_of_model_passed_in()
		{
			mocker.Resolve<TagFillerService>().AutoFillTagsFromModel("text", new
																					{
																						Test1 = "1",
																						Test2 = "2"
																					});

			mocker.GetMock<IProcessReplaceTagService>().Verify(a => a.FindAndReplaceTags(It.IsAny<string>(), "Test1", It.IsAny<string>()), Times.Once());
			mocker.GetMock<IProcessReplaceTagService>().Verify(a => a.FindAndReplaceTags(It.IsAny<string>(), "Test2", It.IsAny<string>()), Times.Once());
		}

		[TestMethod]
		public void Calls_FindAndReplaceTags_method_of_IProcessReplaceTagService_with_correct_value_to_replace_for_each_property_of_model_passed_in()
		{
			mocker.Resolve<TagFillerService>().AutoFillTagsFromModel("text", new
			{
				Test1 = "1",
				Test2 = "2"
			});

			mocker.GetMock<IProcessReplaceTagService>().Verify(a => a.FindAndReplaceTags(It.IsAny<string>(), "Test1", "1"), Times.Once());
			mocker.GetMock<IProcessReplaceTagService>().Verify(a => a.FindAndReplaceTags(It.IsAny<string>(), "Test2", "2"), Times.Once());
		}

		[TestMethod]
		public void Calls_FindAndReplaceTags_method_of_IProcessReplaceTagService_with_correct_string_containing_replace_values()
		{
			mocker.Resolve<TagFillerService>().AutoFillTagsFromModel("text", new
			{
				Test1 = "1",
				Test2 = "2"
			});

			mocker.GetMock<IProcessReplaceTagService>().Verify(a => a.FindAndReplaceTags("text", It.IsAny<string>(), It.IsAny<string>()));
		}

		[TestMethod]
		public void Returns_result_built_from_calls_to_FindAndReplaceTags_method_of_IProcessReplaceTagsService()
		{
			var tagFillerService = new TagFillerService(new FakeIProcessReplaceTagService());
			
			var result = tagFillerService.AutoFillTagsFromModel(string.Empty, new { Test = "test", Test2 = "test2" });

			Assert.AreEqual("12", result);
		}

		[TestMethod]
		public void Calls_FindAndReplaceTags_method_of_IProcessReplaceTagService_with_correct_TagToFind_when_running_against_a_model_with_nested_objects()
		{
			mocker.GetMock<IProcessReplaceTagService>().Setup(a => a.FindAndReplaceTags(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
							.Returns((string q, string r, string s) => q);

			mocker.Resolve<TagFillerService>().AutoFillTagsFromModel("text", new TestModel
			{
				NestedTestModel = new NestedTestModel()
									{
										NestedTestModelProperty = "NestedTestModelProperty"
									},
				TestModelProperty = "TestModelProperty"
			});

			mocker.GetMock<IProcessReplaceTagService>().Verify(a => a.FindAndReplaceTags(It.IsAny<string>(), "TestModelProperty", It.IsAny<string>()), Times.Once());
			mocker.GetMock<IProcessReplaceTagService>().Verify(a => a.FindAndReplaceTags(It.IsAny<string>(), "NestedTestModelProperty", It.IsAny<string>()), Times.Once());
		}

		[TestMethod]
		public void Calls_FindAndReplaceTags_method_of_IProcessReplaceTagService_with_properties_which_are_of_type_int()
		{
			mocker.GetMock<IProcessReplaceTagService>().Setup(a => a.FindAndReplaceTags(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
							.Returns((string q, string r, string s) => q);

			mocker.Resolve<TagFillerService>().AutoFillTagsFromModel("text", new 
																				{
																					IntTest = 2,
																				});

			mocker.GetMock<IProcessReplaceTagService>().Verify(a => a.FindAndReplaceTags(It.IsAny<string>(), "IntTest", It.IsAny<string>()), Times.Once());
		}

		[TestMethod]
		public void Calls_FindAndReplaceTags_method_of_IProcessReplaceTagService_with_properties_which_are_of_type_nullable_int()
		{
			mocker.GetMock<IProcessReplaceTagService>().Setup(a => a.FindAndReplaceTags(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
							.Returns((string q, string r, string s) => q);

			mocker.Resolve<TagFillerService>().AutoFillTagsFromModel("text", new
			{
				IntTest = (int?)2,
			});

			mocker.GetMock<IProcessReplaceTagService>().Verify(a => a.FindAndReplaceTags(It.IsAny<string>(), "IntTest", It.IsAny<string>()), Times.Once());
		}

		[TestMethod]
		public void Calls_FindAndReplaceTags_method_of_IProcessReplaceTagService_with_properties_which_are_of_type_decimal()
		{
			mocker.GetMock<IProcessReplaceTagService>().Setup(a => a.FindAndReplaceTags(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
							.Returns((string q, string r, string s) => q);

			mocker.Resolve<TagFillerService>().AutoFillTagsFromModel("text", new
																				{
																					Test = (decimal)2.34,
																				});

			mocker.GetMock<IProcessReplaceTagService>().Verify(a => a.FindAndReplaceTags(It.IsAny<string>(), "Test", It.IsAny<string>()), Times.Once());
		}

		[TestMethod]
		public void Calls_FindAndReplaceTags_method_of_IProcessReplaceTagService_with_properties_which_are_of_type_nullable_decimal()
		{
			mocker.GetMock<IProcessReplaceTagService>().Setup(a => a.FindAndReplaceTags(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
							.Returns((string q, string r, string s) => q);

			mocker.Resolve<TagFillerService>().AutoFillTagsFromModel("text", new
			{
				Test = (decimal?)2.34,
			});

			mocker.GetMock<IProcessReplaceTagService>().Verify(a => a.FindAndReplaceTags(It.IsAny<string>(), "Test", It.IsAny<string>()), Times.Once());
		}

		[TestMethod]
		public void Calls_FindAndReplaceTags_method_of_IProcessReplaceTagService_with_properties_which_are_of_type_double()
		{
			mocker.GetMock<IProcessReplaceTagService>().Setup(a => a.FindAndReplaceTags(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
							.Returns((string q, string r, string s) => q);

			mocker.Resolve<TagFillerService>().AutoFillTagsFromModel("text", new
																				{
																					Test = (double)2.34,
																				});

			mocker.GetMock<IProcessReplaceTagService>().Verify(a => a.FindAndReplaceTags(It.IsAny<string>(), "Test", It.IsAny<string>()), Times.Once());
		}

		[TestMethod]
		public void Calls_FindAndReplaceTags_method_of_IProcessReplaceTagService_with_properties_which_are_of_type_nullable_double()
		{
			mocker.GetMock<IProcessReplaceTagService>().Setup(a => a.FindAndReplaceTags(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
							.Returns((string q, string r, string s) => q);

			mocker.Resolve<TagFillerService>().AutoFillTagsFromModel("text", new
			{
				Test = (double?)2.34,
			});

			mocker.GetMock<IProcessReplaceTagService>().Verify(a => a.FindAndReplaceTags(It.IsAny<string>(), "Test", It.IsAny<string>()), Times.Once());
		}

		[TestMethod]
		public void Calls_FindAndReplaceTags_method_of_IProcessReplaceTagService_with_properties_which_are_of_type_DateTime()
		{
			mocker.GetMock<IProcessReplaceTagService>().Setup(a => a.FindAndReplaceTags(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
							.Returns((string q, string r, string s) => q);

			mocker.Resolve<TagFillerService>().AutoFillTagsFromModel("text", new
																				{
																					Test = new DateTime(2011, 1, 1),
																				});

			mocker.GetMock<IProcessReplaceTagService>().Verify(a => a.FindAndReplaceTags(It.IsAny<string>(), "Test", It.IsAny<string>()), Times.Once());
		}

		[TestMethod]
		public void Calls_FindAndReplaceTags_method_of_IProcessReplaceTagService_with_properties_which_are_of_type_nullable_DateTime()
		{
			mocker.GetMock<IProcessReplaceTagService>().Setup(a => a.FindAndReplaceTags(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
							.Returns((string q, string r, string s) => q);

			mocker.Resolve<TagFillerService>().AutoFillTagsFromModel("text", new
			{
				Test = (DateTime?) new DateTime(2011, 1, 1),
			});

			mocker.GetMock<IProcessReplaceTagService>().Verify(a => a.FindAndReplaceTags(It.IsAny<string>(), "Test", It.IsAny<string>()), Times.Once());
		}

		[TestMethod]
		public void Calls_FindAndReplaceTags_method_of_IProcessReplaceTagService_with_properties_which_are_of_type_nullable_bool()
		{
			mocker.GetMock<IProcessReplaceTagService>().Setup(a => a.FindAndReplaceTags(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
							.Returns((string q, string r, string s) => q);

			mocker.Resolve<TagFillerService>().AutoFillTagsFromModel("text", new
			{
				Test = (bool?) true,
			});

			mocker.GetMock<IProcessReplaceTagService>().Verify(a => a.FindAndReplaceTags(It.IsAny<string>(), "Test", It.IsAny<string>()), Times.Once());
		}

		[TestMethod]
		public void Calls_FindAndReplaceTags_method_of_IProcessReplaceTagService_with_properties_which_are_of_type_bool()
		{
			mocker.GetMock<IProcessReplaceTagService>().Setup(a => a.FindAndReplaceTags(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
							.Returns((string q, string r, string s) => q);

			mocker.Resolve<TagFillerService>().AutoFillTagsFromModel("text", new
			{
				Test = (bool)true,
			});

			mocker.GetMock<IProcessReplaceTagService>().Verify(a => a.FindAndReplaceTags(It.IsAny<string>(), "Test", It.IsAny<string>()), Times.Once());
		}

		[TestMethod]
		public void Does_not_throw_when_a_model_property_has_a_null_value()
		{
			mocker.GetMock<IProcessReplaceTagService>().Setup(a => a.FindAndReplaceTags(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
							.Returns((string q, string r, string s) => q);

			mocker.Resolve<TagFillerService>().AutoFillTagsFromModel("text", new TestModel());
		}

		[TestMethod]
		public void Does_not_throw_when_a_model_property_is_a_DateTime()
		{
			mocker.GetMock<IProcessReplaceTagService>().Setup(a => a.FindAndReplaceTags(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
							.Returns((string q, string r, string s) => q);

			mocker.Resolve<TagFillerService>().AutoFillTagsFromModel("text", new { Test = new DateTime(2011, 1, 1) });
		}

        [TestMethod]
        public void Does_not_throw_when_a_model_property_is_an_array_of_strings()
        {
            mocker.Resolve<TagFillerService>().AutoFillTagsFromModel("text", new { Strings = new string[] {"1", "2"}});
        }

        [TestMethod]
        public void Does_not_throw_when_a_model_property_is_IEnumerable_of_strings()
        {
            mocker.Resolve<TagFillerService>().AutoFillTagsFromModel("text", new FakeInputModel{ Strings = new string[] { "1", "2" } });
        }
	}

    public class FakeInputModel
    {
        public IEnumerable<string> Strings { get; set; }
    }

	public class FakeIProcessReplaceTagService : IProcessReplaceTagService
	{
		private static int _count = 0;

		public string FindAndReplaceTags(string textContainingReplaceTags, string tagToFind, string replaceValue)
		{
			_count++;
			return textContainingReplaceTags += _count.ToString();
		}
	}

	public class TestModel
	{
		public string TestModelProperty {get; set;}
		public NestedTestModel NestedTestModel { get; set; }
	}

	public class NestedTestModel
	{
		public string NestedTestModelProperty {get; set;}
	}
}
