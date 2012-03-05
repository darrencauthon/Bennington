using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Bennington.Core.List;

namespace Bennington.Cms.ReportManagement.Models
{
    public class ReportListViewModel
    {
        public string Name { get; set; }
        
        [Hidden]
        public string Id { get; set; }

        [DisplayName("")]
        public string StartDate { get; set; }

        [DisplayName("")]
        public string EndDate { get; set; }

        [Hidden]
        public bool DisplayStartDateEndDate { get; set; }

        [Hidden]
        public string UrlToStreamCsv { get; set; }

        [DisplayName("")]
        public string ViewReportLink { get; set; }
    }
}
