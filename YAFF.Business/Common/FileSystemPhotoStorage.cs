using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using YAFF.Core.Interfaces;

namespace YAFF.Business.Common
{
    public class FileSystemPhotoStorage : IPhotoStorage
    {
        private readonly IHostEnvironment _environment;

        public FileSystemPhotoStorage(IHostEnvironment environment)
        {
            _environment = environment;
        }

        public Task<string> StorePhotoAsync(IFile photo)
        {
            return StoreInternal(photo.OpenReadStream(), Path.GetExtension(photo.FileName));
        }

        public Task<string> StorePhotoAsync(byte[] photo, string extension)
        {
            using var stream = new MemoryStream(photo);
            return StoreInternal(stream, extension);
        }

        private async Task<string> StoreInternal(Stream photoStream, string extension)
        {
            var fileName = Guid.NewGuid() + extension;
            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot", "pictures", fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await photoStream.CopyToAsync(stream);

            return fileName;
        }

        public Task DeletePhotoAsync(string fileName)
        {
            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot", "pictures", fileName);
            File.Delete(filePath);

            return Task.CompletedTask;
        }
    }
}