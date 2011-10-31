using System;
using System.Web.Mvc;
using Bennington.Content.Attributes;

namespace Bennington.Content
{
    public static class ContentUtility
    {
        public static bool IsContentController(this Type type)
        {
            return typeof(IController).IsAssignableFrom(type) && type.GetCustomAttributes(typeof(ContentTypeAttribute), true).Length > 0;
        }
    }
}