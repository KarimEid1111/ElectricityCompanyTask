using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebPortalDomain.Context;
using WebPortalDomain.Interfaces.Common;


namespace WebPortal.Service.Common
{
    public class GenericRepository<T>(MyDbContext context) : IGenericRepository<T>
        where T : class
    {

        public async Task AddAsync(T entity)
        {
            await context.Set<T>().AddAsync(entity);
        }
        
        public IQueryable<T> GetAllAsQueryable()
        {
            return context.Set<T>().AsQueryable();
        }

        public void DeleteAsync(T entity)

        {
            context.Set<T>().Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIdAsync(int? id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            context.Set<T>().AddRange(entities);
        }
        
        public void UpdateAsync(T entity)
        {
            context.Set<T>().Update(entity);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await context
                .Set<T>()
                .Where(predicate)
                .AnyAsync();
        }
        
        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().Where(predicate).ToListAsync();
        }
    }
}