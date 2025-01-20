using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebPortal.Models;

public class SearchFiltersViewModel
{
    public string SourceOfCuttingDown { get; set; } // Dropdown for source
    public string ProblemTypeKey { get; set; }      // Dropdown for problem type
    public string IsClosed { get; set; }           // Dropdown for status (Closed/Open)
    public string SearchCriteria { get; set; }     // Dropdown for search criteria
    public string SearchValue { get; set; }        // Text input for search value

    public List<SelectListItem> Sources { get; set; }         // Options for "Source of Cutting Down"
    public List<SelectListItem> ProblemTypes { get; set; }    // Options for "Problem Type"
    public List<SelectListItem> SearchCriteriaList { get; set; } // Options for "Search Criteria"
}