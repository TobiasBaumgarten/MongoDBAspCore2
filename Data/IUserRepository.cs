using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Mongo.Models;

namespace Mongo.Data
{
    public interface IUserRepository
    {
         Task<IdentityResult> CreateAsync(ApplicationUser user);
         Task<IdentityResult> DeleteAsync(ApplicationUser user);
         Task<ApplicationUser> FindByIdAsync(Guid userId);
         Task<ApplicationUser> FindByNameAsync(string userName);
    }
}