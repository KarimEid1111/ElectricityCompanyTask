using WebPortal.Service.Common;
using WebPortalDomain.Context;
using WebPortalDomain.Entities;
using WebPortalDomain.Interfaces.Repositories;

namespace WebPortal.Service.Repositories;

public class CityRepository(MyDbContext context) : GenericRepository<City>(context), ICityRepository
{
    
}