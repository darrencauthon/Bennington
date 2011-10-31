using System;
using System.Linq;
using Bennington.Content.Attributes;
using MvcTurbine;
using MvcTurbine.Blades;
using MvcTurbine.ComponentModel;

namespace Bennington.Content
{
    public class ContentBlade : Blade, ISupportAutoRegistration
    {
        private IContentTreeProvider contentTreeProvider;

        public override void Spin(IRotorContext context)
        {
            var serviceLocator = GetServiceLocatorFromContext(context);
            contentTreeProvider = serviceLocator.Resolve<IContentTreeProvider>();
        }

        protected override void InvokeDisposed(EventArgs e)
        {
            contentTreeProvider.Dispose();
            base.InvokeDisposed(e);
        }

        public void AddRegistrations(AutoRegistrationList registrationList)
        {
            registrationList.Add(Registration.Custom<ServiceRegistration>(IsController, AddContentType));
        }

        private static void AddContentType(IServiceLocator serviceLocator, Type type)
        {
            var contentTypeRegistry = serviceLocator.Resolve<IContentTypeRegistry>();
            var contentTypes = (from ContentTypeAttribute attribute in type.GetCustomAttributes(typeof(ContentTypeAttribute), true)
                                select attribute.ToContentType(type)).ToArray();

            contentTypeRegistry.Save(contentTypes);
        }

        private static bool IsController(Type type, Type serviceType)
        {
            return type.IsContentController();
        }
    }
}