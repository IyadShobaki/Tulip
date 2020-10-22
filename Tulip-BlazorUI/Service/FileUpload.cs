using BlazorInputFile;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tulip_BlazorUI.Contratcs;

namespace Tulip_BlazorUI.Service
{
    public class FileUpload : IFileUplaod
    {
        private readonly IWebHostEnvironment _environment;

        public FileUpload(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public void UploadFile(IFileListEntry file, MemoryStream msFile, string pictureName)
        {
            try
            {           
                var path = $"{_environment.WebRootPath}\\images\\{pictureName}";

                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    msFile.WriteTo(fileStream);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
