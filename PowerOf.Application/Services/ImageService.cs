using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PowerOf.Application.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerOf.Application.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task DeleteFileAsync(string FilePath)
        {
            if (FilePath != null)
            {
                var RootPath = _webHostEnvironment.WebRootPath;
                var oldFile = Path.Combine(RootPath, FilePath);

                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }
            }
        }

        public async Task<string> SaveImageAsync(IFormFile Img, string folderPath)
        {
            if (Img == null || Img.Length == 0)
            {
                return null;
            }
            string extension = Path.GetExtension(Img.FileName); 
            string fileName = Guid.NewGuid().ToString() + extension; 
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, folderPath, fileName); 
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await Img.CopyToAsync(fileStream);
            }

            return Path.Combine(folderPath, fileName);
        }

    }
}
