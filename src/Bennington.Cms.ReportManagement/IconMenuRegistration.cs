using Bennington.Cms.MenuSystem;

namespace Bennington.Cms.ReportManagement
{
    public class MenuSystemConfigurer : IMenuSystemConfigurer
    {
        public void Configure(IMenuRegistry sectionMenuRegistry)
        {
            sectionMenuRegistry.Add(new ActionIconMenuItem("Report Management", "~/Content/Canvas/icondemo.png", "Index", "ReportManagement"));
        }
    }
}
