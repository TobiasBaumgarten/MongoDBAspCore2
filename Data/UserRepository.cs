using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Mongo.Models;
using MongoDB.Driver;

namespace Mongo.Data
{
    public class UserRepository : IUserRepository
    {
        private MongoContext _context;

        public UserRepository(IOptions<MongoDbSettings> settings)
        {
            _context = new MongoContext(settings);
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            try
            {
                await _context.Users.InsertOneAsync(user);
                return IdentityResult.Success;
            }
            catch (System.Exception)
            {
                return FailedOperation($"Could not insert user {user.UserName}.");
            }
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user)
        {
            try
            {
                var result =
                    await _context.Users.DeleteOneAsync(Builders<ApplicationUser>.Filter.Eq(au => au.Id, user.Id));
                if (result.DeletedCount > 0)
                {
                    return IdentityResult.Success;
                }
                return FailedOperation($"Could not remove {user.UserName}");
            }
            catch (System.Exception)
            {
                return FailedOperation($"Could not remove {user.UserName}");
            }
        }

        public async Task<ApplicationUser> FindByIdAsync(Guid userId)
        {
            return await _context.Users
                 .Find(u => u.Id == userId)
                 .FirstOrDefaultAsync();
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            var normUserName = userName.ToUpper();
            return await _context.Users
                .Find(u => u.NormalizedUserName == normUserName)
                .FirstOrDefaultAsync();
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            var normEmail = email.ToUpper();
            return await _context.Users
                .Find(u => u.NormalizedEmail == normEmail)
                .FirstOrDefaultAsync();
        }

        private IdentityResult FailedOperation(string msg) =>
            IdentityResult.Failed(new IdentityError { Description = msg });
    }
}