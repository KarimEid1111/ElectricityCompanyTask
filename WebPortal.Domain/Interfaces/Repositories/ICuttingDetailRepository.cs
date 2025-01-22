using WebPortalDomain.Entities;
using WebPortalDomain.Interfaces.Common;
using WebPortalDomain.Payloads;

namespace WebPortalDomain.Interfaces.Repositories;

public interface ICuttingDetailRepository : IGenericRepository<CuttingDownDetail>
{
    Task<int> CloseCuttingAsync(int id);
    Task<int> CloseAllCuttingsAsync(CloseAllCuttingsPayload request);
}