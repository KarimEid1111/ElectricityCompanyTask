using WebPortalDomain.Dtos;
using WebPortalDomain.Entities;
using WebPortalDomain.Interfaces.Common;

namespace WebPortalDomain.Interfaces.Repositories;

public interface INetworkElementRepository : IGenericRepository<NetworkElement>
{
    Task<List<NetworkElementDto>> GetFirstLevelChildElementsAsync(int parentId);
    Task<int?> GetAffectedCustomersAsync(int networkElementId);
    Task<NetworkElementDto> GetHierarchyPath(string searchValue);
}