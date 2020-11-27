using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using YAFF.Core.Common;
using YAFF.Core.DTO;
using YAFF.Core.Entities;
using YAFF.Core.Interfaces.Data;

namespace YAFF.Business.Commands.Posts
{
    public class CreatePostRequest : IRequest<Result<PostDto>>
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public Guid AuthorId { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
    }

    public class CreatePostRequestHandler : IRequestHandler<CreatePostRequest, Result<PostDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreatePostRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<PostDto>> Handle(CreatePostRequest request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.AuthorId);
            if (user == null)
            {
                return Result<PostDto>.Failure();
            }

            if (user.IsBanned)
            {
                return Result<PostDto>.Failure(string.Empty, "You are banned.");
            }

            var tags = new List<Tag>();
            foreach (var tagId in request.Tags)
            {
                var tag = await _unitOfWork.TagRepository.GetTag(tagId);
                if (tag == null)
                {
                    return Result<PostDto>.Failure(nameof(request.Tags), $"Tag with id {tagId} doesnt exist");
                }

                tags.Add(tag);
            }

            var post = new Post
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Body = request.Body,
                AuthorId = request.AuthorId,
                DatePosted = DateTime.UtcNow,
                Tags = tags
            };
            await _unitOfWork.PostRepository.AddAsync(post);

            var postTags = post.Tags.Select(t => new PostTag {PostId = post.Id, TagId = t.TagId});
            foreach (var postTag in postTags)
            {
                await _unitOfWork.TagRepository.AddPostTag(postTag);
            }

            var result = _mapper.Map<PostDto>(post);
            return Result<PostDto>.Success(result);
        }
    }
}