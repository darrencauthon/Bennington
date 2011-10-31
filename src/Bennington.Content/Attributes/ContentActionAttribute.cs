using System;

namespace Bennington.Content.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ContentActionAttribute : Attribute
    {
        public bool Ignore { get; set; }
        public string DisplayName { get; set; }
    }
}