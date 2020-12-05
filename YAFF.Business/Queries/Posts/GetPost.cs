using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using YAFF.Core.Common;
using YAFF.Core.DTO;
using YAFF.Core.Entities;
using YAFF.Core.Interfaces.Data;

namespace YAFF.Business.Queries.Posts
{
    public class GetPostQuery : IRequest<Result<PostDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetPostQueryHandler : IRequestHandler<GetPostQuery, Result<PostDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPostQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<PostDto>> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            var post = await _unitOfWork.PostRepository.GetPostAsync(request.Id);
            if (post == null)
            {
                return Result<PostDto>.Failure(nameof(request.Id), "Post doesnt exist");
            }

            var result = _mapper.Map<PostDto>(post);
            return Result<PostDto>.Success(result);
        }
    }
}