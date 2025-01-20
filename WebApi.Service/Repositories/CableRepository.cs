using WebApi.Domain.Context;
using WebApi.Domain.Entities;
using WebApi.Domain.Interfaces.Repositories;
using WebApi.Service.Common;

namespace WebApi.Service.Repositories;

public class CableRepository(MyDbContext context) : GenericRepository<Cable>(context), ICableRepository
{
    
}