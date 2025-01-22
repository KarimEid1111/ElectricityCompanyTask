using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebPortal.Models;
using WebPortalDomain.Interfaces.Common;
using WebPortalDomain.Dtos;
using WebPortalDomain.Entities;

namespace WebPortal.Controllers;

public class AddOutageController(IUnitOfWork unitOfWork) : Controller
{
    [HttpGet]
    public async Task<IActionResult> AddOutage()
    {
        ViewData["ShowCuttingPortalNav"] = true;
        ViewData["ActivePage"] = "AddOutage";

        // Fetch dropdown data using Unit of Work repositories
        var problemTypes = await unitOfWork.FtaProblemTypeRepository.GetAllAsync();
        var networkHierarchies = await unitOfWork.NetworkElementHierarchyRepository.GetAllAsync();
        var searchCriteria = await unitOfWork.NetworkElementTypeRepository.GetAllAsync();

        // Map data to view model
        var model = new AddOutageViewModel
        {
            
            ProblemTypes = problemTypes.Select(pt => new SelectListItem
            {
                
                Value = pt.ProblemTypeKey.ToString(),
                Text = pt.ProblemTypeName
            }).ToList(),

            NetworkHierarchies = networkHierarchies.Select(nh => new SelectListItem
            {
                Value = nh.NetworkElementHierarchyPathKey.ToString(),
                Text = nh.Abbreviation
            }).ToList(),

            SearchCriteria = searchCriteria.Select(sc => new SelectListItem
            {
                Value = sc.NetworkElementTypeKey.ToString(),
                Text = sc.NetworkElementTypeName
            }).ToList()
        };

        return View(model);
    }

    [HttpGet("api/AddOutage/GetChildren/{parentId}")]
    public async Task<IActionResult> GetChildren(int parentId)
    {
        // Call repository method to fetch child elements
        var childElements = await unitOfWork.NetworkElementRepository.GetFirstLevelChildElementsAsync(parentId);

        // Ensure the result contains data
        if (childElements == null || !childElements.Any())
        {
            return NotFound("No child elements found.");
        }

        return Ok(childElements);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddOutageViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // Reload dropdown data for rendering in case of validation errors
            model.ProblemTypes = (await unitOfWork.FtaProblemTypeRepository.GetAllAsync())
                .Select(pt => new SelectListItem { Value = pt.ProblemTypeKey.ToString(), Text = pt.ProblemTypeName })
                .ToList();

            model.NetworkHierarchies = (await unitOfWork.NetworkElementHierarchyRepository.GetAllAsync())
                .Select(nh => new SelectListItem { Value = nh.NetworkElementHierarchyPathKey.ToString(), Text = nh.Abbreviation })
                .ToList();

            model.SearchCriteria = (await unitOfWork.NetworkElementTypeRepository.GetAllAsync())
                .Select(sc => new SelectListItem { Value = sc.NetworkElementTypeKey.ToString(), Text = sc.NetworkElementTypeName })
                .ToList();

            return View("AddOutage", model);
        }

        // Map ViewModel to Entity
        var newOutage = new CuttingDownHeader
        {
            CuttingDownIncidentId = model.IncidentId,
            ChannelKey = model.ChannelKey,
            CuttingDownProblemTypeKey = model.ProblemTypeKey,
            ActualCreateDate = DateOnly.FromDateTime(DateTime.Now),
            IsActive = true,
            IsPlanned = model.IsPlanned
        };

        // Add outage through Unit of Work
        await unitOfWork.CuttingDownHeaderRepository.AddAsync(newOutage);
        await unitOfWork.SaveChangesAsync();

        return RedirectToAction("AddOutage");
    }
}
