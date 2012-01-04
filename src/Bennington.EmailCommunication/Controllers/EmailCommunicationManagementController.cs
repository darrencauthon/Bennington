﻿using System;
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
    [Authorize]
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

        [ValidateInput(false)]
        public override ActionResult  Manage(EmailGroupInputModel form)
        {
 	         var reuslt = base.Manage(form);
            return reuslt;
        }

        public override ActionResult Manage()
        {
            var result = base.Manage();
            return result;
        }

        protected override IQueryable<EmailGroup> GetListItems(Core.List.ListViewModel listViewModel)
        {
            return emailGroupRepository.GetAll().AsQueryable();
        }

        public override EmailGroupInputModel GetFormById(object id)
        {
            var emailGroup = emailGroupRepository.GetById(id.ToString());
            if (emailGroup == null) return base.GetFormById(id);

            var emailGroupInpuModel = emailGroupToEmailGroupInputModelMapper.CreateInstance(emailGroup);
            if (emailGroupInpuModel.EmailInputModels == null) emailGroupInpuModel.EmailInputModels = new EmailInputModel[]{};
            if (emailGroupInpuModel.EmailInputModels.Count() < emailGroup.EmailCount)
            {
                var emails = emailGroupInpuModel.EmailInputModels.ToList();
                for (var n = 0; n <= emailGroup.EmailCount - emails.Count; n++)
                {
                    emails.Add(new EmailInputModel());
                }
                emailGroupInpuModel.EmailInputModels = emails.ToArray();

                return emailGroupInpuModel;
            }

            if (emailGroupInpuModel.EmailInputModels.Count() > emailGroup.EmailCount)
            {
                var emails = emailGroupInpuModel.EmailInputModels.ToList();
                var extraEmailCount = emailGroupInpuModel.EmailInputModels.Count() - emailGroup.EmailCount;
                emails.RemoveRange(emails.Count - extraEmailCount, extraEmailCount);

                emailGroupInpuModel.EmailInputModels = emails.ToArray();
            }

            return emailGroupInpuModel;
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
