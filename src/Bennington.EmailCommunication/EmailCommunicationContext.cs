using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bennington.EmailCommunication.Mappers;
using Bennington.EmailCommunication.Models;
using Bennington.EmailCommunication.Repositories;
using Bennington.EmailCommunication.TextProcessing;

namespace Bennington.EmailCommunication
{
    public interface IEmailManagementContext
    {
        IEnumerable<MailMessageWithAnId> GetEmailsFromEmailGroup(string emailGroupEngineerId, object emailViewModel);
    }

    public class EmailCommunicationContext : IEmailManagementContext
    {
        private readonly IEmailGroupRepository emailGroupRepository;
        private readonly IEmailModelToMailMessageWithAnIdMapper emailModelToMailMessageWithAnIdMapper;
        private readonly ITagFillerService tagFillerService;

        public EmailCommunicationContext(IEmailGroupRepository emailGroupRepository,
                                      IEmailModelToMailMessageWithAnIdMapper emailModelToMailMessageWithAnIdMapper,
                                      ITagFillerService tagFillerService)
        {
            this.tagFillerService = tagFillerService;
            this.emailModelToMailMessageWithAnIdMapper = emailModelToMailMessageWithAnIdMapper;
            this.emailGroupRepository = emailGroupRepository;
        }

        public IEnumerable<MailMessageWithAnId> GetEmailsFromEmailGroup(string emailGroupEngineerId, object emailViewModel)
        {
            var emailGroup = emailGroupRepository.GetAll().Where(a => a.EngineerId == emailGroupEngineerId).FirstOrDefault();
            if (emailGroup == null) return new MailMessageWithAnId[] {};

            var mailMessages = new List<MailMessageWithAnId>();
            foreach(var mailMessageWithAnId in emailModelToMailMessageWithAnIdMapper.CreateSet(emailGroup.EmailModels))
            {
                mailMessageWithAnId.Subject = tagFillerService.AutoFillTagsFromModel(mailMessageWithAnId.Subject, emailViewModel);
                mailMessageWithAnId.Body = tagFillerService.AutoFillTagsFromModel(mailMessageWithAnId.Body, emailViewModel);
                mailMessages.Add(mailMessageWithAnId);
            }
            
            return mailMessages;
        }
    }
}
