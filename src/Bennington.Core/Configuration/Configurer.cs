using System.Reflection;
using MvcTurbine.ComponentModel;

namespace Bennington.Core.Configuration
{
    public class Configurer
    {
        private readonly Configurer parentConfigurer;
        
        public Configurer(Configurer parentConfigurer)
        {
            this.parentConfigurer = parentConfigurer;
        }

        private Configurer(Assembly applicationAssembly)
        {
           ApplicationAssembly = applicationAssembly;
        }

        public static Configurer Configure
        {
            get { return new Configurer(Assembly.GetCallingAssembly()); }
        }

        public static Assembly ApplicationAssembly { get; private set; }

        public IServiceLocator ServiceLocator
        {
            get { return ServiceLocatorManager.Current; }
        }

        public virtual void Run()
        {
            if(parentConfigurer != null)
                parentConfigurer.Run();
        }
    }
}