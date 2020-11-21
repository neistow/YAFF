using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using YAFF.Business.Common;
using YAFF.Core.Common;
using YAFF.Core.DTO;
using YAFF.Core.Entities;
using YAFF.Core.Interfaces.Data;
using YAFF.Core.Interfaces.Repositories;

namespace YAFF.Business.Commands.Users
{
    public class CreateUserCommand : IRequest<Result<UserInfo>>
    {
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserInfo>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<UserInfo>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userInDb = await _unitOfWork.UserRepository.GetUserByEmailAsync(request.Email);
            if (userInDb.Id != Guid.Empty)
            {
                return Result<UserInfo>.Failure(nameof(request.Email), "User with such email already exists");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                NickName = request.NickName,
                Email = request.Email,
                PasswordHash = PasswordHasher.HashPassword(request.Password),
                RegistrationDate = DateTime.UtcNow
            };
            
            await _unitOfWork.UserRepository.AddAsync(user);
            _unitOfWork.Commit();

            // TODO: send register verification email

            return Result<UserInfo>.Success(_mapper.Map<UserInfo>(user));
        }
    }
}