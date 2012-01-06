using System.Text.RegularExpressions;

namespace Bennington.EmailCommunication.Validators
{
	public interface IEmailValidator
	{
		bool Validate(string email);
	}

	public class EmailValidator : IEmailValidator
	{
		private const string EmailFragment = @"[\x21\x23-\x27\x2A\x2B\x2D\x2F-\x39\x3D\x3F\x41-\x5A\x5E-\x7C]";

		private const string EmailPattern =
			"^" + EmailFragment + @"+(\." + EmailFragment + @"+)*\@" + EmailFragment + @"+(\." + EmailFragment + @"+)*$";

		private readonly Regex regEx = new Regex(EmailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

		public bool Validate(string email)
		{
			if (string.IsNullOrEmpty(email)) return false;

			return regEx.IsMatch(email);
		}
	}
}
