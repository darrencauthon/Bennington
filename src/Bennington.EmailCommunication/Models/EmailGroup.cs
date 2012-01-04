using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bennington.Core.List;

namespace Bennington.EmailCommunication.Models
{
    public class EmailGroup
    {
        [Hidden]
        public string Id { get; set; }

        public string Name { get; set; }

        public string EngineerId { get; set; }

        public int EmailCount { get; set; }

        [Hidden]
        public DateTime CreateDate { get; set; }

        public DateTime LastModifyDate { get; set; }

        [Hidden]
        public EmailModel[] EmailModels { get; set; }
    }

    public class EmailModel
    {
        public string Name { get; set; }
        public string AdministratorInsturctionText { get; set; }
        public bool ShowToField { get; set; }
        public string ToEmail { get; set; }
        public string CcEmails { get; set; }
        public string BccEmails { get; set; }
        public string FromEmail { get; set; }
        public string BodyText { get; set; }
    }
}