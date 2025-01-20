using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPortalDomain.Context;
using WebPortal.Models;
using WebPortalDomain.Entities;
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
        // Fetch data using Unit of Work and map to the CuttingDownIgnoredViewModel
        var ignoredOutages = (await unitOfWork.CuttingDownIgnoredRepository.GetAllAsync())
            .Select(outage => new CuttingDownIgnoredViewModel
            {
                Id = outage.Id, // Map the primary key
                CuttingDownIncidentId = outage.CuttingDownIncidentId,
                ActualCreateDate = outage.ActualCreateDate,
                SynchCreateDate = outage.SynchCreateDate,
                CableName = outage.CableName,
                CabinName = outage.CabinName,
                CreatedUser = outage.CreatedUser
            })
            .ToList();

        return View(ignoredOutages); // Pass the correct model to the view
    }

    // POST: Delete Ignored Outage
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var record = await unitOfWork.CuttingDownIgnoredRepository.GetByIdAsync(id);

        if (record != null)
        {
            unitOfWork.CuttingDownIgnoredRepository.DeleteAsync(record);
            await unitOfWork.SaveChangesAsync(); // Commit the changes
        }

        return RedirectToAction("IgnoredOutages");
    }

}