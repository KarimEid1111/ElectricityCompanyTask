using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebPortal.Models;

public class NetworkSelectionViewModel
{
    public int? IncidentId { get; set; }
    public int? ChannelKey { get; set; }
    public int? ProblemTypeKey { get; set; }
    public bool IsPlanned { get; set; }

    public List<SelectListItem> ProblemTypes { get; set; } = new();
    public List<SelectListItem> NetworkHierarchies { get; set; } = new();
    public List<SelectListItem> SearchCriteria { get; set; } = new();
}
