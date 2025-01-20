using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebPortal.Models;
using WebPortalDomain.Entities;
using WebPortalDomain.Interfaces.Common;

namespace WebPortal.Controllers;

public class AddOutageController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public AddOutageController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> AddOutage()
    {
        ViewData["ShowCuttingPortalNav"] = true;
        ViewData["ActivePage"] = "AddOutage";
        // Fetch dropdown data using Unit of Work repositories
        var problemTypes = await _unitOfWork.FtaProblemTypeRepository.GetAllAsync();
        var networkHierarchies = await _unitOfWork.NetworkElementHierarchyRepository.GetAllAsync();
        var searchCriteria = await _unitOfWork.NetworkElementTypeRepository.GetAllAsync();

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
                Value = nh.NetworkElementHierarchyPathKey.ToString(), // Assuming Abbreviation maps to this key
                Text = nh.Abbreviation
            }).ToList(),

            SearchCriteria = searchCriteria.Select(sc => new SelectListItem
            {
                Value = sc.NetworkElementTypeKey.ToString(),
                Text = sc.NetworkElementTypeName
            }).ToList()
        };

        ViewData["ActivePage"] = "AddOutage"; // Highlight the current tab in the navigation
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddOutageViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // If the model is invalid, reload dropdown data for rendering
            model.ProblemTypes = (await _unitOfWork.FtaProblemTypeRepository.GetAllAsync())
                .Select(pt => new SelectListItem { Value = pt.ProblemTypeKey.ToString(), Text = pt.ProblemTypeName })
                .ToList();

            model.NetworkHierarchies = (await _unitOfWork.NetworkElementHierarchyRepository.GetAllAsync())
                .Select(nh => new SelectListItem { Value = nh.NetworkElementHierarchyPathKey.ToString(), Text = nh.Abbreviation })
                .ToList();

            model.SearchCriteria = (await _unitOfWork.NetworkElementTypeRepository.GetAllAsync())
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
        await _unitOfWork.CuttingDownHeaderRepository.AddAsync(newOutage);
        await _unitOfWork.SaveChangesAsync();

        return RedirectToAction("AddOutage");
    }
}
