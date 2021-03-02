using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Net.Http.Headers;
using StoryForce.Server.Services;
using StoryForce.Server.ViewModels;
using StoryForce.Shared.Models;

namespace StoryForce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : Controller
    {
        private IConfiguration _configuration;
        private DriveService _gDriveService;
        private IHostingEnvironment hostingEnvironment;

        public FileController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] FilesSubmission submission)
        {
            foreach (var file in submission.FormFiles)
            {
                string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');
                fileName = this.EnsureCorrectFilename(fileName);

                byte[] buffer = new byte[16 * 1024];

                var extension = fileName.Substring(fileName.LastIndexOf('.') + 1);
                fileName = fileName.Replace(fileName.Substring(fileName.LastIndexOf('.') + 1), "");
                var namafilenewExcell = fileName.Replace(".", "") + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "." + extension;
               
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\upload", namafilenewExcell);
                
                if (System.IO.File.Exists(path))
                {
                    FileInfo f2 = new FileInfo(path);
                    f2.Delete();
                }

                await using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                try
                {
                    int readBytes;
                    await using FileStream output = System.IO.File.Create(this.GetPathAndFilename(namafilenewExcell));
                    await using Stream input = file.OpenReadStream();
                    while ((readBytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        await output.WriteAsync(buffer, 0, readBytes);
                    }
                }
                catch (Exception err)
                {
                    return Json(new { message = err.Message });
                }
            }

            return Json(new { message = "File(s) Uploaded Successfully!" });
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            filename = filename.Replace(" ", string.Empty);
            return filename;
        }

        private string GetPathAndFilename(string filename)
        {
            string path = this.hostingEnvironment.WebRootPath + "\\upload\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path + filename;
        }

    }
}
