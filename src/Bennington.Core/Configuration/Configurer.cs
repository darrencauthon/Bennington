using MvcTurbine.ComponentModel;
using System.Collections.Generic;

namespace Bennington.Core.Configuration
{
    public class Configurer
    {
        private static readonly Configurer DefaultConfigurer = new Configurer(null);
        private readonly List<Configurer> configurers = new List<Configurer>();

        public Configurer(Configurer parentConfigurer)
        {
            if(parentConfigurer != null)
                parentConfigurer.configurers.Add(this);
        }

        public static Configurer Configure
        {
            get { return DefaultConfigurer; }
        }

        public IServiceLocator ServiceLocator
        {
            get { return ServiceLocatorManager.Current; }
        }

        public virtual void Run()
        {
            configurers.ForEach(configurer => configurer.Run());
        }
    }
}