using System;
using System.Collections.Generic;
using System.Linq;
using Bennington.Content.Attributes;
using Bennington.Content.Data;
using MvcTurbine;
using MvcTurbine.Blades;
using MvcTurbine.ComponentModel;

namespace Bennington.Content
{
    public class ContentBlade : Blade, ISupportAutoRegistration
    {
        private IContentTreeProvider contentTreeProvider;
        private readonly List<Type> contentTypesToRegister = new List<Type>();

        public override void Spin(IRotorContext context)
        {
            var serviceLocator = GetServiceLocatorFromContext(context);
            contentTreeProvider = serviceLocator.Resolve<IContentTreeProvider>();
            var contentTypeRegistry = serviceLocator.Resolve<IContentTypeRegistry>();

            var contentTypes = (from type in contentTypesToRegister
                                from ContentTypeAttribute attribute in type.GetCustomAttributes(typeof(ContentTypeAttribute), true)
                                select attribute.ToContentType(type)).ToArray();

            contentTypeRegistry.Save(contentTypes);
        }

        protected override void InvokeDisposed(EventArgs e)
        {
            contentTreeProvider.Dispose();
            base.InvokeDisposed(e);
        }

        public void AddRegistrations(AutoRegistrationList registrationList)
        {
            registrationList.Add(Registration.Custom<ServiceRegistration>(
                (t, serviceType) => t.IsContentController(), 
                (serviceLocator, type) => contentTypesToRegister.Add(type)));
        }
    }
}