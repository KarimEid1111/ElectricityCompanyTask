using Microsoft.EntityFrameworkCore;
using WebPortal.Service.Common;
using WebPortalDomain.Context;
using WebPortalDomain.Dtos;
using WebPortalDomain.Entities;
using WebPortalDomain.Interfaces.Repositories;

namespace WebPortal.Service.Repositories;

public class CuttingDownHeaderRepository(MyDbContext context)
    : GenericRepository<CuttingDownHeader>(context), ICuttingDownHeaderRepository
{
    public async Task<List<CuttingDownResultDto>> SearchCuttingDownIncidents(string sourceOfCuttingDown,
        string problemTypeKey, string isClosed,
        string searchCriteria, string searchValue)
    {
        // Build the query
        var query = context.CuttingDownHeaders.AsQueryable();
        
        // Apply filters
        if (!string.IsNullOrEmpty(sourceOfCuttingDown))
        {
            query = query.Where(x => x.ChannelKey.ToString() == sourceOfCuttingDown);
        }

        if (!string.IsNullOrEmpty(problemTypeKey))
        {
            query = query.Where(x => x.CuttingDownProblemTypeKey.ToString() == problemTypeKey);
        }

        if (!string.IsNullOrEmpty(isClosed))
        {
            bool closed = isClosed == "true";
            query = closed
                ? query.Where(x => x.ActualEndDate != null) // Closed
                : query.Where(x => x.ActualEndDate == null); // Open
        }

        if (!string.IsNullOrEmpty(searchCriteria) && !string.IsNullOrEmpty(searchValue))
        {
            var matchingNetworkElements = await context.NetworkElements.Where(ne =>
                    ne.NetworkElementTypeKey.ToString() == searchCriteria &&
                    EF.Functions.Like(ne.NetworkElementName, $"%{searchValue}%"))
                .Select(ne => ne.NetworkElementKey)
                .ToListAsync();


            var allHierarchyNetworkElements = new List<int>();

            foreach (var networkElementKey in matchingNetworkElements)
            {
                var childElements = await context.GetChildNetworkElementsTillCableAsync(networkElementKey);
                var parentElements = await context.GetHigherNetworkElementsTillCabinAsync(networkElementKey);

                allHierarchyNetworkElements.AddRange(childElements);
                allHierarchyNetworkElements.AddRange(parentElements);
            }

            query = query.Where(x => x.CuttingDownDetails
                .Any(cd => cd.NetworkElementKey.HasValue &&
                           allHierarchyNetworkElements.Contains(cd.NetworkElementKey.Value)));
        }

        // Execute the query and project to the view model
        return await query
            .Select(x => new CuttingDownResultDto
            {
                CuttingDownKey = x.CuttingDownKey,
                CuttingDownIncidentId = x.CuttingDownIncidentId,
                ChannelKey = x.ChannelKey,
                CuttingDownProblemTypeKey = x.CuttingDownProblemTypeKey,
                ActualCreateDate = x.ActualCreateDate.HasValue
                    ? x.ActualCreateDate.Value.ToDateTime(TimeOnly.MinValue)
                    : (DateTime?)null,
                SynchCreateDate = x.SynchCreateDate.HasValue
                    ? x.SynchCreateDate.Value.ToDateTime(TimeOnly.MinValue)
                    : (DateTime?)null,
                SynchUpdateDate = x.SynchUpdateDate.HasValue
                    ? x.SynchUpdateDate.Value.ToDateTime(TimeOnly.MinValue)
                    : (DateTime?)null,
                ActualEndDate = x.ActualEndDate.HasValue
                    ? x.ActualEndDate.Value.ToDateTime(TimeOnly.MinValue)
                    : (DateTime?)null,
                IsPlanned = x.IsPlanned,
                IsGlobal = x.IsGlobal,
                PlannedStartDts = x.PlannedStartDts,
                PlannedEndDts = x.PlannedEndDts,
                IsActive = x.IsActive,
                CreateSystemUserId = x.CreateSystemUserId,
                UpdateSystemUserId = x.UpdateSystemUserId
            })
            .ToListAsync();
    }

    public async Task<List<CuttingsForAddDto>> GetCuttingsByNetworkElementId(int elementId)
    {
        return await context.GetCuttingsForAdd
            .FromSqlRaw($"fta.SP_Get_Cutting_Incidents_For_Element {elementId}")
            .ToListAsync();
    }
    public async Task<List<NetworkElementDto>> GetNetworkElementsAsync(int? parentId)
    {
        return await context.NetworkElements
            .Where(x => x.ParentNetworkElementKey == parentId)
            .Select(x => new NetworkElementDto
            {
                Id = x.NetworkElementKey,
                NetworkElementTypeId = x.NetworkElementTypeKey ?? 0,
                NetworkElementName = x.NetworkElementName,
                ParentElementId = x.ParentNetworkElementKey,
                Children = new List<NetworkElementDto>()
            })
            .ToListAsync();
    }
}