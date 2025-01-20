using ConsoleApp.Domain.Context;
using ConsoleApp.Domain.Entities;
using ConsoleApp.Domain.Interfaces.Repositories;
using ConsoleApp.Service.Common;

namespace ConsoleApp.Service.Repositories;

public class ZoneRepository(MyDbContext context) : GenericRepository<Zone>(context), IZoneRepository
{
    
}