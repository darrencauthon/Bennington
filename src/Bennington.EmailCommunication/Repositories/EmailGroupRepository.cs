using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bennington.Core.Helpers;
using Bennington.EmailCommunication.Models;
using Bennington.Repository;
using Bennington.Repository.Helpers;

namespace Bennington.EmailCommunication.Repositories
{
    public interface IEmailGroupRepository
    {
        IEnumerable<EmailGroup> GetAll();
        string SaveAndReturnId(EmailGroup instance);
        EmailGroup GetById(string id);
        void Delete(string id);
    }

    public class EmailGroupRepository : ObjectStore<EmailGroup>, IEmailGroupRepository
    {
        public EmailGroupRepository(IXmlFileSerializationHelper xmlFileSerializationHelper, IGetDataPathForType getDataPathForType, IGetValueOfIdPropertyForInstance getValueOfIdPropertyForInstance, IGuidGetter guidGetter, IFileSystem fileSystem) : base(xmlFileSerializationHelper, getDataPathForType, getValueOfIdPropertyForInstance, guidGetter, fileSystem)
        {
        }
    }
}