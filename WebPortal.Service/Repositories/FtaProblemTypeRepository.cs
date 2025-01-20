using WebPortal.Service.Common;
using WebPortalDomain.Context;
using WebPortalDomain.Entities;
using WebPortalDomain.Interfaces.Repositories;

namespace WebPortal.Service.Repositories;

public class FtaProblemTypeRepository(MyDbContext context) : GenericRepository<FTAProblemType>(context), IFtaProblemTypeRepository
{
    
}