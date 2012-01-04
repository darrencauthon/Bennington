using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bennington.Core.Helpers;
using Bennington.EmailCommunication.Mappers;
using Bennington.EmailCommunication.Models;
using Bennington.EmailCommunication.Repositories;

namespace Bennington.EmailCommunication.Services
{
    public interface IEmailGroupInputModelProcessingService
    {
        string SaveAndReturnId(EmailGroupInputModel form);
    }

    public class EmailGroupInputModelProcessingService : IEmailGroupInputModelProcessingService
    {
        private readonly IEmailGroupRepository emailGroupRepository;
        private readonly IEmailGroupInputModelToEmailGroupMapper emailGroupInputModelToEmailGroupMapper;
        private readonly IGetCurrentDateTime getCurrentDateTime;

        public EmailGroupInputModelProcessingService(IEmailGroupRepository emailGroupRepository,
                                                     IEmailGroupInputModelToEmailGroupMapper emailGroupInputModelToEmailGroupMapper,
                                                     IGetCurrentDateTime getCurrentDateTime)
        {
            this.getCurrentDateTime = getCurrentDateTime;
            this.emailGroupInputModelToEmailGroupMapper = emailGroupInputModelToEmailGroupMapper;
            this.emailGroupRepository = emailGroupRepository;
        }

        public string SaveAndReturnId(EmailGroupInputModel form)
        {
            var emailGroup = emailGroupRepository.GetAll().Where(a => a.Id == form.Id).FirstOrDefault();
            if (emailGroup != null) emailGroupInputModelToEmailGroupMapper.LoadIntoInstance(form, emailGroup ?? new EmailGroup());
            else
                emailGroup = emailGroupInputModelToEmailGroupMapper.CreateInstance(form);

            emailGroup.Id = emailGroup.Id ?? Guid.NewGuid().ToString();
            emailGroup.CreateDate = emailGroup.CreateDate == DateTime.MinValue ? getCurrentDateTime.Now() : emailGroup.CreateDate;
            emailGroup.LastModifyDate = getCurrentDateTime.Now();
            emailGroupRepository.SaveAndReturnId(emailGroup);
            return emailGroup.Id;
        }
    }
}