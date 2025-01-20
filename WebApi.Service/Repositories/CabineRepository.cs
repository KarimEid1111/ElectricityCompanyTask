using WebApi.Domain.Context;
using WebApi.Domain.Entities;
using WebApi.Domain.Interfaces.Repositories;
using WebApi.Service.Common;

namespace WebApi.Service.Repositories;

public class CabineRepository(MyDbContext context) : GenericRepository<Cabin>(context), ICabinRepository
{
}