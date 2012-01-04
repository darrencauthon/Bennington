using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Bennington.Cms.Metadata;
using Bennington.Core.List;

namespace Bennington.EmailCommunication.Models
{
    public class EmailGroupInputModel
    {
        [Hidden]
        public string Id { get; set; }

        public string Name { get; set; }

        [DisplayName("Id to use for pulling these emails from code")]
        public string EngineerId { get; set; }

        [DisplayName("The number of emails for this group.  DECREMENTING THIS NUMBER IS NOT A REVERSIBLE OPERATION")]
        public int EmailCount { get; set; }

        public EmailInputModel[] EmailInputModels { get; set; }
    }

    public class EmailInputModel
    {
        public string Name { get; set; }

        [DisplayName("The text administrators see")]
        public string AdministratorInsturctionText { get; set; }

        [DisplayName("Show To field?")]
        public bool ShowToField { get; set; }

        [DisplayName("To Email")]
        public string ToEmail { get; set; }

        [DisplayName("Cc Emails (separated by semi-colon)")]
        public string CcEmails { get; set; }

        [DisplayName("Bcc Emails (separated by semi-colon)")]
        public string BccEmails { get; set; }

        [DisplayName("From Email")]
        public string FromEmail { get; set; }
        
        [HtmlEditor]
        [DisplayName("Body Text")]
        public string BodyText { get; set; }
    }
}