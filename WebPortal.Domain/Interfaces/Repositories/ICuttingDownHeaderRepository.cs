using WebPortalDomain.Dtos;
using WebPortalDomain.Entities;
using WebPortalDomain.Interfaces.Common;

namespace WebPortalDomain.Interfaces.Repositories;

public interface ICuttingDownHeaderRepository : IGenericRepository<CuttingDownHeader>
{
    Task<List<CuttingDownResultDto>> SearchCuttingDownIncidents(string sourceOfCuttingDown, string problemTypeKey,
        string isClosed, string searchCriteria, string searchValue);

    Task<List<CuttingsForAddDto>> GetCuttingsByNetworkElementId(int elementId);
    Task<List<NetworkElementDto>> GetNetworkElementsAsync(int? parentId);
}