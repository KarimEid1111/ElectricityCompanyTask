using WebPortal.Service.Common;
using WebPortalDomain.Context;
using WebPortalDomain.Entities;
using WebPortalDomain.Interfaces.Repositories;

namespace WebPortal.Service.Repositories;

public class CuttingDownIgnoredRepository(MyDbContext context) : GenericRepository<CuttingDownIgnored>(context), ICuttingDownIgnoredRepository
{
    
}