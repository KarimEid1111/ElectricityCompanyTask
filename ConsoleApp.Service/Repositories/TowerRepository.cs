using ConsoleApp.Domain.Context;
using ConsoleApp.Domain.Entities;
using ConsoleApp.Domain.Interfaces.Repositories;
using ConsoleApp.Service.Common;

namespace ConsoleApp.Service.Repositories;

public class TowerRepository(MyDbContext context) : GenericRepository<Tower>(context), ITowerRepository
{
    
}