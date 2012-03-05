using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Bennington.Cms.ReportManagement.Models
{
    public class ReportInputModel
    {
        public string Name { get; set; }
        
        public string Id { get; set; }

        [DisplayName("Connection String Identifier")]
        public string ConnectionStringIdentifier { get; set; }

        [DisplayName("Collection Name")]
        public string CollectionName { get; set; }

        [DisplayName("Comma Separated List of Fields")]
        public string CommaSeparatedListOfFields { get; set; }

        [DisplayName("Url to Stream .csv (StartDate and EndDate will be passed on the querystring)")]
        public string UrlToStreamCsv { get; set; }

        [DisplayName("Dipslay start/end date in list?")]
        public bool DisplayStartDateEndDate { get; set; }

        public string Filename { get; set; }
    }
}
