using Microsoft.EntityFrameworkCore;
using WebPortal.Service.Common;
using WebPortalDomain.Context;
using WebPortalDomain.Entities;
using WebPortalDomain.Interfaces.Repositories;

namespace WebPortal.Service.Repositories;

public class ZoneRepository(MyDbContext context) : GenericRepository<Zone>(context), IZoneRepository
{
    
}