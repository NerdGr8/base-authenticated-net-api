using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using JK.DAL;
using JK.DAL.Models;

namespace JK.Services
{
    public class UserRecordService
    {
        private readonly RepositoryContext _repositoryContext;
        public UserRecordService(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public async Task<UserRecord> GetUserRecord(Guid id)
        {
            var UserRecordFound = await _repositoryContext.UserRecords.FirstOrDefaultAsync(x => x.ID == id);
            return UserRecordFound;
        }

        public async Task<List<UserRecord>> GetUserRecords()
        {
            var UserRecordsFound = await _repositoryContext.UserRecords.ToListAsync();
            return UserRecordsFound;
        }

        public int GetUserRecordsCount()
        {
            var UserRecordsFound = _repositoryContext.UserRecords.Count();
            return UserRecordsFound;
        }

        public async Task<UserRecord> UpdateUserRecord(UserRecord UserRecord)
        {
            var UserRecordFound = await _repositoryContext.UserRecords.FirstOrDefaultAsync(x => x.ID == UserRecord.ID);
            if (UserRecordFound == null)
            {
                return null;
            }
            UserRecordFound.Commision = UserRecord.Commision;
            UserRecordFound.Name = UserRecord.Name;
            _repositoryContext.UserRecords.Update(UserRecordFound);
            await _repositoryContext.SaveChangesAsync();
            return UserRecordFound;
        }

        public async Task<UserRecord> CreateUserRecord(UserRecord UserRecord)
        {
            var UserRecordFound = await _repositoryContext.UserRecords.FirstOrDefaultAsync(x => x.Name == UserRecord.Name);
            if (UserRecordFound != null)
            {
                return null;
            };

            await _repositoryContext.UserRecords.AddAsync(UserRecord);
            await _repositoryContext.SaveChangesAsync();
            return UserRecord;
        }

    }
}
