using System.Web.Mvc;
using Bennington.Core.Configuration;
using Bennington.Core.List;
using MvcTurbine.ComponentModel;
using MvcTurbine.Unity;
using MvcTurbine.Web;

namespace SampleContentWebsite
{
    public class MvcApplication : TurbineApplication
    {
        static MvcApplication()
        {
            ServiceLocatorManager.SetLocatorProvider(() => new UnityServiceLocator());
        }

        public override void Startup()
        {
            ModelBinders.Binders.Add(typeof(ListViewModel), new ListViewModelBinder());

            Configurer.Configure
                        .Content()
                            .UseSql("Bennington.ContentTree.Domain.ConnectionString")
                            .Run();


            base.Startup();
        }
    }
}