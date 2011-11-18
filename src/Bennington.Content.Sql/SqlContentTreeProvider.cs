using System;
using System.Linq;
using Bennington.Content.Data;
using Bennington.Content.Sql.Data;
using Bennington.Core.Caching;

namespace Bennington.Content.Sql
{
    public class SqlContentTreeProvider : IContentTreeProvider
    {
        private readonly string connectionString;
        private readonly InvalidateCacheEndpoint cacheEndpoint;

        public SqlContentTreeProvider(string connectionString, Uri invalidateCacheUri)
        {
            ContentChanged += (sender, e) => { };
            this.connectionString = connectionString;
            cacheEndpoint = new InvalidateCacheEndpoint(invalidateCacheUri, InvalidateCache);
            cacheEndpoint.Open();
        }

        private void InvalidateCache(string cacheKey)
        {
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
                var nodes = (from contentTreeItem in dataContext.ContentTreeItems
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