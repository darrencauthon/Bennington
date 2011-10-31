namespace Bennington.Content
{
    public interface IContentTypeRegistry
    {
        void Save(params ContentType[] contentTypes);
    }
}