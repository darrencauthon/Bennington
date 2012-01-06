using AutoMoq;
using Bennington.EmailCommunication.TextProcessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bennington.EmailCommunication.Tests.TextProcessing
{
	[TestClass]
	public class ProcessReplaceTagServiceTests_FindAndReplaceTags
	{
		private AutoMoqer mocker;

		[TestInitialize]
		public void Init()
		{
			mocker = new AutoMoqer();
		}

		[TestMethod]
		public void Replace_lower_case_tags()
		{
			var result = mocker.Resolve<ProcessReplaceTagService>().FindAndReplaceTags("text [:replace:]", "Replace", "zzz");

			Assert.AreEqual("text zzz", result);
		}

		[TestMethod]
		public void Replace_upper_case_tags()
		{
			var result = mocker.Resolve<ProcessReplaceTagService>().FindAndReplaceTags("text [:REPLACE:]", "Replace", "zzz");

			Assert.AreEqual("text zzz", result);
		}

		[TestMethod]
		public void Replace_mixed_case_tags()
		{
			var result = mocker.Resolve<ProcessReplaceTagService>().FindAndReplaceTags("text [:RePLaCe:]", "Replace", "zzz");

			Assert.AreEqual("text zzz", result);
		}

		[TestMethod]
		public void Returns_empty_string_when_passed_null_textContainingReplaceTags()
		{
			var result = mocker.Resolve<ProcessReplaceTagService>().FindAndReplaceTags(null, "Replace", "zzz");

			Assert.AreEqual(string.Empty, result);
		}

		[TestMethod]
		public void Returns_original_text_when_passed_null_tagToFind()
		{
			var result = mocker.Resolve<ProcessReplaceTagService>().FindAndReplaceTags("123456", null, "zzz");

			Assert.AreEqual("123456", result);
		}

		[TestMethod]
		public void Returns_original_text_with_blanks_set_for_replace_tags_when_passed_null_replace_value()
		{
			var result = mocker.Resolve<ProcessReplaceTagService>().FindAndReplaceTags("123[:test:]456", "test", null);

			Assert.AreEqual("123456", result);
		}

		[TestMethod]
		public void Correctly_replaces_when_passed_null_as_replace_value_argument()
		{
			var result = mocker.Resolve<ProcessReplaceTagService>().FindAndReplaceTags("1[:tag:]2", "tag", null);

			Assert.AreEqual("12", result);
		}


		[TestMethod]
		public void Correctly_replaces_very_short_strings()
		{
			var result = mocker.Resolve<ProcessReplaceTagService>().FindAndReplaceTags("1[:a:]2", "a", "x");

			Assert.AreEqual("1x2", result);
		}

		[TestMethod]
		public void Correctly_replaces_when_there_are_multiple_instances_of_tag()
		{
			var result = mocker.Resolve<ProcessReplaceTagService>().FindAndReplaceTags("1[:test:]23456[:test:]789", "test", "R");

			Assert.AreEqual("1R23456R789", result);
		}

		[TestMethod]
		public void Correctly_replaces_when_passed_a_very_long_replace_value()
		{
			var result = mocker.Resolve<ProcessReplaceTagService>().FindAndReplaceTags("ABC[:test:]DEF", "test", "123456789123456789");

			Assert.AreEqual("ABC123456789123456789DEF", result);
		}
	}
}
