namespace Bennington.Content.Data
{
    public interface IContentTypeRegistry
    {
        ContentType[] GetContentTypes();
        void Save(params ContentType[] contentTypes);
    }
}