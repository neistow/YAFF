﻿using System.Data;
using YAFF.Core.Interfaces.Data;
using YAFF.Core.Interfaces.Repositories;
using YAFF.Data.Repositories;

namespace YAFF.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        private IRefreshTokenRepository _tokenRepository;

        private IPostRepository _postRepository;
        private ITagRepository _tagRepository;
        private ICommentRepository _commentRepository;
        private ILikeRepository _likeRepository;

        private readonly IDbConnection _connection;

        public UnitOfWork(IDbConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.CreateConnection();
        }

        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_connection);
        public IRoleRepository RoleRepository => _roleRepository ??= new RoleRepository(_connection);

        public IRefreshTokenRepository RefreshTokenRepository =>
            _tokenRepository ??= new RefreshTokenRepository(_connection);

        public IPostRepository PostRepository => _postRepository ??= new PostRepository(_connection);
        public ITagRepository TagRepository => _tagRepository ??= new TagRepository(_connection);
        public ICommentRepository CommentRepository => _commentRepository ??= new CommentRepository(_connection);
        public ILikeRepository LikeRepository => _likeRepository ??= new LikeRepository(_connection);

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}