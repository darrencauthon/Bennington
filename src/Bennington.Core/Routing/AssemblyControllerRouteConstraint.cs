using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Bennington.Core.Routing
{
    public class AssemblyControllerRouteConstraint : IRouteConstraint
    {
        private readonly List<string> controllerNames;

        public AssemblyControllerRouteConstraint()
            : this(Assembly.GetCallingAssembly(), t => true)
        {
        }

        public AssemblyControllerRouteConstraint(Func<Type, bool> filter)
            : this(Assembly.GetCallingAssembly(), filter)
        {
        }

        public AssemblyControllerRouteConstraint(Assembly assembly)
            : this(assembly, t => true)
        {
        }

        public AssemblyControllerRouteConstraint(Assembly assembly, Func<Type, bool> filter)
        {
            controllerNames = (from type in assembly.GetTypes()
                               where typeof(IController).IsAssignableFrom(type)
                               where filter(type)
                               select type.Name.Replace("Controller", "")).ToList();
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var value = values[parameterName] as string;
            return controllerNames.Contains(value);
        }
    }
}