using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Simple.Data;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Repositories
{
    public interface IContentTreeRepository
    {
        Data.ContentType[] GetAllContentTypes();
        Data.ContentTreeNode[] GetAll();
        Data.ContentAction[] GetAllContentActions();
        void Save(Data.ContentTreeNode instance);
        void Delete(string id);
    }

    public class ContentTreeRepository : IContentTreeRepository
    {
        public Data.ContentAction[] GetAllContentActions()
        {
            var list = new List<Data.ContentAction>();
            list.AddRange(GetDatabase().ContentActions.All().Cast<Data.ContentAction>());
            return list.ToArray();
        }

        public Data.ContentType[] GetAllContentTypes()
        {
            var list = new List<Data.ContentType>();
            list.AddRange(GetDatabase().ContentTypes.All().Cast<Data.ContentType>());
            return list.ToArray();            
        }

        public Data.ContentTreeNode[] GetAll()
        {
            var list = new List<Data.ContentTreeNode>();
            list.AddRange(GetDatabase().ContentTree.All().Cast<Data.ContentTreeNode>());
            return list.ToArray();
        }

        public void Save(Data.ContentTreeNode instance)
        {
            var db = GetDatabase();

            var test = db.ContentTree.FindAllById(instance.Id).FirstOrDefault();
            if (test == null)
                db.ContentTree.Insert(instance);
            else
                db.ContentTree.UpdateById(instance);
        }

        public void Delete(string id)
        {
            GetDatabase().ContentTree.Delete(Id: id);
        }


        private dynamic GetDatabase()
        {
            return Database.OpenConnection(ConfigurationManager.ConnectionStrings["Bennington.ContentTree.Domain.ConnectionString"].ConnectionString);
        }
    }
}