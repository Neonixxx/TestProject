using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccess.Repositories;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Web.Auth;

namespace Web.Services
{
    public interface ILoginService
    {
        Task<string> GetTokenAsync(string login, string password);
    }
    
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly JwtIssuerOptions _jwtOptions;

        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        public LoginService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<string> GetTokenAsync(string login, string password)
        {
            var claims = await GetClaimsAsync(login, password);
            var jwt = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                _jwtOptions.NotBefore,
                _jwtOptions.Expiration,
                _jwtOptions.SigningCredentials);
            
            return _jwtSecurityTokenHandler.WriteToken(jwt);
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(string login, string password)
        {
            var user = await _userRepository.GetByLoginAsync(login);
            if (user == null)
            {
                throw new ApplicationException("User not found");
            }
            
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new ApplicationException("Invalid password");
            }
            
            return new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.UserGroup.Code.ToString())
            };
        }
    }
}