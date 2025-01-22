using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebPortal.Models;
using WebPortalDomain.Interfaces.Common;

namespace WebPortal.Controllers;

public class IgnoredOutagesController(IUnitOfWork unitOfWork) : Controller
{
    // GET: Ignored Outages
    [HttpGet]
    public async Task<IActionResult> IgnoredOutages()
    {
        ViewData["ShowCuttingPortalNav"] = true;
        ViewData["ActivePage"] = "IgnoredOutages";

        var ignoredOutages = (await unitOfWork.CuttingDownIgnoredRepository.GetAllAsync())
            .Select(outage => new CuttingDownIgnoredViewModel
            {
                 Id= outage.Id,
                CuttingDownIncidentId = outage.CuttingDownIncidentId,
                ActualCreateDate = outage.ActualCreateDate,
                SynchCreateDate = outage.SynchCreateDate,
                CableName = outage.CableName,
                CabinName = outage.CabinName,
                CreatedUser = outage.CreatedUser
            })
            .ToList();

        return View(ignoredOutages);
    }

    // POST: Delete Ignored Outage
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        // Fetch the record by ID
        var record = await unitOfWork.CuttingDownIgnoredRepository.GetByIdAsync(id);

        if (record != null)
        {
            // Delete the record
            unitOfWork.CuttingDownIgnoredRepository.DeleteAsync(record);
            await unitOfWork.SaveChangesAsync();
        }

        return RedirectToAction("IgnoredOutages");
    }
}