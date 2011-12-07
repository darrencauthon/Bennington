using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Bennington.Core.List;

namespace Bennington.Cms.PrincipalProvider.Models
{
	[Serializable]
	public class User
	{
        [Hidden]
		public string Id { get; set; }
        
        [Display(Order = 10)]
        public string FirstName { get; set;}

        [Display(Order = 5)]
        public string LastName { get; set; }
		
        [Hidden]
        public string Email { get; set; }

        [Display(Order = 3)]
        public string Username { get; set; }

        [Hidden]
        public string Password { get; set; }
	}
}