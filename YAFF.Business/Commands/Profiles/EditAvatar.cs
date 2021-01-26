using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using YAFF.Core.Common;
using YAFF.Core.DTO;
using YAFF.Core.Entities;
using YAFF.Core.Interfaces;
using YAFF.Core.Settings;
using YAFF.Data;
using YAFF.Data.Extensions;

namespace YAFF.Business.Commands.Profiles
{
    public class EditAvatarCommand : IRequest<Result<UserProfileDto>>
    {
        public int UserId { get; set; }
        public IFile Avatar { get; set; }
    }

    public class EditAvatarCommandHandler : IRequestHandler<EditAvatarCommand, Result<UserProfileDto>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IPhotoStorage _photoStorage;
        private readonly IImageProcessor _imageProcessor;

        private readonly PhotoSettings _photoSettings;
        private readonly IMapper _mapper;

        public EditAvatarCommandHandler(ForumDbContext forumDbContext, IPhotoStorage photoStorage,
            IImageProcessor imageProcessor, IOptions<PhotoSettings> photoSettings, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _photoStorage = photoStorage;
            _imageProcessor = imageProcessor;
            _photoSettings = photoSettings.Value;
            _mapper = mapper;
        }

        public async Task<Result<UserProfileDto>> Handle(EditAvatarCommand request, CancellationToken cancellationToken)
        {
            var profile = await _forumDbContext.Profiles
                .IncludeUser()
                .SingleAsync(p => p.UserId == request.UserId);

            var validationResult = ValidatePhoto(request.Avatar);
            if (!validationResult.Succeeded)
            {
                return Result<UserProfileDto>.Failure(validationResult.Field, validationResult.Message);
            }
            
            var oldAvatar = profile.Avatar;
            if (oldAvatar != null)
            {
                _forumDbContext.Photos.Remove(oldAvatar);
                await _photoStorage.DeletePhotoAsync(oldAvatar.FileName);
            }

            var newAvatar = request.Avatar;

            var resizedAvatar = await _imageProcessor.ResizeImage(newAvatar);
            var fileName = await _photoStorage.StorePhotoAsync(resizedAvatar, ".png");

            profile.Avatar = new Photo {FileName = fileName};
            await _forumDbContext.SaveChangesAsync();

            var result = _mapper.Map<UserProfileDto>(profile);
            return Result<UserProfileDto>.Success(result);
        }

        private Result<object> ValidatePhoto(IFile photo)
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