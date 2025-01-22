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
            }).ToList(),
            NetworkElements = await unitOfWork.CuttingDownHeaderRepository.GetNetworkElementsAsync(null)
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

        var cuttingDetail = new CuttingDownDetail();
        var cuttingHeader = new CuttingDownHeader();
        var channels = await unitOfWork.ChannelRepository.GetAllAsync();
        cuttingHeader.ChannelKey = model.HierarchyAbbreviation == "Governrate -> Individual Subscription"
            ? channels.Where(x => x.ChannelName == "Source A")
                .Select(x => x.ChannelKey)
                .FirstOrDefault()
            : channels.Where(x => x.ChannelName == "Source B")
                .Select(x => x.ChannelKey)
                .FirstOrDefault();
        cuttingHeader.IsActive = true;
        cuttingHeader.IsGlobal = false;
        cuttingHeader.IsPlanned = false;
        cuttingHeader.ActualCreateDate = model.StartDate;
        var user = await unitOfWork.UserRepository.GetSingleAsync(i => i.Name == "admin");
        cuttingHeader.CreateSystemUserId = user?.UserKey;
        cuttingHeader.CuttingDownProblemTypeKey = model.ProblemTypeKey;
        cuttingHeader.SynchCreateDate = DateOnly.FromDateTime(DateTime.Now);
        cuttingDetail.ActualCreateDate = model.StartDate;
        cuttingDetail.ImpactedCustomers =
            await unitOfWork.NetworkElementRepository.GetAffectedCustomersAsync(model.NetworkElementId);
        cuttingDetail.NetworkElementKey = model.NetworkElementId;
        cuttingDetail.CuttingDownKeyNavigation = cuttingHeader;
        await unitOfWork.CuttingDetailRepository.AddAsync(cuttingDetail);
        await unitOfWork.SaveChangesAsync();

        return RedirectToAction("AddOutage");
    }
}