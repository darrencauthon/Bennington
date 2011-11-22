using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;

namespace Bennington.FileUploadHandling.Models
{
    public class FileUploadInputModel
    {
        public string Id { get; set; }
        public string Filename { get; set; }
        [UIHint("HttpPostedFileBase")]
        public HttpPostedFileBase FileUpload { get; set; }
        public string ContainerName { get; set; }
        public string UrlRelativeToFileUploadRoot { get; set; }
        public string DirectoryName { get; set; }
        public bool Remove { get; set; }
    }
}
