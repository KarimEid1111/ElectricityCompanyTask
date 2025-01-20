using ConsoleApp.Domain.Interfaces.Repositories;

namespace ConsoleApp.Domain.Interfaces.Common
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        IBlockRepository BlockRepository { get; }
        IBuildingRepository BuildingRepository { get; }
        ICabinRepository CabinRepository { get; }
        ICableRepository CableRepository { get; }
        IChannelRepository ChannelRepository { get; }
        ICityRepository CityRepository { get; }
        IFlatRepository FlatRepository { get; }
        IGovernrateRepository GovernrateRepository { get; }
        INetworkElementHierarchyPathRepository NetworkElementHierarchyRepository { get; }
        INetworkElementRepository NetworkElementRepository { get; }
        INetworkElementTypeRepository NetworkElementTypeRepository { get; }
        IFtaProblemTypeRepository FtaProblemTypeRepository { get; }
        ISectorRepository SectorRepository { get; }
        IStationRepository StationRepository { get; }
        ISubscribtionRepository SubscriptionRepository { get; }
        ITowerRepository TowerRepository { get; }
        IUserRepository UserRepository { get; }
        IZoneRepository ZoneRepository { get; }
        IStaProblemTypeRepository StaProblemTypeRepository { get; }
    }
}