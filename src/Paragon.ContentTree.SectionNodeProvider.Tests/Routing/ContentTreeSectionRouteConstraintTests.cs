﻿using System.Web.Routing;
using AutoMoq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Paragon.ContentTree.Contexts;
using Paragon.ContentTree.Models;
using Paragon.ContentTree.SectionNodeProvider.Routing;

namespace Paragon.ContentTree.SectionNodeProvider.Tests.Routing
{
    [TestClass]
    public class ContentTreeSectionRouteConstraintTests
    {
        private AutoMoqer mocker;

        [TestInitialize]
        public void SetupMocksForAllTests()
        {
            mocker = new AutoMoqer();

			mocker.GetMock<ITreeNodeSummaryContext>().Setup(a => a.GetChildren(Constants.RootNodeId))
				.Returns(new TreeNodeSummary[]
				         	{
				         		new TreeNodeSummary()
				         			{
										Id = "1",
				         				UrlSegment = "rootsegment1",
										Type = typeof(SectionNodeProvider).AssemblyQualifiedName,
				         			},
 								new TreeNodeSummary()
				         			{
										Id = "2",
				         				UrlSegment = "rootsegment2",
										Type = typeof(SectionNodeProvider).AssemblyQualifiedName,
				         			},
								new TreeNodeSummary()
				         			{
										Id = "6",
				         				UrlSegment = "rootsegment3",
										Type = typeof(SectionNodeProvider).AssemblyQualifiedName,
				         			}, 
							});


			mocker.GetMock<ITreeNodeSummaryContext>().Setup(a => a.GetChildren("2"))
					.Returns(new TreeNodeSummary[]
				         	{
								new TreeNodeSummary()
				         		{
									Id = "3",
				         			UrlSegment = "nestLevel1Segment1",
									ParentTreeNodeId = "2",
									Type = typeof(SectionNodeProvider).AssemblyQualifiedName,
				         		},
								new TreeNodeSummary()
				         		{
									Id = "4",
				         			UrlSegment = "nestLevel1Segment2",
									ParentTreeNodeId = "2",
									Type = typeof(SectionNodeProvider).AssemblyQualifiedName,
				         		},
								new TreeNodeSummary()
				         		{
									Id = "5",
				         			UrlSegment = "nestLevel1Segment3",
									ParentTreeNodeId = "2",
									Type = typeof(SectionNodeProvider).AssemblyQualifiedName,
				         		},
							});

			mocker.GetMock<ITreeNodeSummaryContext>().Setup(a => a.GetChildren("4"))
					.Returns(new TreeNodeSummary[]
				         	{
								new TreeNodeSummary()
				         		{
									Id = "7",
				         			UrlSegment = "nestLevel2Segment1",
									ParentTreeNodeId = "4",
									Type = typeof(SectionNodeProvider).AssemblyQualifiedName,
				         		},
								new TreeNodeSummary()
				         		{
									Id = "8",
				         			UrlSegment = "nestLevel2Segment2",
									ParentTreeNodeId = "4",
									Type = typeof(SectionNodeProvider).AssemblyQualifiedName,
				         		},
							});
        }

		[TestMethod]
		public void Returns_true_for_matching_node_first_level_deep_in_content_tree()
		{
			var url = "/rootsegment1";
			
			var result = mocker.Resolve<ContentTreeSectionRouteConstraint>().Match(null, null, null, GetRouteValueDictionaryForUrl(url), new RouteDirection());

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void Returns_false_for_a_node__that_doesnt_match_first_level_deep_in_content_tree()
		{
			var url = "/zzzz";

			var result = mocker.Resolve<ContentTreeSectionRouteConstraint>().Match(null, null, null, GetRouteValueDictionaryForUrl(url), new RouteDirection());

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Returns_true_for_matching_node_second_level_deep_in_content_tree()
		{
			var url = "/rootsegment2/nestLevel1Segment1";

			var result = mocker.Resolve<ContentTreeSectionRouteConstraint>().Match(null, null, null, GetRouteValueDictionaryForUrl(url), new RouteDirection());

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void Returns_false_for_a_node_that_doesnt_match_second_level_deep_in_content_tree()
		{
			var url = "/zzzz/rootsegment1";

			var result = mocker.Resolve<ContentTreeSectionRouteConstraint>().Match(null, null, null, GetRouteValueDictionaryForUrl(url), new RouteDirection());

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Returns_true_for_matching_node_third_level_deep_in_content_tree()
		{
			var url = "/rootsegment2/nestLevel1Segment2/nestLevel2Segment1";

			var result = mocker.Resolve<ContentTreeSectionRouteConstraint>().Match(null, null, null, GetRouteValueDictionaryForUrl(url), new RouteDirection());

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void Returns_false_for_a_node_that_doesnt_match_third_level_deep_in_content_tree()
		{
			var url = "/zzzz/bbbb/aaaa";

			var result = mocker.Resolve<ContentTreeSectionRouteConstraint>().Match(null, null, null, GetRouteValueDictionaryForUrl(url), new RouteDirection());

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Returns_false_for_a_node_that_is_not_a_SectionNodeProvider_type()
		{
			var url = "/rootsegment1";

			mocker.GetMock<ITreeNodeSummaryContext>().Setup(a => a.GetChildren(Constants.RootNodeId))
				.Returns(new TreeNodeSummary[]
				         				{
				         					new TreeNodeSummary()
				         						{
													Id = "1",
				         							UrlSegment = "rootsegment1",
													Type = "some type"
				         						},
										});


			var result = mocker.Resolve<ContentTreeSectionRouteConstraint>().Match(null, null, null, GetRouteValueDictionaryForUrl(url), new RouteDirection());

			Assert.IsFalse(result);
		}

		private static RouteValueDictionary GetRouteValueDictionaryForUrl(string url)
        {
			var routeValueDictionary = new RouteValueDictionary();
			var count = 0;
			foreach (var segment in url.Split('/'))
			{
				if (!string.IsNullOrEmpty(segment))
					routeValueDictionary.Add(string.Format("nodesegment-{0}", count), segment);
				count++;
			}
			return routeValueDictionary;
        }
    }
}