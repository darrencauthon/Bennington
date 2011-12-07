using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapperAssist;
using Bennington.Cms.PrincipalProvider.Encryption;
using Bennington.Cms.PrincipalProvider.Models;

namespace Bennington.Cms.PrincipalProvider.Mappers
{
	public interface IUserInputModelToUserMapper
	{
		User CreateInstance(UserInputModel source);
	}

	public class UserInputModelToUserMapper : Mapper<UserInputModel, User>, IUserInputModelToUserMapper
	{
		private readonly IEncryptionService encryptionService;

		public UserInputModelToUserMapper(IEncryptionService encryptionService)
		{
			this.encryptionService = encryptionService;
		}

		public override void DefineMap(AutoMapper.IConfiguration configuration)
		{
			configuration.CreateMap<UserInputModel, User>()
				;
		}

		public override User CreateInstance(UserInputModel source)
		{
			var mappedInstance = base.CreateInstance(source);

			mappedInstance.Password = encryptionService.Encrypt(source.Password);

			return mappedInstance;
		}
	}
}