using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using YAFF.Core.Interfaces;
using YAFF.Core.Settings;

namespace YAFF.Business.Common
{
    public class ImageProcessor : IImageProcessor
    {
        private readonly PhotoProcessorSettings _settings;

        public ImageProcessor(IOptions<PhotoProcessorSettings> options)
        {
            _settings = options.Value;
        }

        public Task<byte[]> ResizeImage(IFile image)
        {
            return ResizeInternal(image.OpenReadStream());
        }

        public Task<byte[]> ResizeImage(Stream image)
        {
            return ResizeInternal(image);
        }

        private async Task<byte[]> ResizeInternal(Stream imageStream)
        {
            using var img = await Image.LoadAsync(imageStream);
            img.Mutate(ctx => ctx.Resize(_settings.Width, _settings.Height));

            await using var ms = new MemoryStream();
            await img.SaveAsPngAsync(ms);
            
            return ms.ToArray();
        }
    }
}