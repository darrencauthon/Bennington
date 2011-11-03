namespace Bennington.Content.Attributes
{
    public class EngineAttribute : ContentTypeAttribute
    {
        public EngineAttribute(string displayName, params string[] ignoredActions) : base("Engine", ignoredActions)
        {
            DisplayName = displayName;
        }
    }
}