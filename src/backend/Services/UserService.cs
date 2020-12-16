using System;
using JK.DAL.Models;
using JK.Helpers;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using JK.DAL;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using JK.DAL.Mappings;
using Newtonsoft.Json;
using System.Linq;

namespace JK.Services
{
    public class UserService
    {
        private readonly RepositoryContext _repositoryContext;
        public UserService(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public async Task<User> GetUser(Guid id)
        {
            //find users first
            var userFound = await _repositoryContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            return userFound;
        }

        public int GetUsersCount()
        {
            //find users first
            var usersFound = _repositoryContext.Users.Count();
            return usersFound;
        }

        public async Task<List<User>> GetUsers()
        {
            //find users first
            var usersFound = await _repositoryContext.Users.ToListAsync();
            return usersFound;
        }

        public async Task<User> UpdateUser(User admin)
        {
            //find users first
            Console.WriteLine($"Entity: {JsonConvert.SerializeObject(admin)}");
            var userFound = await _repositoryContext.Users.FirstOrDefaultAsync(x => x.Id == admin.Id);
            Console.WriteLine($"Entity: {JsonConvert.SerializeObject(userFound)}");
            if (userFound == null)
            {
                return null;
            }

            userFound.Name = admin.Name;
            userFound.Surname = admin.Surname;
            _repositoryContext.Users.Update(userFound);
            await _repositoryContext.SaveChangesAsync();
            return admin;
        }

    }
}

