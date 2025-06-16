
using HappyDay.Application.Interface.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<List<string>> SaveFilesAsync(List<IFormFile> files, string targetFolder)
        {
            var savedPaths = new List<string>();
            var uploadPath = Path.Combine(_env.WebRootPath, targetFolder);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            foreach (var file in files)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                savedPaths.Add($"/{targetFolder}/{fileName}");
            }

            return savedPaths;
        }
        public async Task<string> SaveFileAsync(IFormFile file, string folder)
        {
            // 1. Dosya klasörü oluşturulacak yol
            var uploadPath = Path.Combine(_env.WebRootPath, folder);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            // 2. Dosya adını benzersiz yap
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            // 3. Tam fiziksel yol
            var filePath = Path.Combine(uploadPath, fileName);

            // 4. Dosyayı fiziksel olarak yaz
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // 5. Web üzerinden erişilebilir yol döndür (örnek: /uploads/cover/abc.jpg)
            var relativePath = Path.Combine(folder, fileName).Replace("\\", "/");
            return "/" + relativePath;
        }
    }
}