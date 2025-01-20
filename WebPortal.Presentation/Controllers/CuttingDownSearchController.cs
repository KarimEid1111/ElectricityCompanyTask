using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebPortalDomain.Context;
using WebPortalDomain.Dtos;
using WebPortal.Models;
using WebPortalDomain.Entities;
using WebPortalDomain.Interfaces.Common;

namespace WebPortal.Controllers
{
    [Authorize]
    public class CuttingDownSearchController(IUnitOfWork unitOfWork) : Controller
    {
        // GET: Render the Search page with dropdown data and empty results
        [HttpGet]
        public async Task<IActionResult> Search()
        {
            ViewData["ShowCuttingPortalNav"] = true;
            ViewData["ActivePage"] = "Search";
            var model = new SearchFiltersViewModel
            {
                Sources = (await unitOfWork.ChannelRepository.GetAllAsync())
                    .Select(x => new SelectListItem { Value = x.ChannelKey.ToString(), Text = x.ChannelName })
                    .ToList(),

                ProblemTypes = (await unitOfWork.FtaProblemTypeRepository.GetAllAsync())
                    .Select(x => new SelectListItem { Value = x.ProblemTypeKey.ToString(), Text = x.ProblemTypeName })
                    .ToList(),

                SearchCriteriaList = (await unitOfWork.NetworkElementTypeRepository.GetAllAsync())
                    .Select(x => new SelectListItem
                    {
                        Value = x.NetworkElementTypeKey.ToString(),
                        Text = x.NetworkElementTypeName
                    })
                    .ToList()
            };

            return View(new SearchPageViewModel
            {
                Items = Enumerable.Empty<CuttingDownResultViewModel>(),
                Filters = model
            });
        }


        [HttpGet]
        public async Task<IActionResult> GetRecordDetails(int id)
        {
            var record = (await unitOfWork.CuttingDownHeaderRepository.GetAsync(x => x.CuttingDownKey == id))
                .Select(x => new
                {
                    x.CuttingDownKey,
                    x.CuttingDownIncidentId,
                    x.ChannelKey,
                    x.CuttingDownProblemTypeKey,
                    x.IsActive,
                    x.ActualEndDate
                })
                .FirstOrDefault();

            if (record == null) return NotFound();

            return Json(record);
        }

        // POST: Process the search request and return results
        [HttpPost]
        public async Task<IActionResult> Search(SearchFiltersViewModel filters)
        {
            ViewData["ShowCuttingPortalNav"] = true;
            ViewData["ActivePage"] = "Search";
            if (!ModelState.IsValid)
            {
                // Repopulate dropdowns if the request is invalid
                filters.Sources = (await unitOfWork.ChannelRepository.GetAllAsync())
                    .Select(x => new SelectListItem { Value = x.ChannelKey.ToString(), Text = x.ChannelName })
                    .ToList();

                filters.ProblemTypes = (await unitOfWork.FtaProblemTypeRepository.GetAllAsync())
                    .Select(x => new SelectListItem { Value = x.ProblemTypeKey.ToString(), Text = x.ProblemTypeName })
                    .ToList();

                filters.SearchCriteriaList = (await unitOfWork.NetworkElementTypeRepository.GetAllAsync())
                    .Select(x => new SelectListItem
                        { Value = x.NetworkElementTypeKey.ToString(), Text = x.NetworkElementTypeName })
                    .ToList();


                // Render the page with empty results initially
                return View(new SearchPageViewModel
                {
                    Items = [],
                    Filters = filters
                });
            }

            var result = await unitOfWork.CuttingDownHeaderRepository.SearchCuttingDownIncidents(filters.SourceOfCuttingDown,
                filters.ProblemTypeKey, filters.IsClosed, filters.SearchCriteria, filters.SearchValue);

            // Populate Channels dropdown
            filters.Sources = (await unitOfWork.ChannelRepository
                    .GetAllAsync()) // Use IQueryable for deferred execution
                .Select(x => new SelectListItem
                {
                    Value = x.ChannelKey.ToString(),
                    Text = x.ChannelName
                })
                .ToList(); // Execute the query on the database

// Populate Problem Types dropdown
            filters.ProblemTypes = await unitOfWork.FtaProblemTypeRepository
                .GetAllAsQueryable() // Use IQueryable for deferred execution
                .Select(x => new SelectListItem
                {
                    Value = x.ProblemTypeKey.ToString(),
                    Text = x.ProblemTypeName
                })
                .ToListAsync(); // Execute the query on the database

// Populate Search Criteria List
            filters.SearchCriteriaList = await unitOfWork.NetworkElementTypeRepository
                .GetAllAsQueryable() // Use IQueryable for deferred execution
                .Select(x => new SelectListItem
                {
                    Value = x.NetworkElementTypeKey.ToString(),
                    Text = x.NetworkElementTypeName
                })
                .ToListAsync(); // Execute the query on the database

            // Return the updated view
            return View(new SearchPageViewModel
            {
                Items = result
                    .Select(x => new CuttingDownResultViewModel
                    {
                        CuttingDownKey = x.CuttingDownKey,
                        CuttingDownIncidentId = x.CuttingDownIncidentId,
                        ChannelKey = x.ChannelKey,
                        CuttingDownProblemTypeKey = x.CuttingDownProblemTypeKey,
                        ActualCreateDate = x.ActualCreateDate,
                        SynchCreateDate = x.SynchCreateDate,
                        SynchUpdateDate = x.SynchUpdateDate,
                        ActualEndDate = x.ActualEndDate,
                        IsPlanned = x.IsPlanned,
                        IsGlobal = x.IsGlobal,
                        PlannedStartDts = x.PlannedStartDts,
                        PlannedEndDts = x.PlannedEndDts,
                        IsActive = x.IsActive,
                        CreateSystemUserId = x.CreateSystemUserId,
                        UpdateSystemUserId = x.UpdateSystemUserId
                    }),
                Filters = filters
            });
        }

