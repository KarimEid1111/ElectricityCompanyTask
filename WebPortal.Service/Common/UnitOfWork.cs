using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using WebPortal.Service.Repositories;
using WebPortalDomain.Context;
using WebPortalDomain.Interfaces.Common;
using WebPortalDomain.Interfaces.Repositories;

namespace WebPortal.Service.Common
{
    public class UnitOfWork(
        MyDbContext context,
        IBlockRepository blockRepository,
        IBuildingRepository buildingRepository,
        ICabinRepository cabinRepository,
        ICableRepository cableRepository,
        IChannelRepository channelRepository,
        ICityRepository cityRepository,
        IFlatRepository flatRepository,
        IGovernrateRepository governrateRepository,
        INetworkElementHierarchyPathRepository networkElementHierarchyRepository,
        INetworkElementRepository networkElementRepository,
        INetworkElementTypeRepository networkElementTypeRepository,
        IFtaProblemTypeRepository ftaProblemTypeRepository,
        IStaProblemTypeRepository staProblemTypeRepository,
        ISectorRepository sectorRepository,
        IStationRepository stationRepository,
        ISubscribtionRepository subscriptionRepository,
        ITowerRepository towerRepository,
        IUserRepository userRepository,
        ICuttingDownHeaderRepository cuttingDownHeaderRepository,
        ICuttingDownIgnoredRepository cuttingDownIgnoredRepository,
        ICuttingDetailRepository cuttingDownDetailRepository,
        IZoneRepository zoneRepository) : IUnitOfWork
    {
        private IDbContextTransaction _currentTransaction;

        public IBlockRepository BlockRepository => blockRepository;
        public IBuildingRepository BuildingRepository => buildingRepository;
        public ICabinRepository CabinRepository => cabinRepository;
        public ICableRepository CableRepository => cableRepository;
        public IChannelRepository ChannelRepository => channelRepository;
        public ICityRepository CityRepository => cityRepository;
        public IFlatRepository FlatRepository => flatRepository;
        public IGovernrateRepository GovernrateRepository => governrateRepository;
        public INetworkElementHierarchyPathRepository NetworkElementHierarchyRepository => networkElementHierarchyRepository;
        public INetworkElementRepository NetworkElementRepository => networkElementRepository;
        public INetworkElementTypeRepository NetworkElementTypeRepository => networkElementTypeRepository;
        public IFtaProblemTypeRepository FtaProblemTypeRepository => ftaProblemTypeRepository;
        public IStaProblemTypeRepository StaProblemTypeRepository => staProblemTypeRepository;
        public ISectorRepository SectorRepository => sectorRepository;
        public IStationRepository StationRepository => stationRepository;
        public ISubscribtionRepository SubscriptionRepository => subscriptionRepository;
        public ITowerRepository TowerRepository => towerRepository;
        public IUserRepository UserRepository => userRepository;
        public IZoneRepository ZoneRepository => zoneRepository;
        public ICuttingDownHeaderRepository CuttingDownHeaderRepository => cuttingDownHeaderRepository;
        public ICuttingDownIgnoredRepository CuttingDownIgnoredRepository => cuttingDownIgnoredRepository;
        public ICuttingDetailRepository CuttingDetailRepository => cuttingDownDetailRepository;

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
