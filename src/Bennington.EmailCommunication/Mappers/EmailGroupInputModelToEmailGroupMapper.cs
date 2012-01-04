using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapperAssist;
using Bennington.EmailCommunication.Models;

namespace Bennington.EmailCommunication.Mappers
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
            configuration.CreateMap<EmailGroupInputModel, EmailGroup>()
                    .ForMember(a => a.CreateDate, b => b.Ignore())
                    .ForMember(a => a.LastModifyDate, b => b.Ignore())
                ;
        }
    }
}