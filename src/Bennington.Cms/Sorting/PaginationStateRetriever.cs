﻿using System;
using System.Web;

namespace Bennington.Cms.Sorting
{
    public interface IPaginationStateRetriever
    {
        PaginationState GetTheCurrentPaginationState(Type type);
    }

    public class PaginationStateRetriever : IPaginationStateRetriever
    {
        public PaginationState GetTheCurrentPaginationState(Type type)
        {
            return new PaginationState
                       {
                           SortBy = HttpContext.Current.Request.QueryString["sortBy"],
                           PageSize = 25,
                           CurrentPage = 0,
                       };
        }
    }

    public class PaginationState
    {
        public string SortBy { get; set; }

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }
    }
}