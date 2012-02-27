using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bennington.Cms.ReportManagement.Models
{
    public class ReportInputModel
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string CollectionName { get; set; }
        public string CommaSeparatedListOfFields { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
