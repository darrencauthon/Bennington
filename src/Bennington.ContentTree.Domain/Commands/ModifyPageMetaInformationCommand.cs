using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleCqrs.Commanding;

namespace Bennington.ContentTree.Domain.Commands
{
    public class ModifyPageMetaInformationCommand : CommandWithAggregateRootId
    {
        public string MetaTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string TreeNodeId { get; set; }
        public string Action { get; set; }
    }
}
