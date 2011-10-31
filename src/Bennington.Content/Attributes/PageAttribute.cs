namespace Bennington.Content.Attributes
{
    public class PageAttribute : ContentTypeAttribute
    {
        public PageAttribute(params string[] ignoredActions) : base("Page", ignoredActions)
        {
        }
    }
}