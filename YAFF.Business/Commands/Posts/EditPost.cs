using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YAFF.Core.Common;
using YAFF.Core.DTO;
using YAFF.Core.Entities;
using YAFF.Core.Interfaces;
using YAFF.Data;
using YAFF.Data.Extensions;

namespace YAFF.Business.Commands.Posts
{
    public class UpdatePostCommand : IRequest<Result<PostDto>>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public IEnumerable<string> Tags { get; set; }

        public int EditorId { get; set; }

        public string PreviewBody { get; init; }
        public IFile PreviewImage { get; init; }
    }

    public class EditPostCommandHandler : IRequestHandler<UpdatePostCommand, Result<PostDto>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IPhotoValidator _photoValidator;
        private readonly IPhotoStorage _photoStorage;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public EditPostCommandHandler(ForumDbContext forumDbContext, IPhotoValidator photoValidator,
            IPhotoStorage photoStorage, IMediator mediator, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _photoValidator = photoValidator;
            _photoStorage = photoStorage;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<Result<PostDto>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _forumDbContext.Posts
                .IncludeTags()
                .IncludePreview()
                .SingleOrDefaultAsync(p => p.Id == request.Id && p.AuthorId == request.EditorId);
            if (post == null)
            {
                return Result<PostDto>.Failure(nameof(request.Id), "Post doesn't exist or you can't edit it");
            }

            post.Title = request.Title;
            post.Body = request.Body;
            post.DateEdited = DateTime.UtcNow;
            post.Preview.Body = request.PreviewBody;

            if (request.PreviewImage != null)
            {
                var validationResult = _photoValidator.ValidatePhoto(request.PreviewImage);
                if (!validationResult.Succeeded)
                {
                    return Result<PostDto>.Failure(validationResult.Field, validationResult.Message);
                }

                var oldPhoto = post.Preview.Image;
                _forumDbContext.Photos.Remove(oldPhoto);
                await _photoStorage.DeletePhotoAsync(oldPhoto.FileName);

                post.Preview.Image = new Photo
                {
                    FileName = await _photoStorage.StorePhotoAsync(request.PreviewImage)
                };
            }

            await _mediator.Send(new UpdatePostTagsCommand {Post = post, Tags = request.Tags});

            await _forumDbContext.SaveChangesAsync();

            var result = _mapper.Map<PostDto>(post);
            return Result<PostDto>.Success(result);
        }
    }
}