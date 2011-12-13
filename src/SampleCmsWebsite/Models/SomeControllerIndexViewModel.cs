using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bennington.Cms.Metadata;

namespace SampleCmsWebsite.Models
{
    public class SomeControllerIndexViewModel
    {
        public SomeForm SomeForm { get; set; }
    }

    public class SomeForm
    {
        [HtmlEditor]
        public string SomeProperty { get; set; }
    }
}