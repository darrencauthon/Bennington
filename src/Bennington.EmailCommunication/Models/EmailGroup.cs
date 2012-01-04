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

        public DateTime CreateDate { get; set; }

        public DateTime LastModifyDate { get; set; }
    }
}