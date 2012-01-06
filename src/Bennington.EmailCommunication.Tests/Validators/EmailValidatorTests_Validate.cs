using AutoMoq;
using Bennington.EmailCommunication.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bennington.EmailCommunication.Tests.Validators
{
	[TestClass]
	public class EmailValidatorTests_Validate
	{
		private AutoMoqer mocker;

		[TestInitialize]
		public void INit()
		{
			mocker = new AutoMoqer();
		}

		[TestMethod]
		public void Returns_true_for_good_email()
		{
			var result = mocker.Resolve<EmailValidator>().Validate("email@x.com");

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void Returns_false_for_bad_email_with_domain()
		{
			var result = mocker.Resolve<EmailValidator>().Validate("eamil@email@email@x.com");

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Returns_false_for_email_without_domain()
		{
			var result = mocker.Resolve<EmailValidator>().Validate("eamil@email@email");

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Returns_false_when_passed_null()
		{
			var result = mocker.Resolve<EmailValidator>().Validate(null);

			Assert.IsFalse(result);
		}
	}
}
