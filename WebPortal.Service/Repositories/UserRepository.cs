using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebPortal.Service.Common;
using WebPortalDomain.Context;
using WebPortalDomain.Entities;
using WebPortalDomain.Interfaces.Repositories;

namespace WebPortal.Service.Repositories;

public class UserRepository(MyDbContext context) : GenericRepository<User>(context), IUserRepository
{
    public async Task<User?> GetSingleAsync(Expression<Func<User, bool>> predicate)
    {
        return await context.Users.FirstOrDefaultAsync(predicate);
    }
}