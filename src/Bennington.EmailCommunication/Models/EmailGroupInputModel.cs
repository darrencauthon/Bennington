using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bennington.Core.List;

namespace Bennington.EmailCommunication.Models
{
    public class EmailGroupInputModel
    {
        [Hidden]
        public string Id { get; set; }

        public string Name { get; set; }
    }
}