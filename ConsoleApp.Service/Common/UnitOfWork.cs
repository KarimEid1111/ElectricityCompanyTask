using ConsoleApp.Domain.Context;
using ConsoleApp.Domain.Interfaces.Common;
using ConsoleApp.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace ConsoleApp.Service.Common
{
    public class UnitOfWork(
        MyDbContext context,
        Lazy<IBlockRepository> blockRepository,
        Lazy<IBuildingRepository> buildingRepository,
        Lazy<ICabinRepository> cabinRepository,
        Lazy<ICableRepository> cableRepository,
        Lazy<IChannelRepository> channelRepository,
        Lazy<ICityRepository> cityRepository,
        Lazy<IFlatRepository> flatRepository,
        Lazy<IGovernrateRepository> governrateRepository,
        Lazy<INetworkElementHierarchyPathRepository> networkElementHierarchyRepository,
        Lazy<INetworkElementRepository> networkElementRepository,
        Lazy<INetworkElementTypeRepository> networkElementTypeRepository,
        Lazy<IFTAProblemTypeRepository> ftaProblemTypeRepository,
        Lazy<ISTAProblemTypeRepository> staProblemTypeRepository,
        Lazy<ISectorRepository> sectorRepository,
        Lazy<ISTAtionRepository> stationRepository,
        Lazy<ISubscribtionRepository> subscriptionRepository,
        Lazy<ITowerRepository> towerRepository,
        Lazy<IUserRepository> userRepository,
        Lazy<IZoneRepository> zoneRepository
    ) : IUnitOfWork
    {
        private IDbContextTransaction _currentTransaction;

        public IBlockRepository BlockRepository => blockRepository.Value;
        public IBuildingRepository BuildingRepository => buildingRepository.Value;
        public ICabinRepository CabinRepository => cabinRepository.Value;
        public ICableRepository CableRepository => cableRepository.Value;
        public IChannelRepository ChannelRepository => channelRepository.Value;
        public ICityRepository CityRepository => cityRepository.Value;
        public IFlatRepository FlatRepository => flatRepository.Value;
        public IGovernrateRepository GovernrateRepository => governrateRepository.Value;
        public INetworkElementHierarchyPathRepository NetworkElementHierarchyRepository => networkElementHierarchyRepository.Value;
        public INetworkElementRepository NetworkElementRepository => networkElementRepository.Value;
        public INetworkElementTypeRepository NetworkElementTypeRepository => networkElementTypeRepository.Value;
        public IFTAProblemTypeRepository IftaProblemTypeRepository => ftaProblemTypeRepository.Value;
        public ISTAProblemTypeRepository IstaProblemTypeRepository => staProblemTypeRepository.Value;
        public ISectorRepository SectorRepository => sectorRepository.Value;
        public ISTAtionRepository IstAtionRepository => stationRepository.Value;
        public ISubscribtionRepository SubscriptionRepository => subscriptionRepository.Value;
        public ITowerRepository TowerRepository => towerRepository.Value;
        public IUserRepository UserRepository => userRepository.Value;
        public IZoneRepository ZoneRepository => zoneRepository.Value;

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
