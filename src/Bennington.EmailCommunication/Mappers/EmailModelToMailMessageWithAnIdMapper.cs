using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using AutoMapperAssist;
using Bennington.EmailCommunication.Models;

namespace Bennington.EmailCommunication.Mappers
{
    public interface IEmailModelToMailMessageWithAnIdMapper
    {
        IEnumerable<MailMessageWithAnId> CreateSet(IEnumerable<EmailModel> source);
    }

    public class EmailModelToMailMessageWithAnIdMapper : Mapper<EmailModel, MailMessageWithAnId>, IEmailModelToMailMessageWithAnIdMapper
    {
        public override void DefineMap(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<EmailModel, MailMessageWithAnId>()
                .ForMember(a => a.From, b => b.MapFrom(c => GetFrom(c)))
                .ForMember(a => a.Sender, b => b.Ignore())
                .ForMember(a => a.ReplyTo, b => b.Ignore())
                .ForMember(a => a.ReplyToList, b => b.Ignore())
                .ForMember(a => a.To, b => b.Ignore())
                .ForMember(a => a.CC, b => b.Ignore())
                .ForMember(a => a.Bcc, b => b.Ignore())
                .ForMember(a => a.IsBodyHtml, b => b.Ignore())
                .ForMember(a => a.SubjectEncoding, b => b.Ignore())
                .ForMember(a => a.HeadersEncoding, b => b.Ignore())
                .ForMember(a => a.Body, b => b.Ignore())
                .ForMember(a => a.BodyEncoding, b => b.Ignore())
                .ForMember(a => a.Attachments, b => b.Ignore())
                .ForMember(a => a.AlternateViews, b => b.Ignore())
                .ForMember(a => a.DeliveryNotificationOptions, b => b.Ignore())
                .ForMember(a => a.Priority, b => b.Ignore())
                ;
            base.DefineMap(configuration);
        }

        public override IEnumerable<MailMessageWithAnId> CreateSet(IEnumerable<EmailModel> source)
        {
            var mailMessages = new List<MailMessageWithAnId>();
            foreach (var sourceItem in source)
            {
                mailMessages.Add(CreateInstance(sourceItem));
            }
            return mailMessages;
        }


        public override MailMessageWithAnId CreateInstance(EmailModel emailModel)
        {
            var result = base.CreateInstance(emailModel);

            result.To.Add(emailModel.ToEmail);
            
            if (!string.IsNullOrWhiteSpace(emailModel.CcEmails))
                result.CC.Add(emailModel.CcEmails.Replace(';', ','));
            
            if (!string.IsNullOrWhiteSpace(emailModel.BccEmails))
                result.Bcc.Add(emailModel.BccEmails.Replace(';', ','));

            result.Body = emailModel.BodyText;
            result.IsBodyHtml = emailModel.IsBodyHtml;

            return result;
        }

        private static MailAddress GetFrom(EmailModel emailModel)
        {
            return new MailAddress(emailModel.FromEmail);
        }
    }
}
