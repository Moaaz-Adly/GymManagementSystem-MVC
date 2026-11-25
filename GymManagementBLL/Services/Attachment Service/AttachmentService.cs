using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Attachment_Service
{
    public class AttachmentService : IAttachmentService
    {
        public AttachmentService(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
        }
        private readonly string[] allowedExtention = {".jpg" , ".jpeg" , ".png"} ;
        private readonly long maxFileSize = 5 * 1024 * 1024; // 5MB
        private readonly IWebHostEnvironment webHost;

        public string? Upload(string folderName, IFormFile file)
        {
            try
            {
                if (string.IsNullOrEmpty(folderName) || file is null || file.Length == 0) return null;

                if (file.Length > maxFileSize) return null;

                var Extention = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtention.Contains(Extention)) return null;

                var FolderPath = Path.Combine(webHost.WebRootPath, "images", folderName);
                if(Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }

                var fileName = Guid.NewGuid().ToString() + Extention;
                var FilePath = Path.Combine(FolderPath , fileName);

                using var fileStream = new FileStream(FilePath, FileMode.Create);
                file.CopyTo(fileStream);

                return fileName;





            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed To Ubload File to {folderName} : {ex}");
                return null;
            }
  
        }


        public bool Delete(string fileName, string folderName)
        {
            try
            {
                if (string.IsNullOrEmpty (folderName) || string.IsNullOrEmpty(fileName)) 
                    return false;
                var FullPath = Path.Combine(webHost.WebRootPath, "images" , folderName , fileName);
                if (Path.Exists(FullPath))
                {
                    File.Delete(FullPath);
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed To Deleted File {fileName} : {ex} ");
                return false;
            }
        }
    }
}
