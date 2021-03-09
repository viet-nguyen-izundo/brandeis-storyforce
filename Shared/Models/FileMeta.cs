using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryForce.Shared.Models
{
    public enum StorageProvider
    {
        DropBox,
        GoogleDrive,
        LocalFileSystem,
        OneDrive,
        Url
    }

    public class FileMeta
    {
        public string FileId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DownloadUrl { get; set; }
        public string accessToken { get; set; }
        public StorageProvider StorageProvider { get; set; }
    }
}
