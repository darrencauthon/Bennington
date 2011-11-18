using Bennington.FileUploadHandling.Models;
using MvcTurbine.Web.Models;

namespace Bennington.FileUploadHandling.Registration
{
    public class FileUploadModelBinderRegistration : ModelBinderRegistry
    {
        public FileUploadModelBinderRegistration()
        {
            Bind<FileUploadInputModel, FileUploadInputModelBinder>();
        }
    } 
}
