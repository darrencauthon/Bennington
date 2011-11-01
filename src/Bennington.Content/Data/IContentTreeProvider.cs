using System;

namespace Bennington.Content.Data
{
    public interface IContentTreeProvider : IDisposable
    {
        event EventHandler<EventArgs> ContentChanged;
        void Refresh();
        ContentTree GetContentTree();
    }
}