using Bennington.Cms.MenuSystem;

namespace Bennington.EmailCommunication
{
    public class MenuSystemConfigurer : IMenuSystemConfigurer
    {
        public void Configure(IMenuRegistry sectionMenuRegistry)
        {
            sectionMenuRegistry.Add(new ActionIconMenuItem("Email Communication Management", "~/Content/Canvas/emailicon.png", "Index", "EmailCommunicationManagement"));
        }
    }
}