        // POST: Close a single case
        [HttpPost]
        public async Task<IActionResult> CloseCase(int id)
        {
            var caseToClose = await unitOfWork.CuttingDownHeaderRepository.GetByIdAsync(id);

            caseToClose.IsActive = false;
            caseToClose.ActualEndDate = DateOnly.FromDateTime(DateTime.Now);
            await unitOfWork.SaveChangesAsync();


            return RedirectToAction("Search");
        }


        // POST: Close all cases
        [HttpPost]
        public async Task<IActionResult> CloseAllCases(List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return RedirectToAction("Search");
            }

            var casesToClose = (await unitOfWork.CuttingDownHeaderRepository
                    .GetAsync(x => ids.Contains(x.CuttingDownKey) && x.IsActive == true && x.ActualEndDate == null))
                .ToList();

            foreach (var openCase in casesToClose)
            {
                openCase.IsActive = false;
                openCase.ActualEndDate = DateOnly.FromDateTime(DateTime.Now);
            }

            await unitOfWork.SaveChangesAsync();
            return RedirectToAction("Search");
        }

        // GET: Render the Ignored Outages page
        [HttpGet]
        public async Task<IActionResult> IgnoredOutages()
        {
            ViewData["ActivePage"] = "IgnoredOutages"; // Set active navigation
            var ignoredOutagesEntities =
                await unitOfWork.CuttingDownHeaderRepository.GetAsync(x => x.IsGlobal == true);

            var ignoredOutages = ignoredOutagesEntities
                .Select(x => new CuttingDownResultViewModel
                {
                    CuttingDownKey = x.CuttingDownKey,
                    CuttingDownIncidentId = x.CuttingDownIncidentId,
                    ChannelKey = x.ChannelKey,
                    CuttingDownProblemTypeKey = x.CuttingDownProblemTypeKey,
                    ActualCreateDate = x.ActualCreateDate.HasValue
                        ? x.ActualCreateDate.Value.ToDateTime(TimeOnly.MinValue)
                        : (DateTime?)null,
                    IsActive = x.IsActive
                })
                .ToList();

            return View("IgnoredOutages", ignoredOutages);
        }

        // GET: Render the Add Outage page
        [HttpGet]
        public IActionResult AddOutage()
        {
            ViewData["ActivePage"] = "AddOutage"; // Set active navigation
            return View("AddOutage");
        }

        // POST: Add a new outage
        // [HttpPost]
        // public async Task<IActionResult> AddOutage(AddOutageViewModel model)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return View("AddOutage", model); // Return the form with validation errors
        //     }
        //
        //     var newOutage = new CuttingDownHeader
        //     {
        //         CuttingDownIncidentId = model.IncidentId,
        //         ChannelKey = model.ChannelKey,
        //         CuttingDownProblemTypeKey = model.ProblemTypeKey,
        //         ActualCreateDate = DateOnly.FromDateTime(DateTime.Now),
        //         IsActive = true,
        //         IsPlanned = model.IsPlanned
        //     };
        //
        //     _unitOfWork.CuttingDownHeaders.Add(newOutage);
        //     await _unitOfWork.SaveChangesAsync();
        //
        //     return RedirectToAction("Search");
        // }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }
    }
}