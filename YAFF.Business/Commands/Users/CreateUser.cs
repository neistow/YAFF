using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using YAFF.Business.Common;
using YAFF.Core.Common;
using YAFF.Core.DTO;
using YAFF.Core.Entities;
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
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Result<UserInfo>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userInDb = await _userRepository.GetUserByEmailAsync(request.Email);
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
            var result = await _userRepository.AddAsync(user);
            if (result != 1)
            {
                return Result<UserInfo>.Failure();
            }

            // TODO: send register verification email

            return Result<UserInfo>.Success(_mapper.Map<UserInfo>(user));
        }
    }
}