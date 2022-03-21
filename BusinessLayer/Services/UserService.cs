using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccess.Repositories;
using Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer.Services
{
    public interface IUserService
    {
        public Task<List<User>> GetAllAsync();
        public Task<User> GetAsync(int userId);
        public Task<int> AddUserAsync(string login, string password);
        public Task RemoveUserAsync(int userId);
    }

    internal class UserService : IUserService
    {
        private readonly ILockService _userLockService;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IUserStateRepository _userStateRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(
            ILockService userLockService,
            IUserGroupRepository userGroupRepository,
            IUserStateRepository userStateRepository,
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher)
        {
            _userLockService = userLockService;
            _userGroupRepository = userGroupRepository;
            _userStateRepository = userStateRepository;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public Task<List<User>> GetAllAsync()
        {
            return _userRepository.GetAllAsync();
        }

        public Task<User> GetAsync(int userId)
        {
            return _userRepository.GetAsync(userId);
        }

        public async Task<int> AddUserAsync(string login, string password)
        {
            if (!_userLockService.TryGet(login, out var disposable))
            {
                ThrowUserLoginAlreadyUsedException();
            }

            using (disposable)
            {
                if (await _userRepository.ExistsWithLoginAsync(login))
                {
                    ThrowUserLoginAlreadyUsedException();
                }

                await Task.Delay(TimeSpan.FromSeconds(5));

                var user = new User
                {
                    Login = login,
                    CreatedDate = DateTime.UtcNow,
                    UserGroupId = await _userGroupRepository.GetDefaultId(),
                    UserStateId = await _userStateRepository.GetActiveIdAsync()
                };
                user.Password = _passwordHasher.HashPassword(user, password);

                await _userRepository.AddAsync(user);

                return user.UserId;
            }

            static void ThrowUserLoginAlreadyUsedException() =>
                throw new ApplicationException("This user login is already used");
        }

        public async Task RemoveUserAsync(int userId)
        {
            var login = await _userRepository.GetLoginAsync(userId);
            if (login == null || !_userLockService.TryGet(login, out var disposable))
            {
                throw new ApplicationException("User not found");
            }

            using (disposable)
            {
                var blockedUserStateId = await _userStateRepository.GetBlockedIdAsync();
                await _userRepository.DeleteAsync(userId, blockedUserStateId);
            }
        }
    }
}