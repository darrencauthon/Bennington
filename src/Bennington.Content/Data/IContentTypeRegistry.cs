namespace Bennington.Content.Data
{
    public interface IContentTypeRegistry
    {
        void Save(params ContentType[] contentTypes);
    }
}