using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Bennington.Content.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ContentTypeAttribute : Attribute
    {
        private readonly string contentType;
        private readonly string[] ignoredActions;

        public ContentTypeAttribute(string contentType, params string[] ignoredActions)
        {
            this.ignoredActions = ignoredActions;
            this.contentType = contentType;
        }

        public string DisplayName { get; set; }

        public ContentType ToContentType(Type controllerType)
        {
            return new ContentType(contentType, DisplayName ?? contentType, controllerType.Name.Replace("Controller", ""), GetAllControllerActions(controllerType));
        }

        private ContentAction[] GetAllControllerActions(Type controllerType)
        {
            return (from method in controllerType.GetMethods()
                    where method.ReturnType == typeof(ActionResult)
                    where !ignoredActions.Contains(method.Name)
                    let attributes = (ContentActionAttribute[])method.GetCustomAttributes(typeof(ContentActionAttribute), true)
                    where attributes.Length == 0 || !attributes[0].Ignore
                    let actionName = GetActionName(method)
                    let displayName = GetDisplayName(method)
                    select new ContentAction {DisplayName = displayName ?? actionName, Action = actionName}).ToArray();
        }

        private static string GetDisplayName(MethodInfo method)
        {
            var attributes = (ContentActionAttribute[])method.GetCustomAttributes(typeof(ContentActionAttribute), true);
            return attributes.Length > 0 ? attributes[0].DisplayName : null;
        }

        private static string GetActionName(MethodInfo method)
        {
            var attributes = (ActionNameAttribute[])method.GetCustomAttributes(typeof(ActionNameAttribute), true);
            return attributes.Length > 0 ? attributes[0].Name : method.Name;
        }
    }
}