using System.Net.Mail;

namespace Bennington.EmailCommunication
{
	public interface IEmailService
	{
		void Send(MailMessage mailMessage);
	}

	public class EmailService : IEmailService
	{
		public void Send(MailMessage mailMessage)
		{
			var emailClient = new SmtpClient();
			emailClient.Send(mailMessage);
		}
	}
}
