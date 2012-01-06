namespace Bennington.EmailCommunicationManagement.Validators.Helpers
{
    public interface IValidateSemiColonDelimitedEmailListHelper
    {
        bool IsThisAValidEmailList(string semiColonDelimitedEmails);
    }

    public class ValidateSemiColonDelimitedEmailListHelper : IValidateSemiColonDelimitedEmailListHelper
    {
        private readonly IEmailValidator emailValidator;

        public ValidateSemiColonDelimitedEmailListHelper(IEmailValidator emailValidator)
        {
            this.emailValidator = emailValidator;
        }

        public bool IsThisAValidEmailList(string semiColonDelimitedEmails)
        {
            if (string.IsNullOrWhiteSpace(semiColonDelimitedEmails)) return false;

            foreach (var email in semiColonDelimitedEmails.Split(';'))
            {
                if (!emailValidator.Validate(email)) return false;
            }

            return true;
        }
    }
}