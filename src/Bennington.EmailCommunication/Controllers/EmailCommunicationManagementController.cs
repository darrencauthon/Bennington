using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bennington.Cms.Controllers;
using Bennington.EmailCommunication.Mappers;
using Bennington.EmailCommunication.Models;
using Bennington.EmailCommunication.Repositories;
using Bennington.Repository;

namespace Bennington.EmailCommunication.Controllers
{
    public class EmailCommunicationManagementController : ListManageController<EmailGroup, EmailGroupInputModel>
    {
        private readonly IEmailGroupRepository emailGroupRepository;
        private readonly IEmailGroupToEmailGroupInputModelMapper emailGroupToEmailGroupInputModelMapper;
        private readonly IEmailGroupInputModelToEmailGroupMapper emailGroupInputModelToEmailGroupMapper;

        public EmailCommunicationManagementController(IEmailGroupRepository emailGroupRepository,
                                                      IEmailGroupToEmailGroupInputModelMapper emailGroupToEmailGroupInputModelMapper,
                                                      IEmailGroupInputModelToEmailGroupMapper emailGroupInputModelToEmailGroupMapper)
        {
            this.emailGroupInputModelToEmailGroupMapper = emailGroupInputModelToEmailGroupMapper;
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
            form.Id = SaveAndReturnId(form);

            base.InsertForm(form);
        }

        private string SaveAndReturnId(EmailGroupInputModel form)
        {
            var emailGroup = emailGroupRepository.GetAll().Where(a => a.Id == form.Id).FirstOrDefault();
            if (emailGroup != null) emailGroupInputModelToEmailGroupMapper.LoadIntoInstance(form, emailGroup ?? new EmailGroup());
            else
                emailGroup = emailGroupInputModelToEmailGroupMapper.CreateInstance(form);

            emailGroup.Id = emailGroup.Id ?? Guid.NewGuid().ToString();
            emailGroup.CreateDate = emailGroup.CreateDate == DateTime.MinValue ? DateTime.Now : emailGroup.CreateDate;
            emailGroup.LastModifyDate = DateTime.Now;
            emailGroupRepository.SaveAndReturnId(emailGroup);
            return emailGroup.Id;
        }

        public override void DeleteItem(object id)
        {
            emailGroupRepository.Delete(id.ToString());
            base.DeleteItem(id);
        }
    }
}
