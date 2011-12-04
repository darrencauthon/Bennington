using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Models
{
    public class ContentTreeNodeMetaInformationInputModel
    {
        public string TreeNodeId { get; set; }
        public string PageId { get; set; }

        [DisplayName("Meta Title")]
        public string MetaTitle { get; set; }
        
        [DisplayName("Meta Keywords")]
        public string MetaKeywords { get; set; }

        [DisplayName("Meta Description")]
        public string MetaDescription { get; set; }
    }
}
