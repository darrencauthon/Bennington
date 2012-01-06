using AutoMapperAssist;
using Bennington.EmailCommunication.Models;
using Bennington.EmailCommunicationManagement.Models;

namespace Bennington.EmailCommunicationManagement.Mappers
{
    public interface IEmailGroupInputModelToEmailGroupMapper
    {
        EmailGroup CreateInstance(EmailGroupInputModel source);
        void LoadIntoInstance(EmailGroupInputModel source, EmailGroup destination);
    }

    public class EmailGroupInputModelToEmailGroupMapper : Mapper<EmailGroupInputModel, EmailGroup>, IEmailGroupInputModelToEmailGroupMapper
    {
        public override void DefineMap(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<EmailInputModel, EmailModel>()
                ;

            configuration.CreateMap<EmailGroupInputModel, EmailGroup>()
                    .ForMember(a => a.CreateDate, b => b.Ignore())
                    .ForMember(a => a.LastModifyDate, b => b.Ignore())
                    .ForMember(a => a.EmailModels, b => b.MapFrom(c => c.EmailInputModels))
                ;
        }
    }
}