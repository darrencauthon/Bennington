using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Bennington.Content.Sql.Data;

namespace Bennington.Content.Sql
{
    public class SqlContentTypeRegistry : IContentTypeRegistry
    {
        private readonly string connectionString;

        public SqlContentTypeRegistry(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Save(params ContentType[] contentTypes)
        {
            using(var ts = new TransactionScope())
            using(var dataContext = new ContentDataContext(connectionString))
            {
                dataContext.ExecuteCommand("Delete From ContentActions");
                dataContext.ExecuteCommand("Delete From ContentTypes");
                dataContext.ContentTypeItems.InsertAllOnSubmit(ConvertToContentTypeItems(contentTypes));
                dataContext.SubmitChanges();
                ts.Complete();
            }
        }

        private static IEnumerable<ContentTypeItem> ConvertToContentTypeItems(IEnumerable<ContentType> contentTypes)
        {
            return (from contentType in contentTypes
                    select CreateContentTypeItem(contentType));
        }

        private static ContentTypeItem CreateContentTypeItem(ContentType contentType)
        {
            var contentTypeItem = new ContentTypeItem {Type = contentType.Type, DisplayName = contentType.DisplayName, ControllerName = contentType.ControllerName};
            var contentActionItems = contentType.Actions.Select(action => new ContentActionItem {Action = action.Action, ContentType = contentType.Type, DisplayName = action.DisplayName});
            contentTypeItem.ContentActionItems.AddRange(contentActionItems);
            return contentTypeItem;
        }
    }
}