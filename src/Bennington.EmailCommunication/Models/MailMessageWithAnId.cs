using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace Bennington.EmailCommunication.Models
{
    public class MailMessageWithAnId : MailMessage
    {
        public string EngineerId { get; set; }
    }
}
