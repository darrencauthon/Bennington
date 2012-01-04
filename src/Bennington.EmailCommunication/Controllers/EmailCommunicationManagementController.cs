using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bennington.Cms.Controllers;
using Bennington.EmailCommunication.Mappers;
using Bennington.EmailCommunication.Models;
using Bennington.EmailCommunication.Repositories;
using Bennington.EmailCommunication.Services;
using Bennington.Repository;

namespace Bennington.EmailCommunication.Controllers
{
    public class EmailCommunicationManagementController : ListManageController<EmailGroup, EmailGroupInputModel>
    {
        private readonly IEmailGroupRepository emailGroupRepository;
        private readonly IEmailGroupToEmailGroupInputModelMapper emailGroupToEmailGroupInputModelMapper;
        private readonly IEmailGroupInputModelProcessingService emailGroupInputModelProcessingService;

        public EmailCommunicationManagementController(IEmailGroupRepository emailGroupRepository,
                                                      IEmailGroupToEmailGroupInputModelMapper emailGroupToEmailGroupInputModelMapper,
                                                      IEmailGroupInputModelProcessingService emailGroupInputModelProcessingService)
        {
            this.emailGroupInputModelProcessingService = emailGroupInputModelProcessingService;
            this.emailGroupToEmailGroupInputModelMapper = emailGroupToEmailGroupInputModelMapper;
            this.emailGroupRepository = emailGroupRepository;
        }

        protected override IQueryable<EmailGroup> GetListItems(Core.List.ListViewModel listViewModel)
        {
            return emailGroupRepository.GetAll().AsQueryable();
        }

        public override EmailGroupInputModel GetFormById(object id)
        {
            var emailGroup = emailGroupRepository.GetById(id.ToString());
            if (emailGroup == null) return base.GetFormById(id);

            return emailGroupToEmailGroupInputModelMapper.CreateInstance(emailGroup);
        }

        public override void InsertForm(EmailGroupInputModel form)
        {
            form.Id = emailGroupInputModelProcessingService.SaveAndReturnId(form);

            base.InsertForm(form);
        }

        public override void UpdateForm(EmailGroupInputModel form)
        {
            form.Id = emailGroupInputModelProcessingService.SaveAndReturnId(form);
            base.UpdateForm(form);
        }

        public override void DeleteItem(object id)
        {
            var isThisAStringArray = id as string[];
            var idToUse = isThisAStringArray != null ? isThisAStringArray[0] : id.ToString();
            emailGroupRepository.Delete(idToUse);

            base.DeleteItem(id);
        }
    }
}
