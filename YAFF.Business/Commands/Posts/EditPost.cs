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
using YAFF.Core.Extensions;
using YAFF.Core.Interfaces.Data;

namespace YAFF.Business.Commands.Posts
{
    public class EditPostCommand : IRequest<Result<PostDto>>
    {
        public Guid PostId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public IEnumerable<Guid> Tags { get; set; }

        public Guid EditorId { get; set; }
    }

    public class EditPostCommandHandler : IRequestHandler<EditPostCommand, Result<PostDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EditPostCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<PostDto>> Handle(EditPostCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.EditorId);
            if (user == null)
            {
                return Result<PostDto>.Failure();
            }

            if (user.IsBanned)
            {
                return Result<PostDto>.Failure(string.Empty, "You are banned.");
            }

            var post = await _unitOfWork.PostRepository.GetPostAsync(request.PostId);
            if (post == null)
            {
                return Result<PostDto>.Failure(nameof(request.PostId), "Post not found");
            }

            if (post.AuthorId != user.Id && !user.CanManagePosts())
            {
                return Result<PostDto>.Failure(nameof(request.PostId), "You are not allowed to edit post");
            }

            var tags = new List<Tag>();
            foreach (var tagId in request.Tags)
            {
                var tag = await _unitOfWork.TagRepository.GetTagAsync(tagId);
                if (tag == null)
                {
                    return Result<PostDto>.Failure(nameof(request.Tags), $"Tag with id {tagId} doesnt exist");
                }

                tags.Add(tag);
            }

            post.Title = request.Title;
            post.Body = request.Body;
            post.Tags = tags;
            post.DateEdited = DateTime.UtcNow;

            await _unitOfWork.PostRepository.UpdatePostAsync(post);

            var postTags = tags.Select(t => new PostTag {PostId = post.Id, TagId = t.TagId}).ToList();
            await _unitOfWork.TagRepository.UpdatePostTagsAsync(post.Id, postTags);

            return Result<PostDto>.Success(_mapper.Map<PostDto>(post));
        }
    }
}