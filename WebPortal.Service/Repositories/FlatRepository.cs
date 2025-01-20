using WebPortal.Service.Common;
using WebPortalDomain.Context;
using WebPortalDomain.Entities;
using WebPortalDomain.Interfaces.Repositories;

namespace WebPortal.Service.Repositories;

public class FlatRepository(MyDbContext context) : GenericRepository<Flat>(context), IFlatRepository
{
    
}