namespace Bennington.Content.Data
{
    public class ContentType
    {
        public string Type { get; set; }
        public string DisplayName { get; set; }
        public string ControllerName { get; set; }
        public ContentAction[] Actions { get; set; }

        public ContentType(string type, string displayName, string controllerName, params ContentAction[] actions)
        {
            Type = type;
            DisplayName = displayName;
            ControllerName = controllerName;
            Actions = actions;
        }

        public override string ToString()
        {
            return Type;
        }
    }
}