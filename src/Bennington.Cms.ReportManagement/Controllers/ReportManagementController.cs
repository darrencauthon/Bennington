using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Bennington.Cms.Controllers;
using Bennington.Cms.ReportManagement.Models;
using Bennington.Cms.ReportManagement.Repositories;
using Omu.ValueInjecter;

namespace Bennington.Cms.ReportManagement.Controllers
{
    [Authorize]
    public class ReportManagementController : ListManageController<ReportListViewModel, ReportInputModel>
    {
        private readonly IReportInputModelRepository reportInputModelRepository;
        private readonly Func<IReportDataRepository> reportRepository;

        public ReportManagementController(IReportInputModelRepository reportInputModelRepository,
                                          Func<IReportDataRepository> reportRepository)
        {
            this.reportRepository = reportRepository;
            this.reportInputModelRepository = reportInputModelRepository;
        }

        public ActionResult Stream(string id, DateTime? startDate, DateTime? endDate)
        {
            var reportInputModel = GetFormById(id);
            if (reportInputModel == null) throw new Exception("Report not found: " + id);

            return File(new UTF8Encoding().GetBytes(reportRepository().GetCsvData(reportInputModel)), "text/csv", string.IsNullOrWhiteSpace(reportInputModel.Filename) ? "Report.csv" : reportInputModel.Filename);                
        }

        public override void DeleteItem(object id)
        {
            var isThisAStringArray = id as string[];
            var idToUse = isThisAStringArray != null ? isThisAStringArray[0] : id.ToString();
            reportInputModelRepository.Delete(idToUse);

            base.DeleteItem(id);
        }

        public override void InsertForm(ReportInputModel form)
        {
            form.Id = Guid.NewGuid().ToString();
            reportInputModelRepository.SaveAndReturnId(form);
        }

        public override void UpdateForm(ReportInputModel form)
        {
            reportInputModelRepository.SaveAndReturnId(form);
            base.UpdateForm(form);
        }

        public override ReportInputModel GetFormById(object id)
        {
            return reportInputModelRepository.GetById(id.ToString());
        }

        protected override IQueryable<ReportListViewModel> GetListItems(Core.List.ListViewModel listViewModel)
        {
            return (
                    from item in reportInputModelRepository.GetAll()
                    select (ReportListViewModel) new ReportListViewModel().InjectFrom(item)
                   ).AsQueryable();
        }
    }
}
