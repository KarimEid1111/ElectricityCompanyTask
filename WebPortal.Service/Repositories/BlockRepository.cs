using WebPortal.Service.Common;
using WebPortalDomain.Context;
using WebPortalDomain.Entities;
using WebPortalDomain.Interfaces.Repositories;

namespace WebPortal.Service.Repositories;

public class BlockRepository(MyDbContext context) : GenericRepository<Block>(context), IBlockRepository
{
    
}