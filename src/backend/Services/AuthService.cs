using System;
using JK.DAL.Models;
using JK.Helpers;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using JK.DAL;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JK.Services
{
    public class AuthService
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly JwtService _jwtService;
        public AuthService(IOptions<Settings> options, JwtService jwtService, RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _jwtService = jwtService;
        }

        public async Task<object> LoginUser(string email, string password)
        {
            var userFound = await _repositoryContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (userFound == null)
            {
                return null;
            };

            //verify user password
            var passwordValid = CryptoService.VerifyHashedPassword(userFound.Password, password);
            if (!passwordValid)
            {
                return null;
            }

            userFound.Password = null;
            //created successfull respponse
            var payload = new
            {
                status = true,
                message = "User successfully authenticated",
                token = _jwtService.Authenticate(Convert.ToString(userFound.Id), userFound.Email, userFound.Usertype),
                data = userFound
            };

            return payload;

        }

        public async Task<User> CreateUser(User user)
        {
            //find user first
            var userFound = await _repositoryContext.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (userFound != null)
            {
                return null;
            };

            //todo send to messaging gateway to send email to created user account
            user.Password = CryptoService.HashPassword(user.Password);
            await _repositoryContext.Users.AddAsync(user);
            await _repositoryContext.SaveChangesAsync();
            return user;
        }
    }
}

