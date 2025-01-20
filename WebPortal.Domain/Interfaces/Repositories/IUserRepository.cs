using System.Linq.Expressions;
using WebPortalDomain.Entities;
using WebPortalDomain.Interfaces.Common;

namespace WebPortalDomain.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetSingleAsync(Expression<Func<User, bool>> predicate);
}