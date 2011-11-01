using System.Data.Linq;
using System.Linq;
using System.Transactions;
using Bennington.Content.Data;
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
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<ContentTypeItem>(ct => ct.ContentActionItems);

                dataContext.LoadOptions = loadOptions;

                var contentTypeItems = dataContext.ContentTypeItems.ToList();
                var itemsToDelete = from data in contentTypeItems
                                    where !contentTypes.Any(t => t.Type == data.Type && t.ControllerName == data.ControllerName)
                                    select data;

                var itemsToUpdate = (from data in contentTypeItems
                                     let type = contentTypes.SingleOrDefault(t => t.Type == data.Type && t.ControllerName == data.ControllerName)
                                     where type != null
                                     select new {data, type}).ToList();

                var itemsToInsert = (from type in contentTypes
                                     where !contentTypeItems.Any(t => t.Type == type.Type && t.ControllerName == type.ControllerName)
                                     select CreateContentTypeItem(type)).ToList();

                itemsToUpdate.ForEach(i => UpdateContentTypeItem(i.data, i.type, dataContext));

                dataContext.ContentTypeItems.DeleteAllOnSubmit(itemsToDelete);
                dataContext.ContentTypeItems.InsertAllOnSubmit(itemsToInsert);

                dataContext.SubmitChanges();
                ts.Complete();
            }
        }

        private static void UpdateContentTypeItem(ContentTypeItem contentTypeItem, ContentType contentType, ContentDataContext contentDataContext)
        {
            contentTypeItem.DisplayName = contentType.DisplayName;

            var actionsToDelete = (from item in contentTypeItem.ContentActionItems
                                   where !contentType.Actions.Any(action => action.Action == item.Action)
                                   select item).ToList();

            var actionsToUpdate = (from item in contentTypeItem.ContentActionItems
                                   let action = contentType.Actions.SingleOrDefault(a => a.Action == item.Action)
                                   where action != null
                                   select new {item, action}).ToList();

            var actionsToInsert = (from action in contentType.Actions
                                   where !contentTypeItem.ContentActionItems.Any(a => a.Action == action.Action)
                                   select new ContentActionItem {Action = action.Action, ContentType = contentTypeItem.Type, DisplayName = action.DisplayName}).ToList();

            contentDataContext.ContentActionItems.DeleteAllOnSubmit(actionsToDelete);
            actionsToUpdate.ForEach(a => { a.item.DisplayName = a.action.DisplayName; });
            contentTypeItem.ContentActionItems.AddRange(actionsToInsert);
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