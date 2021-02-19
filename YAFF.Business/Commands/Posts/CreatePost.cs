using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using YAFF.Core.Common;
using YAFF.Core.DTO;
using YAFF.Core.Entities;
using YAFF.Core.Entities.Identity;
using YAFF.Core.Interfaces;
using YAFF.Data;

namespace YAFF.Business.Commands.Posts
{
    public class CreatePostRequest : IRequest<Result<PostDto>>
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public int AuthorId { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public string PreviewBody { get; init; }
        public IFile PreviewImage { get; init; }
    }

    public class CreatePostRequestHandler : IRequestHandler<CreatePostRequest, Result<PostDto>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly UserManager<User> _userManager;
        private readonly IPhotoValidator _photoValidator;
        private readonly IPhotoStorage _photoStorage;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CreatePostRequestHandler(ForumDbContext forumDbContext, UserManager<User> userManager,
            IPhotoValidator photoValidator, IPhotoStorage photoStorage, IMediator mediator, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _userManager = userManager;
            _photoValidator = photoValidator;
            _photoStorage = photoStorage;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<Result<PostDto>> Handle(CreatePostRequest request, CancellationToken cancellationToken)
        {
            var validationResult = _photoValidator.ValidatePhoto(request.PreviewImage);
            if (!validationResult.Succeeded)
            {
                return Result<PostDto>.Failure(validationResult.Field, validationResult.Message);
            }

            var fileName = await _photoStorage.StorePhotoAsync(request.PreviewImage);
            var user = await _userManager.FindByIdAsync(request.AuthorId.ToString());

            var post = new Post
            {
                Title = request.Title,
                Body = request.Body,
                Author = user,
                DateAdded = DateTime.UtcNow,
                Preview = new PostPreview
                {
                    Body = request.PreviewBody,
                    Image = new Photo {FileName = fileName}
                }
            };
            
            await _forumDbContext.Posts.AddAsync(post);
            await _mediator.Send(new UpdatePostTagsCommand {Post = post, Tags = request.Tags});
            
            await _forumDbContext.SaveChangesAsync();

            var result = _mapper.Map<PostDto>(post);
            return Result<PostDto>.Success(result);
        }
    }
}