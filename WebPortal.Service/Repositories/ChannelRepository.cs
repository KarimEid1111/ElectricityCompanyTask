using WebPortal.Service.Common;
using WebPortalDomain.Context;
using WebPortalDomain.Entities;
using WebPortalDomain.Interfaces.Repositories;

namespace WebPortal.Service.Repositories;

public class ChannelRepository(MyDbContext context) : GenericRepository<Channel>(context), IChannelRepository
{
    
}