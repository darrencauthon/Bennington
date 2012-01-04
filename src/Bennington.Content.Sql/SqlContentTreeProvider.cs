using System;
using System.Linq;
using System.Runtime.Caching;
using Bennington.Content.Data;
using Bennington.Content.Sql.Data;
using Bennington.Core.Caching;

namespace Bennington.Content.Sql
{
    public class SqlContentTreeProvider : IContentTreeProvider
    {
        private readonly string connectionString;
        private readonly InvalidateCacheEndpoint cacheEndpoint;
        private readonly ObjectCache contentTreeCache = MemoryCache.Default;
        private const string ContentTreeCacheKey = "contentTreeItems";

        public SqlContentTreeProvider(string connectionString, Uri invalidateCacheUri)
        {
            ContentChanged += (sender, e) => { };
            this.connectionString = connectionString;
            cacheEndpoint = new InvalidateCacheEndpoint(invalidateCacheUri, InvalidateCache);
            cacheEndpoint.Open();
        }

        private void InvalidateCache(string cacheKey)
        {
            contentTreeCache.Remove(ContentTreeCacheKey);
            ContentChanged(this, new EventArgs());
        }

        public event EventHandler<EventArgs> ContentChanged;

        public void Refresh()
        {
            InvalidateCache(string.Empty);
        }

        public ContentTree GetContentTree()
        {
            using(var dataContext = new ContentDataContext(connectionString))
            {
                var contentTreeItems = contentTreeCache.Get(ContentTreeCacheKey) as ContentTreeItem[];
                if (contentTreeItems == null)
                {
                    contentTreeItems = dataContext.ContentTreeItems.ToArray();
                    contentTreeCache.Set(ContentTreeCacheKey, contentTreeItems, new DateTimeOffset(DateTime.Now.AddHours(1)));
                }

                var nodes = (from contentTreeItem in contentTreeItems
                             select new ContentNode
                                        {
                                            Action = contentTreeItem.Action,
                                            Controller = contentTreeItem.Controller,
                                            Id = contentTreeItem.Id,
                                            ParentId = contentTreeItem.ParentId,
                                            Segment = contentTreeItem.Segment,
                                            ActionId = contentTreeItem.ActionId,
                                            TreeNodeId = contentTreeItem.TreeNodeId,
                                        });

                return ContentTree.BuildTree(nodes);
            }
        }

        public void Dispose()
        {
            cacheEndpoint.Dispose();
        }
    }
}