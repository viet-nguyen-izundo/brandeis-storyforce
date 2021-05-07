using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryForce.Shared.Models;

namespace StoryForce.Shared.ViewModels
{
    public class UploadByUrl : UploadFile 
    {
        public UploadByUrl() => this.StorageProvider = StorageProvider.Url;

        public string AccessToken { get; set; }
    }
}
