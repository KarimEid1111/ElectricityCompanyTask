using Microsoft.EntityFrameworkCore;
using WebPortal.Service.Common;
using WebPortalDomain.Context;
using WebPortalDomain.Dtos;
using WebPortalDomain.Entities;
using WebPortalDomain.Interfaces.Repositories;

namespace WebPortal.Service.Repositories;

public class NetworkElementRepository(MyDbContext context)
    : GenericRepository<NetworkElement>(context), INetworkElementRepository
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

    public async Task<int?> GetAffectedCustomersAsync(int networkElementId)
    {
        var result = await context.NumberOfAffectedCustomers
            .FromSqlRaw($"FTA.SP_Get_Affected_Customers {networkElementId}")
            .ToListAsync();
        return result.Select(x => x.AffectedCustomers).FirstOrDefault();
    }

    public async Task<NetworkElementDto> GetHierarchyPath(string searchValue)
    {
        // Fetch the elements from the stored procedure
        var elements = await context.GetNetworkHierarchyPath
            .FromSqlRaw($"fta.SP_GetHierarchyPath '{searchValue}'")
            .ToListAsync();

        // Create a dictionary to quickly access elements by their ID
        var elementDictionary = elements.ToDictionary(
            x => x.NetworkElementKey,
            x => new NetworkElementDto
            {
                Id = x.NetworkElementKey,
                Name = x.NetworkElementName,
                NetworkElementTypeId = x.NetworkElementTypeKey,
                ParentElementId = x.ParentNetworkElementKey,
                Children = new List<NetworkElementDto>(),
                NetworkElementName = x.NetworkElementName
            });

        // Populate the parent-child relationships
        foreach (var element in elementDictionary.Values)
        {
            if (element.ParentElementId != null)
            {
                if (elementDictionary.TryGetValue(element.ParentElementId.Value, out var parent))
                {
                    parent.Children.Add(element);
                }
            }
        }

        // Find and return the top-level parent
        var root = elementDictionary.Values.FirstOrDefault(x => x.ParentElementId == null);
        return root;
    }

}