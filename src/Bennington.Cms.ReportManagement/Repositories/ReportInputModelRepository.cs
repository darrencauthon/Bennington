using System.Collections.Generic;
using Bennington.Cms.ReportManagement.Models;
using Bennington.Core.Helpers;
using Bennington.Repository;
using Bennington.Repository.Helpers;

namespace Bennington.Cms.ReportManagement.Repositories
{
    public interface IReportInputModelRepository
    {
        IEnumerable<ReportInputModel> GetAll();
        string SaveAndReturnId(ReportInputModel instance);
        ReportInputModel GetById(string id);
        void Delete(string id);
    }

    public class EmailGroupRepository : ObjectStore<ReportInputModel>, IReportInputModelRepository
    {
        public EmailGroupRepository(IXmlFileSerializationHelper xmlFileSerializationHelper, IGetDataPathForType getDataPathForType, IGetValueOfIdPropertyForInstance getValueOfIdPropertyForInstance, IGuidGetter guidGetter, IFileSystem fileSystem) : base(xmlFileSerializationHelper, getDataPathForType, getValueOfIdPropertyForInstance, guidGetter, fileSystem)
        {
        }
    }
}