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
        string GetData(ReportInputModel reportInputModel);
    }

    public class ReportDataRepository : IReportDataRepository
    {
        public string GetData(ReportInputModel reportInputModel)
        {
            return "test1,test2\na,b\n1,2";
        }
    }
}
