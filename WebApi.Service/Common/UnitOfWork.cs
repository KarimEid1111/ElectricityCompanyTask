using Microsoft.EntityFrameworkCore.Storage;
using WebApi.Domain.Context;
using WebApi.Domain.Interfaces.Common;
using WebApi.Domain.Interfaces.Repositories;

namespace WebApi.Service.Common
{
    public class UnitOfWork(
        MyDbContext context,
        ICabinRepository cabinRepository,
        ICableRepository cableRepository,
        ICuttingDownARepository cuttingDownARepository,
        ICuttingDownBRepository cuttingDownBRepository,
        ISTAProblemTypeRepository istaProblemTypeRepository
    ) : IUnitOfWork
    {
        private IDbContextTransaction _currentTransaction;

        public ICabinRepository CabinRepository => cabinRepository;
        public ICableRepository CableRepository => cableRepository;
        public ICuttingDownARepository CuttingDownARepository => cuttingDownARepository;
        public ICuttingDownBRepository CuttingDownBRepository => cuttingDownBRepository;
        public ISTAProblemTypeRepository IstaProblemTypeRepository => istaProblemTypeRepository;


        // Save changes to the database
        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        // Begin a new database transaction
        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }

            _currentTransaction = await context.Database.BeginTransactionAsync();
        }

        // Commit the transaction and rollback if an error occurs
        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();
                await _currentTransaction.CommitAsync();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        // Rollback the transaction
        public async Task RollbackTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        // Dispose the database context
        public void Dispose()
        {
            context.Dispose();
        }
    }
}