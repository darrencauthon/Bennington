namespace Bennington.Content.Attributes
{
    public class SectionAttribute : ContentTypeAttribute
    {
        public SectionAttribute(params string[] ignoredActions) : base("Section", ignoredActions)
        {
        }
    }
}