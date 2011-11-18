using System.Collections.Generic;
using System.Reflection;

namespace Bennington.FileUploadHandling.Registration
{
    public class InterfaceToSingleImplementationRegistrationConvention : Bennington.Core.Registration.InterfaceToSingleImplementationRegistrationConvention
    {
        protected override IEnumerable<Assembly> GetAssembliesToScan()
        {
            return new Assembly[] { GetType().Assembly };
        }
    }
}