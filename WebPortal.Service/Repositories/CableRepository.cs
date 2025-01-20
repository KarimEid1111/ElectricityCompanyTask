using WebPortal.Service.Common;
using WebPortalDomain.Context;
using WebPortalDomain.Entities;
using WebPortalDomain.Interfaces.Repositories;

namespace WebPortal.Service.Repositories;

public class CableRepository(MyDbContext context) : GenericRepository<Cable>(context), ICableRepository
{
    
}