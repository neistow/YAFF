﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YAFF.Core.Common;
using YAFF.Core.DTO;
using YAFF.Core.Entities;
using YAFF.Core.Entities.Identity;
using YAFF.Data;

namespace YAFF.Business.Commands.Posts
{
    public class CreatePostRequest : IRequest<Result<PostDto>>
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public int AuthorId { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }

    public class CreatePostRequestHandler : IRequestHandler<CreatePostRequest, Result<PostDto>>
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public CreatePostRequestHandler(ForumDbContext forumDbContext, UserManager<User> userManager, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Result<PostDto>> Handle(CreatePostRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.AuthorId.ToString());
            if (user == null)
            {
                return Result<PostDto>.Failure();
            }

            if (user.IsBanned)
            {
                return Result<PostDto>.Failure(string.Empty, "You are banned.");
            }

            var post = new Post
            {
                Title = request.Title,
                Body = request.Body,
                Author = user,
                DateAdded = DateTime.UtcNow
            };
            await _forumDbContext.Posts.AddAsync(post);

            foreach (var tagName in request.Tags)
            {
                var tagInDb = await _forumDbContext.Tags
                    .SingleOrDefaultAsync(t => t.Name == tagName.ToLowerInvariant());
                if (tagInDb != null)
                {
                    post.PostTags.Add(new PostTag {TagId = tagInDb.Id});
                }
                else
                {
                    var tag = new Tag {Name = tagName.ToLowerInvariant()};
                    await _forumDbContext.Tags.AddAsync(tag);

                    post.PostTags.Add(new PostTag {Tag = tag});
                }
            }

            await _forumDbContext.SaveChangesAsync();

            var result = _mapper.Map<PostDto>(post);
            return Result<PostDto>.Success(result);
        }
    }
}