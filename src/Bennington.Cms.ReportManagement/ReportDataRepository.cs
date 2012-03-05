using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bennington.Cms.ReportManagement.Models;

namespace Bennington.Cms.ReportManagement
{
    public interface IReportDataRepository
    {
        string GetCsvData(ReportInputModel reportInputModel);
    }

    public class ReportDataRepository : IReportDataRepository
    {
        public string GetCsvData(ReportInputModel reportInputModel)
        {
            throw new NotImplementedException();
        }
    }
}
