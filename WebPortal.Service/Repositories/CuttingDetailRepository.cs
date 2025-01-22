using Microsoft.EntityFrameworkCore;
using WebPortal.Service.Common;
using WebPortalDomain.Context;
using WebPortalDomain.Entities;
using WebPortalDomain.Interfaces.Repositories;
using WebPortalDomain.Payloads;

namespace WebPortal.Service.Repositories;

public class CuttingDetailRepository(MyDbContext context)
    : GenericRepository<CuttingDownDetail>(context), ICuttingDetailRepository
{
    public override async Task<CuttingDownDetail> GetByIdAsync(int? id)
    {
        return await context.Set<CuttingDownDetail>()
            .Include(i => i.CuttingDownKeyNavigation)
            .FirstOrDefaultAsync(i => i.CuttingDownKey == id);
    }

    public async Task<int> CloseCuttingAsync(int id)
    {
        var detail = await context.CuttingDownDetails
            .Include(i => i.CuttingDownKeyNavigation)
            .FirstOrDefaultAsync(x => x.CuttingDownKey == id);

        var header = await context.CuttingDownHeaders.FirstOrDefaultAsync(x => x.CuttingDownKey == id);

        detail.ActualEndDate = DateOnly.FromDateTime(DateTime.Now);
        var user = await context.Users.FirstOrDefaultAsync(i => i.Name == "Manual");
        header.IsActive = false;
        header.ActualEndDate = DateOnly.FromDateTime(DateTime.Now);
        header.UpdateSystemUserId = user.UserKey;
        await context.SaveChangesAsync();
        return user.UserKey;
    }

    public async Task<int> CloseAllCuttingsAsync(CloseAllCuttingsPayload request)
    {
        var cuttingDownDetails = await context.CuttingDownDetails
            .Where(x =>
            request.HeaderIds.Contains(x.CuttingDownKeyNavigation!.CuttingDownKey)
            && (x.CuttingDownKeyNavigation.IsActive ?? false))
            .Include(i => i.CuttingDownKeyNavigation)
            .ToListAsync();

        var cuttingDownHeaders = cuttingDownDetails
                .Select(i => i.CuttingDownKeyNavigation)
                .ToList();
        
        foreach (var detail in cuttingDownDetails)
        {
            detail.ActualEndDate = DateOnly.FromDateTime(DateTime.Now);
        }

        var users = await context.Users.FirstOrDefaultAsync(i => i.Name == "Manual");

        foreach (var header in cuttingDownHeaders)
        {
            header.IsActive = false;
            header.ActualEndDate = DateOnly.FromDateTime(DateTime.Now);
            header.UpdateSystemUserId = users.UserKey;
        }

        await context.SaveChangesAsync();
        return users.UserKey;
    }
}