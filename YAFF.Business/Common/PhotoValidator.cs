using Microsoft.Extensions.Options;
using YAFF.Core.Common;
using YAFF.Core.Interfaces;
using YAFF.Core.Settings;

namespace YAFF.Business.Common
{
    public class PhotoValidator : IPhotoValidator
    {
        private readonly PhotoSettings _photoSettings;

        public PhotoValidator(IOptions<PhotoSettings> photoSettings)
        {
            _photoSettings = photoSettings.Value;
        }

        public Result<object> ValidatePhoto(IFile photo)
        {
            if (photo.Length > _photoSettings.MaxBytes || photo.Length <= 0)
            {
                return Result<object>.Failure(nameof(photo.Length),
                    $"Photo size should be greater than 0 and less than {_photoSettings.MaxBytes / 1024 / 1024} MB");
            }

            if (!_photoSettings.HasSupportedExtension(photo.FileName))
            {
                return Result<object>.Failure(nameof(photo.FileName), "Unsupported file extension!");
            }

            return Result<object>.Success();
        }
    }
}