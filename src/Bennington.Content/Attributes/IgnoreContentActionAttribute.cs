namespace Bennington.Content.Attributes
{
    public class IgnoreContentActionAttribute : ContentActionAttribute
    {
        public IgnoreContentActionAttribute()
        {
            Ignore = true;
        }
    }
}