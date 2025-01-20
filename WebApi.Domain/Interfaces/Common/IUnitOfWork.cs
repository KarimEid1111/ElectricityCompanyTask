using WebApi.Domain.Interfaces.Repositories;

namespace WebApi.Domain.Interfaces.Common
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        ICabinRepository CabinRepository { get; }
        ICableRepository CableRepository { get; }
        ICuttingDownARepository CuttingDownARepository { get; }
        ICuttingDownBRepository CuttingDownBRepository { get; }
        ISTAProblemTypeRepository IstaProblemTypeRepository { get; }
    }
}