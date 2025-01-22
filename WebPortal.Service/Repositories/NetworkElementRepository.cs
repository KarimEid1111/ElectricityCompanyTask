    using Microsoft.EntityFrameworkCore;
    using WebPortal.Service.Common;
    using WebPortalDomain.Context;
    using WebPortalDomain.Dtos;
    using WebPortalDomain.Entities;
    using WebPortalDomain.Interfaces.Repositories;

    namespace WebPortal.Service.Repositories;

    public class NetworkElementRepository(MyDbContext context) : GenericRepository<NetworkElement>(context), INetworkElementRepository
    {
        public async Task<List<NetworkElementDto>> GetFirstLevelChildElementsAsync(int parentId)
        {
            var childElements = new List<NetworkElementDto>();

            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SP_Get_First_Level_Child_Network_Elements";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var param = command.CreateParameter();
                param.ParameterName = "@ParentId";
                param.Value = parentId;
                command.Parameters.Add(param);

                await context.Database.OpenConnectionAsync();

                using (var result = await command.ExecuteReaderAsync())
                {
                    while (await result.ReadAsync())
                    {
                        childElements.Add(new NetworkElementDto
                        {
                            Id = result.GetInt32(0), // Assuming first column is the ID
                            Name = result.GetString(1), // Assuming second column is the name
                            HasChildren = result.GetBoolean(2) // Assuming third column indicates if it has children
                        });
                    }
                }

                await context.Database.CloseConnectionAsync();
            }

            return childElements;
        }
    }