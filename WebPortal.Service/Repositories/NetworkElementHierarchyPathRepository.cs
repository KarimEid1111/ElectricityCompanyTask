using Microsoft.EntityFrameworkCore;
using WebPortal.Service.Common;
using WebPortalDomain.Context;
using WebPortalDomain.Entities;
using WebPortalDomain.Interfaces.Repositories;

namespace WebPortal.Service.Repositories;

public class NetworkElementHierarchyPathRepository(MyDbContext context) : GenericRepository<NetworkElementHierarchyPath>(context), INetworkElementHierarchyPathRepository
{
    
}